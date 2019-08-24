namespace VpnTray.ViewModels.Configuration
{
    public interface IVpnTrayConfigurationProvider
    {
        VpnTrayFormConfiguration Configuration { get; }
        void Save();
    }
}
