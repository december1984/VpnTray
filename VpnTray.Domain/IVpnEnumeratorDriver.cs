using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace VpnTray.Domain
{
    public interface IVpnEnumeratorDriver
    {
        Task<Vpn[]> GetVpns();
    }
}
