using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
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

            try
            {
                var document = XDocument.Parse(Settings.Default.Entries);
                foreach (var providerElement in document.Root.Elements("Provider"))
                {
                    var entries = new List<VpnTraySettingsEntryConfiguration>();
                    foreach (var entryElement in providerElement.Elements("Entry"))
                    {
                        entries.Add(new VpnTraySettingsEntryConfiguration
                        {
                            Id = entryElement.Attribute("id").Value,
                            Name = entryElement.Attribute("name").Value,
                            RefreshInterval = (TimeSpan)entryElement.Attribute("refreshInterval"),
                            IsSelected = (bool)entryElement.Attribute("isSelected"),
                            DisconnectOnLock = (bool)entryElement.Attribute("disconnectOnLock"),
                            ReconnectOnUnlock = (bool)entryElement.Attribute("reconnectOnUnlock")
                        });
                    }
                    Configuration.Entries[providerElement.Attribute("name").Value] = entries;
                }
            }
            catch
            {
                Configuration.Entries.Clear();
            }
        }

        public void Save()
        {
            Settings.Default.EnumeratorRefreshInterval = Configuration.EnumeratorRefreshInterval;
            Settings.Default.DefaultMonitorRefreshInterval = Configuration.DefaultMonitorRefreshInterval;
            var document = new XDocument(
                new XElement("Entries", Configuration.Entries.Select(p => 
                    new XElement("Provider", 
                        new XAttribute("name", p.Key), p.Value.Select(e => 
                        new XElement("Entry",
                            new XAttribute("id", e.Id),
                            new XAttribute("name", e.Name),
                            new XAttribute("refreshInterval", e.RefreshInterval),
                            new XAttribute("isSelected", e.IsSelected),
                            new XAttribute("disconnectOnLock", e.DisconnectOnLock),
                            new XAttribute("reconnectOnUnlock", e.ReconnectOnUnlock)))))));
            Settings.Default.Entries = document.ToString();
            Settings.Default.Save();
        }
    }
}
