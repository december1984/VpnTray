using System;
using System.Collections.Generic;
using System.Text;

namespace VpnTray.Domain
{
    public class VpnEventArgs : EventArgs
    {
        public Vpn Vpn { get; }

        public VpnEventArgs(Vpn vpn)
        {
            Vpn = vpn;
        }
    }
}
