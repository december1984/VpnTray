using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VpnTray.ViewModels;

namespace VpnTray.Views
{
    class VpnTraySettingsView : DataGridView
    {
        private readonly BindingList<VpnTraySettingsEntryViewModel> _items;

        public VpnTraySettingsView(ObservableCollection<VpnTraySettingsEntryViewModel> viewModel)
        {
            AllowUserToAddRows = false;
            AllowUserToDeleteRows = false;
            AutoGenerateColumns = false;
            RowHeadersVisible = false;
            AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

            _items = new BindingList<VpnTraySettingsEntryViewModel>();
            DataSource = _items;

            Columns.Add(new DataGridViewCheckBoxColumn { HeaderText = "Show in tray", DataPropertyName = "IsSelected" });
            Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Name", DataPropertyName = "Name", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill });
            Columns.Add(new DataGridViewCheckBoxColumn { HeaderText = "Disconnect on lock", DataPropertyName = "DisconnectOnLock" });
            Columns.Add(new DataGridViewCheckBoxColumn { HeaderText = "Reconnect on unlock", DataPropertyName = "ReconnectOnUnlock" });

            foreach (var item in viewModel)
            {
                _items.Add(item);
            }
            viewModel.CollectionChanged += Settings_CollectionChanged;
        }

        private void Settings_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (InvokeRequired)
            {
                NotifyCollectionChangedEventHandler handler = Settings_CollectionChanged;
                Invoke(handler, sender, e);
            }
            else
            {
                switch (e.Action)
                {
                    case NotifyCollectionChangedAction.Add:
                        for (int i = 0; i < e.NewItems.Count; i++)
                        {
                            var setting = e.NewItems[i] as VpnTraySettingsEntryViewModel;
                            _items.Insert(e.NewStartingIndex + i, setting);
                        }
                        break;
                    case NotifyCollectionChangedAction.Remove:
                        for (int i = 0; i < e.OldItems.Count; i++)
                        {
                            _items.RemoveAt(e.OldStartingIndex);
                        }
                        break;
                }
            }
        }

        protected override void OnCurrentCellDirtyStateChanged(EventArgs e)
        {
            base.OnCurrentCellDirtyStateChanged(e);
            if (CurrentCell is DataGridViewCheckBoxCell)
            {
                EndEdit();
            }
        }
    }
}
