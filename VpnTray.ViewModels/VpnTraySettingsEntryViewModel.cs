using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VpnTray.Domain;
using VpnTray.ViewModels.Configuration;

namespace VpnTray.ViewModels
{
    public class VpnTraySettingsEntryViewModel : BaseViewModel
    {
        private readonly VpnTraySettingsEntryConfiguration _configuration;
        internal VpnManager VpnManager { get; }

        public VpnTraySettingsEntryViewModel(VpnManager vpnManager, VpnTraySettingsEntryConfiguration configuration)
        {
            _configuration = configuration;

            VpnManager = vpnManager;
            VpnManager.Vpn.NameChanged += (s, e) => { Name = VpnManager.Vpn.Name; };

            VpnManager.Monitor.RefreshInterval = configuration.RefreshInterval;
            VpnManager.Monitor.IsEnabled = configuration.IsSelected;
            VpnManager.DisconnectOnLock = configuration.DisconnectOnLock;
            VpnManager.ReconnectOnUnlock = configuration.ReconnectOnUnlock;
        }

        public string Id => VpnManager.Vpn.Id;
        public string Name
        {
            get => _configuration.Name;
            set
            {
                if (_configuration.Name == value) return;
                _configuration.Name = value;
                OnPropertyChanged();
            }
        }

        public bool IsSelected
        {
            get => _configuration.IsSelected;
            set
            {
                if (_configuration.IsSelected == value) return;
                _configuration.IsSelected = value;
                VpnManager.Monitor.IsEnabled = value;
                OnPropertyChanged();
            }
        }

        public bool DisconnectOnLock
        {
            get => _configuration.DisconnectOnLock;
            set
            {
                if (_configuration.DisconnectOnLock == value) return;
                _configuration.DisconnectOnLock = value;
                VpnManager.DisconnectOnLock = value;
                OnPropertyChanged();
            }
        }

        public bool ReconnectOnUnlock
        {
            get => _configuration.ReconnectOnUnlock;
            set
            {
                if (_configuration.ReconnectOnUnlock == value) return;
                _configuration.ReconnectOnUnlock = value;
                VpnManager.ReconnectOnUnlock = value;
                OnPropertyChanged();
            }
        }

        public TimeSpan RefreshInterval
        {
            get => _configuration.RefreshInterval;
            set
            {
                if (_configuration.RefreshInterval == value) return;
                _configuration.RefreshInterval = value;
                VpnManager.Monitor.RefreshInterval = value;
                OnPropertyChanged();
            }
        }
    }
}
