using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace VpnTray.Driver.WinApi
{
    [StructLayout(LayoutKind.Sequential, Pack = 4, CharSet = CharSet.Ansi)]
    public struct RasIpV6Addr
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
        public byte[] Byte;
    }
}
