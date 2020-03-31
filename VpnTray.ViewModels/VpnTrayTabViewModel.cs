using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using VpnTray.Domain;
using VpnTray.ViewModels.Configuration;

namespace VpnTray.ViewModels
{
    public class VpnTrayTabViewModel : BaseViewModel
    {
        private readonly VpnProvider _vpnProvider;
        private readonly IVpnTrayConfigurationProvider _configurationProvider;

        public string Name => _vpnProvider.Name;

        public ObservableCollection<VpnTraySettingsEntryViewModel> Settings { get; }

        public ObservableCollection<VpnTrayIconViewModel> Icons { get; }

        public VpnTrayTabViewModel(VpnProvider vpnProvider, IVpnTrayConfigurationProvider configurationProvider)
        {
            _vpnProvider = vpnProvider;
            _configurationProvider = configurationProvider;

            Settings = new ObservableCollection<VpnTraySettingsEntryViewModel>();
            Icons = new ObservableCollection<VpnTrayIconViewModel>();

            vpnProvider.VpnEnumerator.Vpns.Clear();
            if (!configurationProvider.Configuration.Entries.TryGetValue(vpnProvider.Name, out var entries))
            {
                configurationProvider.Configuration.Entries[vpnProvider.Name] = entries = new List<VpnTraySettingsEntryConfiguration>();
                configurationProvider.Save();
            }
            foreach (var entry in entries)
            {
                var vpn = new Vpn(entry.Id, entry.Name);
                vpnProvider.VpnEnumerator.Vpns.Add(vpn);

                var vm = new VpnTraySettingsEntryViewModel(vpnProvider.VpnManagerFactory.Create(vpn), entry);
                vm.PropertyChanged += Entry_PropertyChanged;
                Settings.Add(vm);

                if (vm.IsSelected)
                {
                    Icons.Add(new VpnTrayIconViewModel(vm.VpnManager));
                }
            }

            vpnProvider.VpnEnumerator.Added += (s, e) => AddManager(e.Vpn);
            vpnProvider.VpnEnumerator.Removed += (s, e) => RemoveManager(e.Vpn);

            vpnProvider.VpnEnumerator.RefreshInterval = configurationProvider.Configuration.EnumeratorRefreshInterval;
            vpnProvider.VpnEnumerator.IsEnabled = true;
        }

        private void Entry_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var entry = sender as VpnTraySettingsEntryViewModel;

            if (entry == null) return;

            switch (e.PropertyName)
            {
                case nameof(VpnTraySettingsEntryViewModel.IsSelected):
                    {
                        if (entry.IsSelected)
                        {
                            Icons.Add(new VpnTrayIconViewModel(entry.VpnManager));
                        }
                        else
                        {
                            var icon = Icons.SingleOrDefault(i => i.VpnManager == entry.VpnManager);
                            if (icon != null)
                            {
                                Icons.Remove(icon);
                            }
                        }

                        break;
                    }
            }

            _configurationProvider.Save();
        }

        private void RemoveManager(Vpn vpn)
        {
            var settingsEntry = Settings.FirstOrDefault(s => s.VpnManager.Vpn.Id == vpn.Id);
            if (settingsEntry == null) return;

            settingsEntry.IsSelected = false;
            settingsEntry.PropertyChanged -= Entry_PropertyChanged;
            Settings.Remove(settingsEntry);

            _configurationProvider.Configuration.Entries[_vpnProvider.Name].RemoveAll(e => e.Id == vpn.Id);
            _configurationProvider.Save();
        }

        private void AddManager(Vpn vpn)
        {
            var configuration = new VpnTraySettingsEntryConfiguration
            {
                Id = vpn.Id,
                Name = vpn.Name,
                IsSelected = false,
                RefreshInterval = _configurationProvider.Configuration.DefaultMonitorRefreshInterval
            };

            _configurationProvider.Configuration.Entries[_vpnProvider.Name].Add(configuration);
            _configurationProvider.Save();

            var entry = new VpnTraySettingsEntryViewModel(_vpnProvider.VpnManagerFactory.Create(vpn), configuration);
            entry.PropertyChanged += Entry_PropertyChanged;
            Settings.Add(entry);
        }

    }
}
