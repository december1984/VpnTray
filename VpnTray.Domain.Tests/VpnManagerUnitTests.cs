using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace VpnTray.Domain.Tests
{
    public class VpnManagerUnitTests
    {
        [Test]
        public async Task ShouldDisconnect_WhenDisconnectOnLockEnabledAndConnected()
        {
            // ARRANGE
            var id = "vpn1";
            var vpn = new Vpn(id, "VPN 1");
            var vpnConnectorDriver = Substitute.For<IVpnConnectorDriver>();
            var vpnMonitorDriver = Substitute.For<IVpnMonitorDriver>();
            vpnMonitorDriver.GetStatus(id).Returns(Task.FromResult(VpnStatus.Connected));
            var systemEventsProvider = Substitute.For<ISystemEventsProvider>();
            var sut = new VpnManager(vpn, vpnConnectorDriver, vpnMonitorDriver, systemEventsProvider)
            {
                DisconnectOnLock = true
            };
            await sut.Monitor.Refresh();

            // ACT
            systemEventsProvider.SessionLock += Raise.Event();

            // ASSERT
            await vpnConnectorDriver.Received(1).Disconnect(Arg.Is(id));
        }

        [Test]
        public async Task ShouldNotDisconnect_WhenDisconnectOnLockDisabledAndConnected()
        {
            // ARRANGE
            var id = "vpn1";
            var vpn = new Vpn(id, "VPN 1");
            var vpnConnectorDriver = Substitute.For<IVpnConnectorDriver>();
            var vpnMonitorDriver = Substitute.For<IVpnMonitorDriver>();
            vpnMonitorDriver.GetStatus(id).Returns(Task.FromResult(VpnStatus.Connected));
            var systemEventsProvider = Substitute.For<ISystemEventsProvider>();
            var sut = new VpnManager(vpn, vpnConnectorDriver, vpnMonitorDriver, systemEventsProvider)
            {
                DisconnectOnLock = false
            };
            await sut.Monitor.Refresh();

            // ACT
            systemEventsProvider.SessionLock += Raise.Event();

            // ASSERT
            await vpnConnectorDriver.DidNotReceive().Disconnect(Arg.Is(id));
        }

        [Test]
        public async Task ShouldNotDisconnect_WhenDisconnectOnLockDisabledAndNotConnected()
        {
            // ARRANGE
            var id = "vpn1";
            var vpn = new Vpn(id, "VPN 1");
            var vpnConnectorDriver = Substitute.For<IVpnConnectorDriver>();
            var vpnMonitorDriver = Substitute.For<IVpnMonitorDriver>();
            vpnMonitorDriver.GetStatus(id).Returns(Task.FromResult(VpnStatus.Disconnected));
            var systemEventsProvider = Substitute.For<ISystemEventsProvider>();
            var sut = new VpnManager(vpn, vpnConnectorDriver, vpnMonitorDriver, systemEventsProvider)
            {
                DisconnectOnLock = false
            };
            await sut.Monitor.Refresh();

            // ACT
            systemEventsProvider.SessionLock += Raise.Event();

            // ASSERT
            await vpnConnectorDriver.DidNotReceive().Disconnect(Arg.Is(id));
        }

        [Test]
        public async Task ShouldNotDisconnect_WhenDisconnectOnLockEnabledAndNotConnected()
        {
            // ARRANGE
            var id = "vpn1";
            var vpn = new Vpn(id, "VPN 1");
            var vpnConnectorDriver = Substitute.For<IVpnConnectorDriver>();
            var vpnMonitorDriver = Substitute.For<IVpnMonitorDriver>();
            vpnMonitorDriver.GetStatus(id).Returns(Task.FromResult(VpnStatus.Disconnected));
            var systemEventsProvider = Substitute.For<ISystemEventsProvider>();
            var sut = new VpnManager(vpn, vpnConnectorDriver, vpnMonitorDriver, systemEventsProvider)
            {
                DisconnectOnLock = true
            };
            await sut.Monitor.Refresh();

            // ACT
            systemEventsProvider.SessionLock += Raise.Event();

            // ASSERT
            await vpnConnectorDriver.DidNotReceive().Disconnect(Arg.Is(id));

        }

        [Test]
        public async Task ShouldConnect_WhenReconnectOnUnlockEnabledAndHasDisconnected()
        {
            // ARRANGE
            var id = "vpn1";
            var vpn = new Vpn(id, "VPN 1");
            var vpnConnectorDriver = Substitute.For<IVpnConnectorDriver>();
            var vpnMonitorDriver = Substitute.For<IVpnMonitorDriver>();
            vpnMonitorDriver.GetStatus(id).Returns(Task.FromResult(VpnStatus.Connected));
            var systemEventsProvider = Substitute.For<ISystemEventsProvider>();
            var sut = new VpnManager(vpn, vpnConnectorDriver, vpnMonitorDriver, systemEventsProvider)
            {
                DisconnectOnLock = true,
                ReconnectOnUnlock = true
            };
            await sut.Monitor.Refresh();

            // ACT
            systemEventsProvider.SessionLock += Raise.Event();
            systemEventsProvider.SessionUnlock += Raise.Event();

            // ASSERT
            await vpnConnectorDriver.Received(1).Connect(Arg.Is(id));
        }

        [Test]
        public async Task ShouldNotConnect_WhenReconnectOnUnlockDisabledAndHasDisconnected()
        {
            // ARRANGE
            var id = "vpn1";
            var vpn = new Vpn(id, "VPN 1");
            var vpnConnectorDriver = Substitute.For<IVpnConnectorDriver>();
            var vpnMonitorDriver = Substitute.For<IVpnMonitorDriver>();
            vpnMonitorDriver.GetStatus(id).Returns(Task.FromResult(VpnStatus.Connected));
            var systemEventsProvider = Substitute.For<ISystemEventsProvider>();
            var sut = new VpnManager(vpn, vpnConnectorDriver, vpnMonitorDriver, systemEventsProvider)
            {
                DisconnectOnLock = true,
                ReconnectOnUnlock = false
            };
            await sut.Monitor.Refresh();

            // ACT
            systemEventsProvider.SessionLock += Raise.Event();
            systemEventsProvider.SessionUnlock += Raise.Event();

            // ASSERT
            await vpnConnectorDriver.DidNotReceive().Connect(Arg.Is(id));
        }

        [Test]
        public async Task ShouldNotConnect_WhenReconnectOnUnlockEnabledAndHasNotDisconnected()
        {
            // ARRANGE
            var id = "vpn1";
            var vpn = new Vpn(id, "VPN 1");
            var vpnConnectorDriver = Substitute.For<IVpnConnectorDriver>();
            var vpnMonitorDriver = Substitute.For<IVpnMonitorDriver>();
            vpnMonitorDriver.GetStatus(id).Returns(Task.FromResult(VpnStatus.Connected));
            var systemEventsProvider = Substitute.For<ISystemEventsProvider>();
            var sut = new VpnManager(vpn, vpnConnectorDriver, vpnMonitorDriver, systemEventsProvider)
            {
                DisconnectOnLock = false,
                ReconnectOnUnlock = true
            };
            await sut.Monitor.Refresh();

            // ACT
            systemEventsProvider.SessionLock += Raise.Event();
            systemEventsProvider.SessionUnlock += Raise.Event();

            // ASSERT
            await vpnConnectorDriver.DidNotReceive().Connect(Arg.Is(id));
        }

        [Test]
        public async Task ShouldNotConnect_WhenReconnectOnUnlockDisabledAndHasNotDisconnected()
        {
            // ARRANGE
            var id = "vpn1";
            var vpn = new Vpn(id, "VPN 1");
            var vpnConnectorDriver = Substitute.For<IVpnConnectorDriver>();
            var vpnMonitorDriver = Substitute.For<IVpnMonitorDriver>();
            vpnMonitorDriver.GetStatus(id).Returns(Task.FromResult(VpnStatus.Connected));
            var systemEventsProvider = Substitute.For<ISystemEventsProvider>();
            var sut = new VpnManager(vpn, vpnConnectorDriver, vpnMonitorDriver, systemEventsProvider)
            {
                DisconnectOnLock = false,
                ReconnectOnUnlock = false
            };
            await sut.Monitor.Refresh();

            // ACT
            systemEventsProvider.SessionLock += Raise.Event();
            systemEventsProvider.SessionUnlock += Raise.Event();

            // ASSERT
            await vpnConnectorDriver.DidNotReceive().Connect(Arg.Is(id));
        }
    }
}
