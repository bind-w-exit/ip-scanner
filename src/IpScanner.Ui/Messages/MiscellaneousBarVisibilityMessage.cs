namespace IpScanner.Ui.Messages
{
    public class MiscellaneousBarVisibilityMessage
    {
        public MiscellaneousBarVisibilityMessage(bool isVisible)
        {
            Visible = isVisible;
        }

        public bool Visible { get; }
    }
}
