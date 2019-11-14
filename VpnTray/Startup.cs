using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleInjector;
using VpnTray.Domain;
using VpnTray.Driver;
using VpnTray.SystemDriver;
using VpnTray.ViewModels;
using VpnTray.ViewModels.Configuration;

namespace VpnTray
{
    class Startup
    {
        public Container Services { get; } = new Container();

        public void ConfigureServices()
        {
            Services.Register<IVpnTrayConfigurationProvider, SettingsConfigurationProvider>();

            Services.Register<IVpnEnumeratorDriver, VpnEnumeratorDriver>();
            Services.Register<IVpnConnectorDriver, VpnConnectorDriver>();
            Services.Register<IVpnMonitorDriver, VpnMonitorDriver>();

            Services.RegisterSingleton<ISystemEventsProvider, SystemEventsProvider>();

            Services.Register<VpnEnumerator>();
            Services.Register<VpnManagerFactory>();

            Services.Register<VpnTrayFormViewModel>();

            Services.Register<VpnTrayForm>();
        }
    }
}
