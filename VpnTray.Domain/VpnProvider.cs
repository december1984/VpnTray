using System;
using System.Collections.Generic;
using System.Text;

namespace VpnTray.Domain
{
    public abstract class VpnProvider
    {
        protected VpnProvider(IVpnEnumeratorDriver vpnEnumeratorDriver, 
            IVpnConnectorDriver vpnConnectorDriver,
            IVpnMonitorDriver vpnMonitorDriver,
            ISystemEventsProvider systemEventsProvider)
        {
            VpnEnumerator = new VpnEnumerator(vpnEnumeratorDriver);
            VpnManagerFactory = new VpnManagerFactory(vpnConnectorDriver, vpnMonitorDriver, systemEventsProvider);
        }

        public abstract string Name { get; protected set; }

        public VpnEnumerator VpnEnumerator { get; }
        public VpnManagerFactory VpnManagerFactory { get; }
    }
}
