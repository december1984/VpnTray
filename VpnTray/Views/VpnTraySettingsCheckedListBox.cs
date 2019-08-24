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
    class VpnTraySettingsCheckedListBox : CheckedListBox
    {
        private readonly ObservableCollection<VpnTraySettingsEntryViewModel> _viewModel;

        public VpnTraySettingsCheckedListBox(ObservableCollection<VpnTraySettingsEntryViewModel> viewModel)
        {
            _viewModel = viewModel;
            for (int i = 0; i < _viewModel.Count; i++)
            {
                _viewModel[i].PropertyChanged += Setting_PropertyChanged;
                AddSetting(i, _viewModel[i]);
            }
            _viewModel.CollectionChanged += Settings_CollectionChanged;
        }

        private void Settings_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    for (int i = 0; i < e.NewItems.Count; i++)
                    {
                        var setting = e.NewItems[i] as VpnTraySettingsEntryViewModel;
                        setting.PropertyChanged += Setting_PropertyChanged;
                        AddSetting(e.NewStartingIndex + i, setting);
                    }
                    break;
                case NotifyCollectionChangedAction.Remove:
                    for (int i = 0; i < e.OldItems.Count; i++)
                    {
                        var setting = e.OldItems[i] as VpnTraySettingsEntryViewModel;
                        setting.PropertyChanged -= Setting_PropertyChanged;
                        RemoveSetting(e.OldStartingIndex);
                    }
                    break;
            }
        }

        private void Setting_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var setting = sender as VpnTraySettingsEntryViewModel;
            if (e.PropertyName == nameof(VpnTraySettingsEntryViewModel.Name))
            {
                int index = _viewModel.IndexOf(setting);
                ChangeCaption(index, setting.Name);
            }
        }

        private void AddSetting(int index, VpnTraySettingsEntryViewModel setting)
        {
            if (InvokeRequired)
            {
                Action<int, VpnTraySettingsEntryViewModel> action = AddSetting;
                Invoke(action, index, setting);
            }
            else
            {
                Items.Insert(index, setting.Name);
                if (setting.IsSelected)
                {
                    SetItemChecked(index, true);
                }
            }
        }

        private void ChangeCaption(int index, string caption)
        {
            if (InvokeRequired)
            {
                Action<int, string> action = ChangeCaption;
                Invoke(action, index, caption);
            }
            else
            {
                Items[index] = caption;
            }
        }

        private void RemoveSetting(int index)
        {
            if (InvokeRequired)
            {
                Action<int> action = RemoveSetting;
                Invoke(action, index);
            }
            else
            {
                Items.RemoveAt(index);
            }
        }

        protected override void OnItemCheck(ItemCheckEventArgs ice)
        {
            base.OnItemCheck(ice);
            _viewModel[ice.Index].IsSelected = ice.NewValue == CheckState.Checked;
        }
    }
}
