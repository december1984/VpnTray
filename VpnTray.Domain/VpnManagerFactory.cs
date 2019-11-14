using System;
using System.Collections.Generic;
using System.Text;

namespace VpnTray.Domain
{
    public class VpnManagerFactory
    {
        private readonly IVpnConnectorDriver _vpnConnectorDriver;
        private readonly IVpnMonitorDriver _vpnMonitorDriver;
        private readonly ISystemEventsProvider _systemEventsProvider;

        public VpnManagerFactory(IVpnConnectorDriver vpnConnectorDriver, IVpnMonitorDriver vpnMonitorDriver, ISystemEventsProvider systemEventsProvider)
        {
            _vpnConnectorDriver = vpnConnectorDriver;
            _vpnMonitorDriver = vpnMonitorDriver;
            _systemEventsProvider = systemEventsProvider;
        }

        public VpnManager Create(Vpn vpn)
        {
            return new VpnManager(vpn, _vpnConnectorDriver, _vpnMonitorDriver, _systemEventsProvider);
        }
    }
}
