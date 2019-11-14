using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VpnTray.Domain;

namespace VpnTray.SystemDriver
{
    public class SystemEventsProvider : ISystemEventsProvider
    {
        public event EventHandler SessionLock;
        public event EventHandler SessionUnlock;

        public SystemEventsProvider()
        {
            Microsoft.Win32.SystemEvents.SessionSwitch += SystemEvents_SessionSwitch;
        }

        ~SystemEventsProvider()
        {
            Microsoft.Win32.SystemEvents.SessionSwitch -= SystemEvents_SessionSwitch;
        }

        private void SystemEvents_SessionSwitch(object sender, Microsoft.Win32.SessionSwitchEventArgs e)
        {
            switch (e.Reason)
            {
                case Microsoft.Win32.SessionSwitchReason.SessionLock:
                    SessionLock?.Invoke(this, EventArgs.Empty);
                    break;
                case Microsoft.Win32.SessionSwitchReason.SessionUnlock:
                    SessionUnlock?.Invoke(this, EventArgs.Empty);
                    break;
            }
        }
    }
}
