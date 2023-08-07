using Windows.Storage;

namespace IpScanner.Ui.Messages
{
    public class DevicesLoadedMessage
    {
        public DevicesLoadedMessage(StorageFile storageFile)
        {
            StorageFile = storageFile;
        }

        public StorageFile StorageFile { get; set; }
    }
}
