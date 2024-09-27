using Microsoft.Extensions.DependencyInjection;
using Quest.Engine.Configuration;
using Quest.Engine.Server.Interfaces;
using System.IO.Pipes;

var services = ApplicationConfiguration.Initialize();

Console.WriteLine("Starting Named Pipe Server...");


while (true)
{
    NamedPipeServerStream pipeServer = new NamedPipeServerStream("QuestPipe", PipeDirection.InOut, NamedPipeServerStream.MaxAllowedServerInstances);
    Console.WriteLine("Quest Pipe Server is waiting for connection...");

    pipeServer.WaitForConnection();

    Console.WriteLine("Client connected.");
    var server = services.GetRequiredService<IServer>();

    Thread clientThread = new Thread(() => server.HandleRequest(pipeServer));
    clientThread.Start();
}