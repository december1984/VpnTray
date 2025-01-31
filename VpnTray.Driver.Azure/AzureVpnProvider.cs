using System;
using System.IO;
using System.Linq;
using VpnTray.Domain;

namespace VpnTray.Driver.Azure
{
    public class AzureVpnProvider : VpnProvider
    {
        private static string _phonebookPath = 
            Path.Combine(Directory.GetDirectories(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Packages"), "Microsoft.AzureVpn*").FirstOrDefault() ?? string.Empty, 
                "LocalState", "rasphone.pbk");

        public AzureVpnProvider(ISystemEventsProvider systemEventsProvider)
            : base(new AzureVpnEnumeratorDriver(_phonebookPath), new AzureVpnConnectorDriver(_phonebookPath), new VpnMonitorDriver(), systemEventsProvider)
        {
            Name = "Azure VPN";
        }
        public override string Name { get; protected set; }
    }
}
