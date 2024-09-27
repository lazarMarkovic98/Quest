using System;
using System.Collections.Generic;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quest.Engine.Server.Interfaces;
public interface IServer
{
    Task HandleRequest(NamedPipeServerStream pipeServer);

}
