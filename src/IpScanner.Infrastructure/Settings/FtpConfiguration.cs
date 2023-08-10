namespace IpScanner.Infrastructure.Settings
{
    public class FtpConfiguration
    {
        public FtpConfiguration(string ftpAddress, string username, string password)
        {
            FtpAddress = ftpAddress;
            Username = username;
            Password = password;
        }

        public string FtpAddress { get; }
        public string Username { get; }
        public string Password { get; }
    }
}
