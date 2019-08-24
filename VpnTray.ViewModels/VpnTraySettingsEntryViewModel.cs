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
            VpnManager.Vpn.NameChanged += (s, e) => OnPropertyChanged(nameof(Name));

            RefreshInterval = configuration.RefreshInterval;
            IsSelected = configuration.IsSelected;
        }

        public string Id => VpnManager.Vpn.Id;
        public string Name => VpnManager.Vpn.Name;

        public bool IsSelected
        {
            get => VpnManager.Monitor.IsEnabled;
            set
            {
                if (VpnManager.Monitor.IsEnabled == value) return;
                VpnManager.Monitor.IsEnabled = value;
                OnPropertyChanged();
            }
        }

        public TimeSpan RefreshInterval
        {
            get => VpnManager.Monitor.RefreshInterval;
            set => VpnManager.Monitor.RefreshInterval = value;
        }
    }
}
