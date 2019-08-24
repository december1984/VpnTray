using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace VpnTray.Driver.WinApi
{
    [StructLayout(LayoutKind.Sequential, Pack = 4, CharSet = CharSet.Ansi)]
    public struct RasIpAddr
    {
        public byte a;
        public byte b;
        public byte c;
        public byte d;
    }
}
