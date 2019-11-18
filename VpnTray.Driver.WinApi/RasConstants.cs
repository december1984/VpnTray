using System;
using System.Collections.Generic;
using System.Text;

namespace VpnTray.Driver.WinApi
{
    public static class RasConstants
    {
        public const int RAS_MaxEntryName = 256;
        public const int RAS_MaxDeviceType = 16;
        public const int RAS_MaxDeviceName = 128;
        public const int RAS_MaxPhoneNumber = 128;
        public const int RAS_MaxCallbackNumber = 128;
        public const int RAS_MaxIpAddress = 15;

        public const int RAS_MaxAreaCode = 10;
        public const int RAS_MaxPadType = 32;
        public const int RAS_MaxX25Address = 200;
        public const int RAS_MaxFacilities = 200;
        public const int RAS_MaxUserData = 200;
        public const int RAS_MaxDnsSuffix = 256;

        public const int UNLEN = 256;
        public const int PWLEN = 256;
        public const int DNLEN = 15;
        public const int MAX_PATH = 260;
    }
}
