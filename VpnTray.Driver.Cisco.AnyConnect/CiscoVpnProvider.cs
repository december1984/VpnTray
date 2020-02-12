using System;
using System.Collections.Generic;
using System.Text;
using VpnTray.Domain;

namespace VpnTray.Driver.Cisco.AnyConnect
{
    public class CiscoVpnProvider : VpnProvider
    {
        public CiscoVpnProvider(ISystemEventsProvider systemEventsProvider)
            : base(new VpnEnumeratorDriver(), new VpnConnectorDriver(), new VpnMonitorDriver(), systemEventsProvider)
        {
            Name = "Cisco AnyConnect VPN";
        }
        public override string Name { get; protected set; }
    }
}
