using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace VpnTray.Driver.WinApi
{
    [StructLayout(LayoutKind.Sequential, Pack = 4, CharSet = CharSet.Ansi)]
    public class RasPppIp
    {
        public int Size;
        public int Error;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = RasConstants.RAS_MaxIpAddress + 1)]
        public string IpAddress;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = RasConstants.RAS_MaxIpAddress + 1)]
        public string ServerIpAddress;
        public int Options;
        public int ServerOptions;
    }
}
