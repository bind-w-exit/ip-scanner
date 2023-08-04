namespace IpScanner.Ui.Messages
{
    public abstract class VisibilityMessageBase
    {
        public VisibilityMessageBase(bool isVisible)
        {
            Visible = isVisible;
        }

        public bool Visible { get; }
    }
}
