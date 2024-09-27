using System.IO.Pipes;

namespace Quest.Engine.Server.Interfaces;
public interface IServer
{
    Task HandleRequest(NamedPipeServerStream pipeServer);

}
