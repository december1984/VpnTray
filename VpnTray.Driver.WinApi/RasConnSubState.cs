namespace VpnTray.Driver.WinApi
{
    public enum RasConnSubState
    {
        None,
        Dormant,
        Reconnecting,
        Reconnected = 0x2000
    }
}