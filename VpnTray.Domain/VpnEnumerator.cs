using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Exception = System.Exception;

namespace VpnTray.Domain
{
    public class VpnEnumerator
    {
        private readonly IVpnEnumeratorDriver _vpnEnumeratorDriver;
        private readonly Timer _refreshTimer;

        public VpnEnumerator(IVpnEnumeratorDriver vpnEnumeratorDriver)
        {
            _vpnEnumeratorDriver = vpnEnumeratorDriver ?? throw new ArgumentNullException(nameof(vpnEnumeratorDriver));

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

        public List<Vpn> Vpns { get; } = new List<Vpn>();

        public async Task Refresh()
        {
            try
            {
                var vpns = await _vpnEnumeratorDriver.GetVpns();

                if (vpns == null)
                {
                    throw new Exception("Driver returned null");
                }

                foreach (var vpn in vpns)
                {
                    var existing = Vpns.SingleOrDefault(v => v.Id == vpn.Id);
                    if (existing == null)
                    {
                        Vpns.Add(vpn);
                        OnAdded(vpn);
                    }
                    else
                    {
                        existing.Name = vpn.Name;
                    }
                }

                var removed = Vpns.Where(existing => vpns.All(v => v.Id != existing.Id)).ToList();

                foreach (var v in removed)
                {
                    Vpns.Remove(v);
                    OnRemoved(v);
                }
            }
            catch (Exception ex)
            {
                OnError(ex);
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

        public event EventHandler<VpnEventArgs> Added;
        protected virtual void OnAdded(VpnEventArgs e) => Added?.Invoke(this, e);
        protected void OnAdded(Vpn vpn) => OnAdded(new VpnEventArgs(vpn));

        public event EventHandler<VpnEventArgs> Removed;
        protected virtual void OnRemoved(VpnEventArgs e) => Removed?.Invoke(this, e);
        protected void OnRemoved(Vpn vpn) => OnRemoved(new VpnEventArgs(vpn));

        public event ErrorEventHandler Error;
        protected virtual void OnError(ErrorEventArgs e) => Error?.Invoke(this, e);
        protected void OnError(Exception ex) => OnError(new ErrorEventArgs(ex));
    }
}
