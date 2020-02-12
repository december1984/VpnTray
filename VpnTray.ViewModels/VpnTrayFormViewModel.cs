using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VpnTray.Domain;
using VpnTray.ViewModels.Configuration;

namespace VpnTray.ViewModels
{
    public class VpnTrayFormViewModel : BaseViewModel
    {
        public VpnTrayTabViewModel[] Tabs { get; private set; }
        public VpnTrayFormViewModel(VpnProvider[] vpnProviders, IVpnTrayConfigurationProvider configurationProvider)
        {
            Tabs = vpnProviders.Select(p => new VpnTrayTabViewModel(p, configurationProvider)).ToArray();
        }
    }
}
