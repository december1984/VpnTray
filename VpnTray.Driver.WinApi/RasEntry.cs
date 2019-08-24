using System;
using System.Runtime.InteropServices;

namespace VpnTray.Driver.WinApi
{
    [StructLayout(LayoutKind.Sequential, Pack = 4, CharSet = CharSet.Ansi)]
    public struct RasEntry
    {
        public int Size;
        public int Options;
        public int CountryID;
        public int CountryCode;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = RasConstants.RAS_MaxAreaCode + 1)]
        public string AreaCode;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = RasConstants.RAS_MaxPhoneNumber + 1)]
        public string LocalPhoneNumber;
        public int AlternateOffset;
        public RasIpAddr IP;
        public RasIpAddr Dns;
        public RasIpAddr DnsAlt;
        public RasIpAddr Wins;
        public RasIpAddr WinsAlt;
        public int FrameSize;
        public int NetProtocols;
        public int FramingProtocol;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = RasConstants.MAX_PATH)]
        public string Script;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = RasConstants.MAX_PATH)]
        public string AutodialDll;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = RasConstants.MAX_PATH)]
        public string AutodialFunc;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = RasConstants.RAS_MaxDeviceType + 1)]
        public string DeviceType;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = RasConstants.RAS_MaxDeviceName + 1)]
        public string DeviceName;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = RasConstants.RAS_MaxPadType + 1)]
        public string X25PadType;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = RasConstants.RAS_MaxX25Address + 1)]
        public string X25Address;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = RasConstants.RAS_MaxFacilities + 1)]
        public string X25Facilities;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = RasConstants.RAS_MaxUserData + 1)]
        public string X25UserData;
        public int Channels;
        public int Reserved1;
        public int Reserved2;
        public int SubEntries;
        public int DialMode;
        public int DialExtraPercent;
        public int DialExtraSampleSeconds;
        public int HangUpExtraPercent;
        public int HangUpExtraSampleSeconds;
        public int IdleDisconnectSeconds;
        public int Type;
        public int EncryptionType;
        public int CustomAuthKey;
        public Guid Id;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = RasConstants.MAX_PATH)]
        public string CustomDialDll;
        public int VpnStrategy;
        public int Options2;
        public int Options3;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = RasConstants.RAS_MaxDnsSuffix)]
        public string DnsSuffix;
        public int TcpWindowSize;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = RasConstants.MAX_PATH)]
        public string PrerequisitePbk;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = RasConstants.RAS_MaxEntryName + 1)]
        public string PrerequisiteEntry;
        public int RedialCount;
        public int RedialPause;
        public RasIpV6Addr V6Dns;
        public RasIpV6Addr V6DnsAlt;
        public int IPv4InterfaceMetric;
        public int IPv6InterfaceMetric;
        public RasIpV6Addr V6IP;
        public int IPv6PrefixLength;
        public int NetworkOutageTime;
    }
}