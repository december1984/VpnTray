using System;
using System.Collections.Generic;
using System.Text;

namespace VpnTray.Domain
{
    public class Vpn
    {
        private string _name;

        public Vpn(string id, string name)
        {
            Id = id;
            Name = name;
        }
        public string Id { get; }

        public string Name
        {
            get => _name;
            set
            {
                if (_name == value) return;
                _name = value;
                OnNameChanged();
            }
        }

        public event EventHandler NameChanged;
        protected virtual void OnNameChanged(EventArgs e) => NameChanged?.Invoke(this, e);
        protected void OnNameChanged() => OnNameChanged(EventArgs.Empty);
    }
}
