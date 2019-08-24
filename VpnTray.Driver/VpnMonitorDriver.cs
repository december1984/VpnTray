using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using VpnTray.Domain;
using VpnTray.Driver.WinApi;

namespace VpnTray.Driver
{
    public class VpnMonitorDriver : IVpnMonitorDriver
    {
        public Task<VpnStatus> GetStatus(string id)
        {
            var connection = RasApi32.RasEnumConnections().FirstOrDefault(c => c.Entry.ToString() == id);
            if (connection.Handle == IntPtr.Zero)
            {
                return Task.FromResult(VpnStatus.Disconnected);
            }

            var status = RasApi32.RasGetConnectStatus(connection.Handle);
            switch (status.State)
            {
                case RasConnState.Connected:
                    return Task.FromResult(VpnStatus.Connected);
                case RasConnState.Disconnected:
                    return Task.FromResult(VpnStatus.Disconnecting);
                default:
                    return Task.FromResult(VpnStatus.Connecting);
            }
        }

        public Task<IPAddress> GetIpAddress(string id)
        {
            var connection = RasApi32.RasEnumConnections().FirstOrDefault(c => c.Entry.ToString() == id);
            if (connection.Handle == IntPtr.Zero)
            {
                return Task.FromResult(IPAddress.None);
            }

            var status = RasApi32.RasGetConnectStatus(connection.Handle);
            return Task.FromResult(new IPAddress(status.RemoteEndPoint.IPv4));
        }
    }
}
