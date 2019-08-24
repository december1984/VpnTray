using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VpnTray.Domain;
using VpnTray.Driver.WinApi;

namespace VpnTray.Driver
{
    public class VpnEnumeratorDriver : IVpnEnumeratorDriver
    {
        public Task<Vpn[]> GetVpns()
        {
            return Task.FromResult((
                    from name in RasApi32.RasEnumEntries()
                    let entry = RasApi32.RasGetEntryProperties(name.EntryName, name.PhonebookPath)
                    where entry.DeviceType.ToLower() == "vpn"
                    select new Vpn(entry.Id.ToString(), name.EntryName))
                .ToArray());
        }
    }
}
