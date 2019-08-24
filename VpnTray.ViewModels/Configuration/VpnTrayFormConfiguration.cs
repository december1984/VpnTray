using System;
using System.Collections.Generic;

namespace VpnTray.ViewModels.Configuration
{
    public class VpnTrayFormConfiguration
    {
        public TimeSpan EnumeratorRefreshInterval { get; set; }
        public TimeSpan DefaultMonitorRefreshInterval { get; set; }
        public List<VpnTraySettingsEntryConfiguration> Entries { get; } = new List<VpnTraySettingsEntryConfiguration>();
    }
}
