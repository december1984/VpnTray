using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace VpnTray.Domain
{
    public interface IVpnMonitorDriver
    {
        Task<VpnStatus> GetStatus(string id);
        Task<IPAddress> GetIpAddress(string id);
    }
}
