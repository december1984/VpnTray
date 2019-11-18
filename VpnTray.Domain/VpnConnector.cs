using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace VpnTray.Domain
{
    public class VpnConnector
    {
        private readonly IVpnConnectorDriver _vpnConnectorDriver;
        public Vpn Vpn { get; }

        public VpnConnector(Vpn vpn, IVpnConnectorDriver vpnConnectorDriver)
        {
            _vpnConnectorDriver = vpnConnectorDriver ?? throw new ArgumentNullException(nameof(vpnConnectorDriver));
            Vpn = vpn ?? throw new ArgumentNullException(nameof(vpn));
        }

        public async Task Connect()
        {
            await _vpnConnectorDriver.Connect(Vpn.Id);
        }

        public async Task Disconnect()
        {
            await _vpnConnectorDriver.Disconnect(Vpn.Id);
        }
    }
}
