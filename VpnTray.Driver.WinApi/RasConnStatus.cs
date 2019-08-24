using System.Runtime.InteropServices;

namespace VpnTray.Driver.WinApi
{
    [StructLayout(LayoutKind.Sequential, Pack = 4, CharSet = CharSet.Ansi)]
    public struct RasConnStatus
    {
        public int Size;
        public RasConnState State;
        public int Error;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = RasConstants.RAS_MaxDeviceType + 1)]
        public string DeviceType;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = RasConstants.RAS_MaxDeviceName + 1)]
        public string DeviceName;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = RasConstants.RAS_MaxPhoneNumber + 1)]
        public string PhoneNumber;

        public RasTunnelEndpoint LocalEndPoint;
        public RasTunnelEndpoint RemoteEndPoint;
        public RasConnSubState Substate;
    };
}