using System.Runtime.InteropServices;

namespace VpnTray.Driver.WinApi
{
    [StructLayout(LayoutKind.Sequential, Pack = 4, CharSet = CharSet.Ansi)]
    public struct RasTunnelEndpoint
    {
        public int Type;
        public long IPv4;
        public long Dummy;
        //public RasIpV6Addr IPv6;
    }
}