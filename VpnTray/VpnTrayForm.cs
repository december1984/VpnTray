using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VpnTray.ViewModels;
using VpnTray.Views;
using Action = System.Action;

namespace VpnTray
{
    public partial class VpnTrayForm : Form
    {
        private readonly VpnTrayFormViewModel _viewModel;

        private readonly VpnTraySettingsCheckedListBox _settingsCheckedListBox;

        private readonly VpnTrayNotifyIconManager _vpnTrayNotifyIcon;

        public VpnTrayForm(VpnTrayFormViewModel viewModel)
        {
            _viewModel = viewModel;

            InitializeComponent();

            if (components == null)
            {
                components = new Container();
            }

            _settingsCheckedListBox = new VpnTraySettingsCheckedListBox(_viewModel.Settings)
            {
                Dock = DockStyle.Fill,
                CheckOnClick = true
            };
            Controls.Add(_settingsCheckedListBox);
            
            _vpnTrayNotifyIcon = new VpnTrayNotifyIconManager(components, contextMenuStrip1, _viewModel.Icons);
        }

        private void ContextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            if (contextMenuStrip1.Tag is VpnTrayIconViewModel viewModel)
            {
                connectToolStripMenuItem.Enabled = viewModel.CanConnect;
                disconnectToolStripMenuItem.Enabled = viewModel.CanDisconnect;
            }
            else
            {
                connectToolStripMenuItem.Enabled = false;
                disconnectToolStripMenuItem.Enabled = false;
            }

            settingsToolStripMenuItem.Enabled = !Visible;
        }

        private void ConnectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (contextMenuStrip1.Tag is VpnTrayIconViewModel viewModel)
            {
                viewModel.Connect().Wait();
                contextMenuStrip1.Tag = null;
            }
        }

        private void DisconnectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (contextMenuStrip1.Tag is VpnTrayIconViewModel viewModel)
            {
                viewModel.Disconnect().Wait();
                contextMenuStrip1.Tag = null;
            }
        }

        private void SettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Show();
        }

        private void QuitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            if (_vpnTrayNotifyIcon.NotifyIcons.Count > 0 && e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                Hide();
            }
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            if (_vpnTrayNotifyIcon.NotifyIcons.Count > 0)
            {
                Hide();
            }
        }
    }
}
