using FluentResults;
using System.Net;

namespace IpScanner.Infrastructure.Services
{
    public interface ITelnetService
    {
        Result OpenTelnetSession(IPAddress address);
    }
}
