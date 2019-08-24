using System;
using System.Collections.Generic;
using System.Text;

namespace VpnTray.Driver.WinApi
{
    public delegate void RasDialFunc(uint message, RasConnState rasConnState, int error);
}
