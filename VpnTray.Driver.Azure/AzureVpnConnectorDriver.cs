using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VpnTray.Domain;
using VpnTray.Driver.WinApi;

namespace VpnTray.Driver.Azure
{
    public class AzureVpnConnectorDriver : IVpnConnectorDriver
    {
        private readonly string _phonebookPath;

        public AzureVpnConnectorDriver(string phonebookPath)
        {
            _phonebookPath = phonebookPath;
        }

        public Task Connect(string id)
        {
            var entryName = RasApi32.RasEnumEntries(_phonebookPath)
                .FirstOrDefault(n => RasApi32.RasGetEntryProperties(n.EntryName, n.PhonebookPath).Id.ToString() == id);

            var dialParams = RasApi32.RasGetEntryDialParams(entryName.EntryName, entryName.PhonebookPath);

            RasApi32.RasDial(dialParams, entryName.PhonebookPath, (message, state, error) => { });

            return Task.CompletedTask;
        }

        public Task Disconnect(string id)
        {
            var connection = RasApi32.RasEnumConnections().FirstOrDefault(c => c.Entry.ToString() == id);

            RasApi32.RasHangUp(connection.Handle);

            return Task.CompletedTask;
        }
    }
}
