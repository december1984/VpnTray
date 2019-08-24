using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace VpnTray.Driver.WinApi
{
    [StructLayout(LayoutKind.Sequential, Pack = 4, CharSet = CharSet.Ansi)]
    public struct RasDialParams
    {
        public int Size;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = RasConstants.RAS_MaxEntryName + 1)]
        public string EntryName;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = RasConstants.RAS_MaxPhoneNumber + 1)]
        public string PhoneNumber;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = RasConstants.RAS_MaxCallbackNumber + 1)]
        public string CallbackNumber;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = RasConstants.UNLEN + 1)]
        public string UserName;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = RasConstants.PWLEN + 1)]
        public string Password;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = RasConstants.DNLEN + 1)]
        public string Domain;

        public int SubEntry;
        public IntPtr CallbackId;

        public int IfIndex;

        [MarshalAs(UnmanagedType.LPStr)]
        public string EncPassword;
    }
}
