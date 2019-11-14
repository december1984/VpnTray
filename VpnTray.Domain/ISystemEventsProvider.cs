using System;
using System.Collections.Generic;
using System.Text;

namespace VpnTray.Domain
{
    public interface ISystemEventsProvider
    {
        event EventHandler SessionLock;
        event EventHandler SessionUnlock;
    }
}
