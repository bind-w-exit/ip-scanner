namespace IpScanner.Ui.Messages
{
    public class ActionsBarVisibilityMessage
    {
        public ActionsBarVisibilityMessage(bool isVisible)
        {
            Visible = isVisible;
        }

        public bool Visible { get; }
    }
}
