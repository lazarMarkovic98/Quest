using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Quest.Infrastructure.Helper;
using Quest.WPF.Client.Interfaces;
using Quest.WPF.Configuration;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.IO.Pipes;
using System.Windows;

namespace Quest.WPF.Client.Implementations;
public class Client : IClient
{
    private IOptionsMonitor<SetupOptions> _setupOptions;
    public Client(IOptionsMonitor<SetupOptions> setupOptions)
    {
        _setupOptions = setupOptions;
    }
    public ObservableCollection<ResultDto> ShowReports()
    {
        var result = new ObservableCollection<ResultDto>();
        try
        {
            using (NamedPipeClientStream pipeClient = new NamedPipeClientStream(".", "QuestPipe", PipeDirection.InOut))
            {

                pipeClient.Connect(1000);

                var writer = new StreamWriter(pipeClient);
                var reader = new StreamReader(pipeClient);

                writer.AutoFlush = true;
                var request = new CheckFilesInputDto
                {
                    Folders = _setupOptions.CurrentValue.Folders,
                    MaximumConcurentProcessingJobs = _setupOptions.CurrentValue.MaximumConcurentProcessingJobs
                };

                string messageToSend = JsonConvert.SerializeObject(request);
                writer.WriteLine(messageToSend);

                string serverResponse = reader.ReadLine()!;
                result = serverResponse is not null ? JsonConvert.DeserializeObject<ObservableCollection<ResultDto>>(serverResponse): new();

            }
        }
        catch(TimeoutException)
        {
            throw;
        }
        catch (Exception ex)
        {
            MessageBox.Show("Error: " + ex.Message);
        }
        return result!;

    }
}
