namespace IpScanner.Ui.Messages
{
    public class DetailsPageVisibilityMessage
    {
        public DetailsPageVisibilityMessage(bool visible)
        {
            Visible = visible;
        }

        public bool Visible { get; }
    }
}
