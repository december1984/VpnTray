using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace VpnTray.Driver.WinApi
{
    [StructLayout(LayoutKind.Sequential, Pack = 4, CharSet = CharSet.Ansi)]
    public struct RasConn
    {
        public int Size;
        public IntPtr Handle;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = RasConstants.RAS_MaxEntryName + 1)]
        public string EntryName;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = RasConstants.RAS_MaxDeviceType + 1)]
        public string DeviceType;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = RasConstants.RAS_MaxDeviceName + 1)]
        public string DeviceName;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = RasConstants.MAX_PATH)]
        public string Phonebook;
        public int SubEntry;
        public Guid Entry;

        //public int Flags;
        //public long Luid;
        //public Guid CorrelationId;
    };
}
