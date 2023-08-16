using IpScanner.Ui.Enums;

namespace IpScanner.Ui.Messages
{
    public class SetFocusToCellMessage
    {
        public SetFocusToCellMessage(DeviceRow row, bool favorite)
        {
            Row = row;
            Favorite = favorite;
        }

        public bool Favorite { get;  }
        public DeviceRow Row { get; }
    }
}
