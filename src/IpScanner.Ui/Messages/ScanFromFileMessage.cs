namespace IpScanner.Ui.Messages
{
    public class ScanFromFileMessage
    {
        public ScanFromFileMessage(string content)
        {
            Content = content;
        }

        public string Content { get; }
    }
}
