using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VpnTray.Properties;
using VpnTray.ViewModels;

namespace VpnTray.Views
{
    class VpnTrayNotifyIconManager
    {
        private readonly ContextMenuStrip _menu;
        private readonly IContainer _container;
        public List<VpnTrayNotifyIconWrapper> NotifyIcons { get; } = new List<VpnTrayNotifyIconWrapper>();

        public VpnTrayNotifyIconManager(IContainer container, ContextMenuStrip menu,
            ObservableCollection<VpnTrayIconViewModel> viewModel)
        {
            _menu = menu;
            _container = container;

            foreach (var icon in viewModel)
            {
                var wrapper = new VpnTrayNotifyIconWrapper(_container, _menu, icon);
                NotifyIcons.Add(wrapper);
            }

            viewModel.CollectionChanged += ViewModel_CollectionChanged;
        }

        private void ViewModel_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    for (int i = 0; i < e.NewItems.Count; i++)
                    {
                        var wrapper = new VpnTrayNotifyIconWrapper(_container, _menu, e.NewItems[i] as VpnTrayIconViewModel);
                        NotifyIcons.Insert(e.NewStartingIndex + i, wrapper);
                    }
                    break;
                case NotifyCollectionChangedAction.Remove:
                    for (int i = 0; i < e.OldItems.Count; i++)
                    {
                        var notifyIcon = NotifyIcons[e.OldStartingIndex];
                        _container.Remove(notifyIcon.Icon);
                        NotifyIcons.Remove(notifyIcon);
                        notifyIcon.Dispose();
                    }
                    break;
            }
        }
    }
}
