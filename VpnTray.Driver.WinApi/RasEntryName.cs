using System;
using System.Runtime.InteropServices;

namespace VpnTray.Driver.WinApi
{
    [StructLayout(LayoutKind.Sequential, Pack = 4, CharSet = CharSet.Ansi)]
    public struct RasEntryName
    {
        public int Size;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = RasConstants.RAS_MaxEntryName + 1)]
        public string EntryName;

        public RasEntryNameFlags Flags;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = RasConstants.MAX_PATH + 1)]
        public string PhonebookPath;
    };
}