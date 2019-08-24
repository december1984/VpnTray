using System;
using System.Collections.Generic;
using System.Text;

namespace VpnTray.Domain
{
    public class VpnManager
    {
        public VpnManager(Vpn vpn, IVpnConnectorDriver vpnConnectorDriver, IVpnMonitorDriver vpnMonitorDriver)
        {
            Vpn = vpn;
            Connector = new VpnConnector(vpn, vpnConnectorDriver);
            Monitor = new VpnMonitor(vpn, vpnMonitorDriver);
        }

        public Vpn Vpn { get; }
        public VpnConnector Connector { get; }
        public VpnMonitor Monitor { get; }
    }
}
