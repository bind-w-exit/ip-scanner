using FluentResults;
using System.Diagnostics;
using System;
using System.Net;

namespace IpScanner.Infrastructure.Services
{
    public class TelnetService : ITelnetService
    {
        public Result OpenTelnetSession(IPAddress address)
        {
            if (address == null)
            {
                throw new ArgumentNullException(nameof(address));
            }

            try
            {
                var telnetProcess = new Process();

                telnetProcess.StartInfo.FileName = "telnet.exe";
                telnetProcess.StartInfo.Arguments = address.ToString();
                telnetProcess.StartInfo.UseShellExecute = false;
                telnetProcess.StartInfo.RedirectStandardInput = true;
                telnetProcess.StartInfo.RedirectStandardOutput = true;
                telnetProcess.StartInfo.CreateNoWindow = true;

                telnetProcess.Start();
                return Result.Ok();
            }
            catch (Exception ex)
            {
                return Result.Fail(ex.Message);
            }
        }
    }
}
