using Microsoft.Extensions.Options;
using Moq;
using Newtonsoft.Json;
using Quest.Infrastructure.Helper;
using Quest.WPF.Configuration;
using System.Collections.ObjectModel;
using System.IO.Pipes;

namespace Quest.WPF.Tests.Tests;

public class ClientTests
{
    private Mock<IOptionsMonitor<SetupOptions>> _mockOptionsMonitor;
    private Client.Implementations.Client _client;

    public ClientTests()
    {
        _mockOptionsMonitor = new Mock<IOptionsMonitor<SetupOptions>>();
        _client = new Client.Implementations.Client(_mockOptionsMonitor.Object);
    }

    [Fact]
    public async Task ShowReports_ReturnsReports_WhenServerResponds()
    {
        // Arrange
        var setupOptions = new SetupOptions
        {
            Folders = new List<string> { "testFolder" },
            MaximumConcurentProcessingJobs = 5
        };

        _mockOptionsMonitor
            .Setup(m => m.CurrentValue)
            .Returns(setupOptions);

        var serverTask = Task.Run(() => StartNamedPipeServer());

        // Act
        var result = _client.ShowReports();

        await serverTask;

        // Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result);
    }

    private void StartNamedPipeServer()
    {
        using (var pipeServer = new NamedPipeServerStream("QuestPipe", PipeDirection.InOut))
        {
            pipeServer.WaitForConnection();

            using (var reader = new StreamReader(pipeServer))
            using (var writer = new StreamWriter(pipeServer) { AutoFlush = true })
            {
                var message = reader.ReadLine()!;
                var request = JsonConvert.DeserializeObject<CheckFilesInputDto>(message);

                var results = new ObservableCollection<ResultDto>
                {
                    new ResultDto { Status = "Ok", NumberOfComponents = 2, FileName = "report1.json" },
                    new ResultDto { Status = "", NumberOfComponents = 0, FileName = "report2.json" }
                };

                string jsonResponse = JsonConvert.SerializeObject(results);
                writer.WriteLine(jsonResponse);
            }
        }
    }
}