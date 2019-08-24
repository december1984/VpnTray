using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace VpnTray.Domain.Tests
{
    public class VpnMonitorUnitTests
    {
        [Test]
        public void Constructor_ShouldThrowArgumentNullException_WhenVpnIsNull()
        {
            // ARRANGE
            Vpn vpn = null;
            var driver = Substitute.For<IVpnMonitorDriver>();

            // ACT
            Action action = () => new VpnMonitor(vpn, driver);

            // ASSERT
            action.Should().Throw<ArgumentNullException>().Where(e => e.ParamName == "vpn");
        }

        [Test]
        public void Constructor_ShouldThrowArgumentNullException_WhenDriverIsNull()
        {
            // ARRANGE
            var vpn = new Vpn("ID1", "VPN1");
            IVpnMonitorDriver driver = null;

            // ACT
            Action action = () => new VpnMonitor(vpn, driver);

            // ASSERT
            action.Should().Throw<ArgumentNullException>().Where(e => e.ParamName == "vpnMonitorDriver");
        }

        [Test]
        public void Constructor_ShouldNotThrowException_WhenArgumentsAreNotNull()
        {
            // ARRANGE
            var vpn = new Vpn("ID1", "VPN1");
            var driver = Substitute.For<IVpnMonitorDriver>();

            // ACT
            Action action = () => new VpnMonitor(vpn, driver);

            // ASSERT
            action.Should().NotThrow();
        }

        [Test]
        public async Task Refresh_ShouldCallDriverGetStatus()
        {
            // ARRANGE
            var vpn = new Vpn("ID1", "VPN1");

            var driver = Substitute.For<IVpnMonitorDriver>();

            driver.GetStatus(default).ReturnsForAnyArgs(Task.FromResult(VpnStatus.Disconnected));

            var sut = new VpnMonitor(vpn, driver);

            // ACT
            await sut.Refresh();

            // ASSERT
            await driver.Received(1).GetStatus(Arg.Is(vpn.Id));
        }

        [Test]
        public async Task Refresh_ShouldCallDriverGetIpAddress_WhenStatusIsConnected()
        {
            // ARRANGE
            var vpn = new Vpn("ID1", "VPN1");

            var driver = Substitute.For<IVpnMonitorDriver>();

            driver.GetStatus(default).ReturnsForAnyArgs(Task.FromResult(VpnStatus.Connected));

            var sut = new VpnMonitor(vpn, driver);

            // ACT
            await sut.Refresh();

            // ASSERT
            await driver.Received(1).GetIpAddress(Arg.Is(vpn.Id));
        }

        [Test]
        [TestCase(VpnStatus.Unknown)]
        [TestCase(VpnStatus.Connected)]
        [TestCase(VpnStatus.Disconnecting)]
        [TestCase(VpnStatus.Disconnected)]
        [TestCase(VpnStatus.Connecting)]
        [TestCase(VpnStatus.DoesNotExist)]
        [TestCase(VpnStatus.DriverFailure)]
        [TestCase((VpnStatus)333)]
        public async Task Refresh_ShouldSetStatus(VpnStatus status)
        {
            // ARRANGE
            var vpn = new Vpn("ID1", "VPN1");

            var driver = Substitute.For<IVpnMonitorDriver>();

            driver.GetStatus(vpn.Id).Returns(Task.FromResult(status));

            var sut = new VpnMonitor(vpn, driver);

            // ACT
            await sut.Refresh();

            // ASSERT
            sut.Status.Should().Be(status);
        }

        [Test]
        [TestCase("10.0.0.1")]
        [TestCase("127.0.0.1")]
        [TestCase("192.168.127.55")]
        [TestCase("169.254.222.33")]
        public async Task Refresh_ShouldSetIpAddress_WhenStatusIsConnected(string ip)
        {
            // ARRANGE
            var vpn = new Vpn("ID1", "VPN1");
            var ipAddress = IPAddress.Parse(ip);
            var driver = Substitute.For<IVpnMonitorDriver>();

            driver.GetStatus(vpn.Id).Returns(Task.FromResult(VpnStatus.Connected));
            driver.GetIpAddress(vpn.Id).Returns(Task.FromResult(ipAddress));

            var sut = new VpnMonitor(vpn, driver);

            // ACT
            await sut.Refresh();

            // ASSERT
            sut.IPAddress.Should().BeEquivalentTo(ipAddress);
        }

        [Test]
        public async Task Refresh_ShouldSetStatusToDriverFailure_WhenDriverThrowsException()
        {
            // ARRANGE
            var vpn = new Vpn("ID1", "VPN1");

            var driver = Substitute.For<IVpnMonitorDriver>();

            driver.GetStatus(default).ThrowsForAnyArgs<Exception>();

            var sut = new VpnMonitor(vpn, driver);

            // ACT
            await sut.Refresh();

            // ASSERT
            sut.Status.Should().Be(VpnStatus.DriverFailure);
        }
    }
}
