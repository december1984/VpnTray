using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VpnTray.Domain;

namespace VpnTray.Driver.Cisco.AnyConnect
{
    public class VpnConnectorDriver : IVpnConnectorDriver
    {
        public async Task Connect(string id)
        {
            var vpnId = VpnId.Parse(id);
            await VpnCli.Connect(vpnId.Name, $"CiscoAnyConnect\\{vpnId.Name}.txt");
        }

        public async Task Disconnect(string id)
        {
            var vpnId = VpnId.Parse(id);
            var stats = await VpnCli.GetStats();
            if (stats.Server == vpnId.Server)
            {
                await VpnCli.Disconnect();
            }
        }
    }
}
