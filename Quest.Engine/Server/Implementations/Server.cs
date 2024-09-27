using AutoMapper;
using Newtonsoft.Json;
using Quest.Engine.Interfaces;
using Quest.Engine.Server.Interfaces;
using Quest.Infrastructure.Helper;
using System.IO.Pipes;

namespace Quest.Engine.Server.Implementations;
public class Server(IEngine engine, IMapper mapper) : IServer
{
    private readonly IEngine _engine = engine;
    private readonly IMapper _mapper = mapper;

    public async Task HandleRequest(NamedPipeServerStream pipeServer)
    {
        try
        {
            var reader = new StreamReader(pipeServer);
            var writer = new StreamWriter(pipeServer);
            {
                writer.AutoFlush = true;

                string clientMessage = reader.ReadLine()!;

                var request = JsonConvert.DeserializeObject<CheckFilesInputDto>(clientMessage);

                var result = _mapper.Map<List<ResultDto>>(await _engine.GenerateReports(request!));

                var jsonResult = JsonConvert.SerializeObject(result);

                writer.WriteLine(jsonResult);
            }

        }
        catch (Exception ex)
        {
            Console.WriteLine("Unexpected error: " + ex.Message);
        }
        finally
        {
            pipeServer.Close();
        }
    }
}
