using CommunityToolkit.Mvvm.ComponentModel;
using IpScanner.Domain.Enums;
using System;

namespace IpScanner.Ui.ViewModels.Modules
{
    public class ProgressModule : ObservableObject
    {
        private int _progress;
        private int _countOfUnknownDevices;
        private int _countOfOnlineDevices;
        private int _countOfOfflineDevices;
        private int _countOfScannedIps;

        public ProgressModule()
        {
            CountOfScannedIps = 0;
            CountOfUnknownDevices = 0;
            CountOfOnlineDevices = 0;
            CountOfOfflineDevices = 0;
            TotalCountOfIps = int.MaxValue;
        }

        public int CountOfScannedIps
        {
            get => _progress;
            set
            {
                if (SetProperty(ref _progress, value))
                {
                    OnPropertyChanged(nameof(ProgressString));
                }
            }
        }

        public int TotalCountOfIps
        {
            get => _countOfScannedIps;
            set => SetProperty(ref _countOfScannedIps, value);
        }

        public int CountOfUnknownDevices
        {
            get => _countOfUnknownDevices;
            set
            {
                if (SetProperty(ref _countOfUnknownDevices, value))
                {
                    OnPropertyChanged(nameof(ProgressString));
                }
            }
        }

        public int CountOfOnlineDevices
        {
            get => _countOfOnlineDevices;
            set => SetProperty(ref _countOfOnlineDevices, value);
        }

        public int CountOfOfflineDevices
        {
            get => _countOfOfflineDevices;
            set
            {
                if (SetProperty(ref _countOfOfflineDevices, value))
                {
                    OnPropertyChanged(nameof(ProgressString));
                }
            }
        }

        public string ProgressString
        {
            get => $"{CalculateProgress()}%, {CountOfOfflineDevices} dead, {CountOfUnknownDevices} unknown";
        }

        public void UpdateProgress(int currentCount, DeviceStatus status)
        {
            CountOfScannedIps = currentCount;
            IncreaseCountOfSpecificDevices(status);
        }

        public void ResetProgress()
        {
            CountOfScannedIps = 0;
            CountOfUnknownDevices = 0;
            CountOfOnlineDevices = 0;
            CountOfOfflineDevices = 0;
            TotalCountOfIps = int.MaxValue;
        }

        private double CalculateProgress()
        {
            if (TotalCountOfIps == 0)
            {
                return 0;
            }

            return Math.Round(((double)CountOfScannedIps / TotalCountOfIps) * 100, 2);
        }

        private void IncreaseCountOfSpecificDevices(DeviceStatus status)
        {
            switch (status)
            {
                case DeviceStatus.Unknown:
                    CountOfUnknownDevices++;
                    break;
                case DeviceStatus.Online:
                    CountOfOnlineDevices++;
                    break;
                case DeviceStatus.Offline:
                    CountOfOfflineDevices++;
                    break;
            }
        }
    }
}
