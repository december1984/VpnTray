using System;
using System.Collections.Generic;
using System.Text;

namespace VpnTray.Domain
{
    public class VpnManagerFactory
    {
        private readonly IVpnConnectorDriver _vpnConnectorDriver;
        private readonly IVpnMonitorDriver _vpnMonitorDriver;

        public VpnManagerFactory(IVpnConnectorDriver vpnConnectorDriver, IVpnMonitorDriver vpnMonitorDriver)
        {
            _vpnConnectorDriver = vpnConnectorDriver;
            _vpnMonitorDriver = vpnMonitorDriver;
        }

        public VpnManager Create(Vpn vpn)
        {
            return new VpnManager(vpn, _vpnConnectorDriver, _vpnMonitorDriver);
        }
    }
}
