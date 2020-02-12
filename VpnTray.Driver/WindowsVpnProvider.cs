using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VpnTray.Domain;

namespace VpnTray.Driver
{
    public class WindowsVpnProvider : VpnProvider
    {
        public WindowsVpnProvider(ISystemEventsProvider systemEventsProvider)
            : base(new VpnEnumeratorDriver(), new VpnConnectorDriver(), new VpnMonitorDriver(), systemEventsProvider)
        {
            Name = "Windows VPN";
        }
        public override string Name { get; protected set; }
    }
}
