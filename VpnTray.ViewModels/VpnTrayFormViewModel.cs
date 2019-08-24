using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VpnTray.Domain;
using VpnTray.ViewModels.Configuration;

namespace VpnTray.ViewModels
{
    public class VpnTrayFormViewModel : BaseViewModel
    {
        private readonly VpnEnumerator _vpnEnumerator;
        private readonly VpnManagerFactory _vpnManagerFactory;
        private readonly IVpnTrayConfigurationProvider _configurationProvider;

        public ObservableCollection<VpnTraySettingsEntryViewModel> Settings { get; }

        public ObservableCollection<VpnTrayIconViewModel> Icons { get; }

        public VpnTrayFormViewModel(VpnEnumerator vpnEnumerator, VpnManagerFactory vpnManagerFactory, IVpnTrayConfigurationProvider configurationProvider)
        {
            _vpnEnumerator = vpnEnumerator;
            _vpnManagerFactory = vpnManagerFactory;
            _configurationProvider = configurationProvider;

            Settings = new ObservableCollection<VpnTraySettingsEntryViewModel>();
            Icons = new ObservableCollection<VpnTrayIconViewModel>();

            vpnEnumerator.Vpns.Clear();
            foreach (var entry in configurationProvider.Configuration.Entries)
            {
                var vpn = new Vpn(entry.Id, entry.Name);
                vpnEnumerator.Vpns.Add(vpn);

                var vm = new VpnTraySettingsEntryViewModel(vpnManagerFactory.Create(vpn), entry);
                vm.PropertyChanged += Entry_PropertyChanged;
                Settings.Add(vm);

                if (vm.IsSelected)
                {
                    Icons.Add(new VpnTrayIconViewModel(vm.VpnManager));
                }
            }

            _vpnEnumerator.Added += (s, e) => AddManager(e.Vpn);
            _vpnEnumerator.Removed += (s, e) => RemoveManager(e.Vpn);

            vpnEnumerator.RefreshInterval = configurationProvider.Configuration.EnumeratorRefreshInterval;
            vpnEnumerator.IsEnabled = true;
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

                        _configurationProvider.Configuration.Entries.Single(s => s.Id == entry.Id).IsSelected =
                            entry.IsSelected;
                        break;
                    }
                case nameof(VpnTraySettingsEntryViewModel.Name):
                    _configurationProvider.Configuration.Entries.Single(s => s.Id == entry.Id).Name = entry.Name;
                    break;
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

            _configurationProvider.Configuration.Entries.RemoveAll(e => e.Id == vpn.Id);
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

            _configurationProvider.Configuration.Entries.Add(configuration);
            _configurationProvider.Save();

            var entry = new VpnTraySettingsEntryViewModel(_vpnManagerFactory.Create(vpn), configuration);
            entry.PropertyChanged += Entry_PropertyChanged;
            Settings.Add(entry);
        }

    }
}
