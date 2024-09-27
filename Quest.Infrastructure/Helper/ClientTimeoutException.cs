using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quest.Infrastructure.Helper;
public class ClientTimeoutException : Exception
{
    public ClientTimeoutException(string? message) : base(message) { }
}
