namespace VpnTray.Driver.WinApi
{
    public enum RasProjection
    {
        Amb = 0x10000,
        PppNbf = 0x803F,
        PppIpx = 0x802B,
        PppIp = 0x8021,
        PppCcp = 0x80FD,
        PppLcp = 0xC021,
        PppIpv6 = 0x8057,
        Slip = 0x20000
    }
}