using System.Diagnostics;

namespace IpScanner.Infrastructure.Services
{
    public class CmdService : ICmdService
    {
        public void Execute(string command)
        {
            Process.Start("cmd.exe", $"/k {command}");
        }
    }
}
