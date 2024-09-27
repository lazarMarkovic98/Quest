namespace Quest.Infrastructure.Helper;
public class ClientTimeoutException : Exception
{
    public ClientTimeoutException(string? message) : base(message) { }
}
