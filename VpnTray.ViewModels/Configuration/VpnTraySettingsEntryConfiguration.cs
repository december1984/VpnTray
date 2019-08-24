using System;

namespace VpnTray.ViewModels.Configuration
{
    public class VpnTraySettingsEntryConfiguration
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public bool IsSelected { get; set; }
        public TimeSpan RefreshInterval { get; set; }
    }
}