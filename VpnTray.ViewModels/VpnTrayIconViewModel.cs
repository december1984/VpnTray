using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using VpnTray.Domain;

namespace VpnTray.ViewModels
{
    public class VpnTrayIconViewModel : BaseViewModel
    {
        internal VpnManager VpnManager { get; }

        public VpnTrayIconViewModel(VpnManager vpnManager)
        {
            VpnManager = vpnManager;
            VpnManager.Vpn.NameChanged += (s, e) =>
            {
                OnPropertyChanged(nameof(Name));
                OnPropertyChanged(nameof(TooltipText));
            };
            VpnManager.Monitor.StatusChanged += (s, e) =>
            {
                OnPropertyChanged(nameof(Status));
                OnPropertyChanged(nameof(TooltipText));
                OnPropertyChanged(nameof(CanConnect));
                OnPropertyChanged(nameof(CanDisconnect));
            };
            VpnManager.Monitor.IPAddressChanged += (s, e) =>
            {
                OnPropertyChanged(nameof(TooltipText));
            };
        }

        public string Name => VpnManager.Vpn.Name;
        public VpnStatus Status => VpnManager.Monitor.Status;

        public string TooltipText
        {
            get
            {
                var builder = new StringBuilder(Name);
                builder.Append(" - ");
                builder.Append(Status);
                if (Status == VpnStatus.Connected)
                {
                    builder.Append(" (");
                    builder.Append(VpnManager.Monitor.IPAddress);
                    builder.Append(")");
                }

                return builder.ToString();
            }
        }

        public bool CanConnect => VpnManager.Monitor.Status == VpnStatus.Disconnected;

        public async Task Connect()
        {
            if (CanConnect)
            {
                await VpnManager.Connector.Connect();
            }
        }

        public bool CanDisconnect => VpnManager.Monitor.Status == VpnStatus.Connected;

        public async Task Disconnect()
        {
            if (CanDisconnect)
            {
                await VpnManager.Connector.Disconnect();
            }
        }
    }
}
