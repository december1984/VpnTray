using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace VpnTray.Domain
{
    public class VpnManager
    {
        private readonly ISystemEventsProvider _systemEventsProvider;
        private bool _disconnectedOnLock;

        public VpnManager(Vpn vpn, IVpnConnectorDriver vpnConnectorDriver, IVpnMonitorDriver vpnMonitorDriver, ISystemEventsProvider systemEventsProvider)
        {
            Vpn = vpn;
            _systemEventsProvider = systemEventsProvider;
            Connector = new VpnConnector(vpn, vpnConnectorDriver);
            Monitor = new VpnMonitor(vpn, vpnMonitorDriver);
            _systemEventsProvider.SessionLock += new EventHandler(async (s, e) => await _systemEventsProvider_SessionLock(s, e));
            _systemEventsProvider.SessionUnlock += new EventHandler(async (s, e) => await _systemEventsProvider_SessionUnlock(s, e));
        }

        private async Task _systemEventsProvider_SessionUnlock(object sender, EventArgs e)
        {
            if (ReconnectOnUnlock && _disconnectedOnLock)
            {
                await Connector.Connect();
                _disconnectedOnLock = false;
            }
        }

        private async Task _systemEventsProvider_SessionLock(object sender, EventArgs e)
        {
            if (DisconnectOnLock && Monitor.Status == VpnStatus.Connected)
            {
                await Connector.Disconnect();
                _disconnectedOnLock = true;
            }
        }

        public Vpn Vpn { get; }
        public VpnConnector Connector { get; }
        public VpnMonitor Monitor { get; }

        public bool DisconnectOnLock { get; set; }
        public bool ReconnectOnUnlock { get; set; }
    }
}
