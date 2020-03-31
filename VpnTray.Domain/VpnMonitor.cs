using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace VpnTray.Domain
{
    public class VpnMonitor
    {
        public Vpn Vpn { get; }
        private readonly IVpnMonitorDriver _vpnMonitorDriver;
        private readonly Timer _refreshTimer;

        public VpnMonitor(Vpn vpn, IVpnMonitorDriver vpnMonitorDriver)
        {
            Vpn = vpn ?? throw new ArgumentNullException(nameof(vpn));
            _vpnMonitorDriver = vpnMonitorDriver ?? throw new ArgumentNullException(nameof(vpnMonitorDriver));

            _refreshTimer = new Timer(async _ => await Refresh());
        }

        private bool _isEnabled;

        public bool IsEnabled
        {
            get => _isEnabled;
            set
            {
                if (_isEnabled == value) return;
                _isEnabled = value;
                SetTimer();
            }
        }

        private TimeSpan _refreshInterval = Timeout.InfiniteTimeSpan;

        public TimeSpan RefreshInterval
        {
            get => _refreshInterval;
            set
            {
                if (_refreshInterval == value) return;
                _refreshInterval = value;
                SetTimer();
            }
        }

        private VpnStatus _status;

        public VpnStatus Status
        {
            get => _status;
            set
            {
                if (_status == value) return;
                _status = value;
                OnStatusChanged();
            }
        }

        public event EventHandler StatusChanged;

        protected virtual void OnStatusChanged(EventArgs e) => StatusChanged?.Invoke(this, e);
    
        protected void OnStatusChanged() => OnStatusChanged(EventArgs.Empty);

        private IPAddress _ipAddress;

        public IPAddress IPAddress
        {
            get => _ipAddress;
            set
            {
                if (Equals(_ipAddress, value)) return;
                _ipAddress = value;
                OnIPAddressChanged();
            }
        }

        public event EventHandler IPAddressChanged;

        protected virtual void OnIPAddressChanged(EventArgs e) => IPAddressChanged?.Invoke(this, e);

        protected void OnIPAddressChanged() => OnIPAddressChanged(EventArgs.Empty);

        public async Task Refresh()
        {
            try
            {
                Status = await _vpnMonitorDriver.GetStatus(Vpn.Id);
                if (Status == VpnStatus.Connected)
                {
                    try
                    {
                        IPAddress = await _vpnMonitorDriver.GetIpAddress(Vpn.Id);
                    }
                    catch
                    {
                        IPAddress = IPAddress.None;
                    }
                }
            }
            catch (Exception)
            {
                Status = VpnStatus.DriverFailure;
            }
            finally
            {
                SetTimer();
            }
        }

        private void SetTimer()
        {
            _refreshTimer.Change(_isEnabled ? _refreshInterval : Timeout.InfiniteTimeSpan, Timeout.InfiniteTimeSpan);
        }
    }
}
