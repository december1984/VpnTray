using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using VpnTray.Domain;

namespace VpnTray.Driver.Cisco.AnyConnect
{
    public class VpnMonitorDriver : IVpnMonitorDriver
    {
        public async Task<IPAddress> GetIpAddress(string id)
        {
            var vpnId = VpnId.Parse(id);
            var stats = await VpnCli.GetStats();
            if (stats.Server == vpnId.Server && IPAddress.TryParse(stats.Ip, out var ipaddress))
            {
                return ipaddress;
            }
            return IPAddress.Any;
        }

        public async Task<VpnStatus> GetStatus(string id)
        {
            var vpnId = VpnId.Parse(id);
            var stats = await VpnCli.GetStats();
            if (stats.Server == vpnId.Server)
            {
                if (Enum.TryParse<VpnStatus>(stats.State, out var status))
                {
                    return status;
                }
                return VpnStatus.Unknown;
            }
            return VpnStatus.Disconnected;            
        }
    }
}
