using System;
using System.Collections.Generic;

namespace VpnTray.ViewModels.Configuration
{
    public class VpnTrayFormConfiguration
    {
        public TimeSpan EnumeratorRefreshInterval { get; set; }
        public TimeSpan DefaultMonitorRefreshInterval { get; set; }
        public Dictionary<string, List<VpnTraySettingsEntryConfiguration>> Entries { get; } = new Dictionary<string, List<VpnTraySettingsEntryConfiguration>>();
    }
}
