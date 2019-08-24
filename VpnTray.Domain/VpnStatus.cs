using System;

namespace VpnTray.Domain
{
    public enum VpnStatus
    {
        Unknown,
        Disconnected,
        Connecting,
        Connected,
        Disconnecting,
        DriverFailure
    }
}
