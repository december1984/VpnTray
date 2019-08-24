using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VpnTray.Properties;
using VpnTray.ViewModels.Configuration;

namespace VpnTray
{
    class SettingsConfigurationProvider : IVpnTrayConfigurationProvider
    {
        public VpnTrayFormConfiguration Configuration { get; }

        public SettingsConfigurationProvider()
        {
            Configuration = new VpnTrayFormConfiguration
            {
                EnumeratorRefreshInterval = Settings.Default.EnumeratorRefreshInterval,
                DefaultMonitorRefreshInterval = Settings.Default.DefaultMonitorRefreshInterval,
            };

            var entries = Settings.Default.Entries?
                .OfType<string>()
                .Select(s =>
                {
                    var parts = s.Split(';');
                    return new VpnTraySettingsEntryConfiguration
                    {
                        Id = parts[0],
                        Name = parts[1],
                        RefreshInterval = TimeSpan.Parse(parts[2]),
                        IsSelected = bool.Parse(parts[3])
                    };
                })
                .ToArray() ?? new VpnTraySettingsEntryConfiguration[0];
            Configuration.Entries.AddRange(entries);
        }

        public void Save()
        {
            Settings.Default.EnumeratorRefreshInterval = Configuration.EnumeratorRefreshInterval;
            Settings.Default.DefaultMonitorRefreshInterval = Configuration.DefaultMonitorRefreshInterval;
            Settings.Default.Entries = new StringCollection();
            Settings.Default.Entries.AddRange(Configuration.Entries.Select(e => $"{e.Id};{e.Name};{e.RefreshInterval};{e.IsSelected}").ToArray());
            Settings.Default.Save();
        }
    }
}
