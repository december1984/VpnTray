using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VpnTray.Domain;
using VpnTray.Properties;
using VpnTray.ViewModels;

namespace VpnTray.Views
{
    class VpnTrayNotifyIconWrapper
    {
        private static readonly Dictionary<VpnStatus, Icon> Icons = new Dictionary<VpnStatus, Icon>
        {
            {VpnStatus.Connected, Resources.Connected},
            {VpnStatus.Connecting, Resources.Connecting},
            {VpnStatus.Disconnected, Resources.Disconnected},
            {VpnStatus.Disconnecting, Resources.Disconnecting},
            {VpnStatus.DriverFailure, Resources.DriverError},
            {VpnStatus.Unknown, Resources.Default}
    };

        private static readonly Dictionary<VpnStatus, ToolTipIcon> BalloonIcons = new Dictionary<VpnStatus, ToolTipIcon>
        {
            {VpnStatus.Connected, ToolTipIcon.Info},
            {VpnStatus.Connecting, ToolTipIcon.Info},
            {VpnStatus.Disconnected, ToolTipIcon.Warning},
            {VpnStatus.Disconnecting, ToolTipIcon.Warning},
            {VpnStatus.DriverFailure, ToolTipIcon.Error},
            {VpnStatus.Unknown, ToolTipIcon.None}
    };

        public VpnTrayIconViewModel ViewModel { get; }
        public NotifyIcon Icon { get; }

        public VpnTrayNotifyIconWrapper(IContainer container, ContextMenuStrip menu, VpnTrayIconViewModel viewModel)
        {
            ViewModel = viewModel;

            Icon = new NotifyIcon(container)
            {
                Visible = true,
                ContextMenuStrip = menu
            };

            if (Icon.ContextMenuStrip != null)
            {
                Icon.MouseMove += (s, e) => Icon.ContextMenuStrip.Tag = ViewModel;
            }

            Icon.MouseDoubleClick += (s, e) => ViewModel.Connect().Wait();

            SetIcon();
            SetTooltip();

            ViewModel.PropertyChanged += ViewModelOnPropertyChanged;
        }

        private void ViewModelOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(VpnTrayIconViewModel.Name):
                    SetBalloon();
                    break;
                case nameof(VpnTrayIconViewModel.Status):
                    SetIcon();
                    SetBalloon();
                    break;
                case nameof(VpnTrayIconViewModel.TooltipText):
                    SetTooltip();
                    break;
            }
        }

        private void SetBalloon()
        {
            if (!BalloonIcons.TryGetValue(ViewModel.Status, out var icon))
            {
                icon = ToolTipIcon.Info;
            }
            Icon.ShowBalloonTip(5000, ViewModel.Name, ViewModel.Status.ToString(), icon);
        }

        private void SetIcon()
        {
            if (!Icons.TryGetValue(ViewModel.Status, out var icon))
            {
                icon = Resources.Default;
            }
            Icon.Icon = icon;
        }

        private void SetTooltip()
        {
            Icon.Text = ViewModel.TooltipText;
        }
    }
}
