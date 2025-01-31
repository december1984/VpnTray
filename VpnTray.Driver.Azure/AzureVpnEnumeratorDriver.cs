using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VpnTray.Domain;
using VpnTray.Driver.WinApi;

namespace VpnTray.Driver.Azure
{
    public class AzureVpnEnumeratorDriver : IVpnEnumeratorDriver
    {
        private string _phonebookPath;

        public AzureVpnEnumeratorDriver(string phonebookPath)
        {
            _phonebookPath = phonebookPath;
        }

        public Task<Vpn[]> GetVpns()
        {
            return Task.FromResult((
                    from name in RasApi32.RasEnumEntries(_phonebookPath)
                    let entry = RasApi32.RasGetEntryProperties(name.EntryName, name.PhonebookPath)
                    select new Vpn(entry.Id.ToString(), name.EntryName))
                .ToArray());
        }
    }
}
