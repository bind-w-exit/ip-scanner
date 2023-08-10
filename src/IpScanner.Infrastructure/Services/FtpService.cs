using IpScanner.Infrastructure.Settings;
using System.Threading.Tasks;
using System.Net;

namespace IpScanner.Infrastructure.Services
{
    public class FtpService : IFtpService
    {
        public async Task<bool> ConnectAsync(FtpConfiguration configuration)
        {
            try
            {
                return await SendRequest(configuration);
            }
            catch (WebException ex)
            {
                return IsAuthenticationError(ex);
            }
        }

        private async Task<bool> SendRequest(FtpConfiguration configuration)
        {
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(configuration.FtpAddress);
            request.Method = WebRequestMethods.Ftp.ListDirectory;

            request.Credentials = new NetworkCredential(configuration.Username, configuration.Password);

            using (FtpWebResponse response = (FtpWebResponse)await request.GetResponseAsync())
            {
                if (response.StatusCode == FtpStatusCode.OpeningData || 
                    response.StatusCode == FtpStatusCode.AccountNeeded)
                {
                    return true;
                }
            }

            return false;
        }

        private bool IsAuthenticationError(WebException exception)
        {
            if (exception.Status == WebExceptionStatus.ProtocolError)
            {
                FtpWebResponse response = exception.Response as FtpWebResponse;
                if (response?.StatusCode == FtpStatusCode.NotLoggedIn)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
