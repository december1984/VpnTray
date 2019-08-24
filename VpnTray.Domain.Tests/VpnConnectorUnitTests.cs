using System;
using System.Threading.Tasks;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;

namespace VpnTray.Domain.Tests
{
    public class VpnConnectorUnitTests
    { 
        [Test]
        public void Constructor_ShouldThrowArgumentNullException_WhenVpnIsNull()
        {
            // ARRANGE
            Vpn vpn = null;
            IVpnConnectorDriver driver = Substitute.For<IVpnConnectorDriver>();

            // ACT
            Action action = () => new VpnConnector(vpn, driver);

            // ASSERT
            action.Should().Throw<ArgumentNullException>().Where(e => e.ParamName == "vpn");
        }

        [Test]
        public void Constructor_ShouldThrowArgumentNullException_WhenVpnConnectorDriverIsNull()
        {
            // ARRANGE
            Vpn vpn = new Vpn("MockId", "MockName");
            IVpnConnectorDriver driver = null;

            // ACT 
            Action action = () => new VpnConnector(vpn, driver);

            // ASSERT
            action.Should().Throw<ArgumentNullException>().Where(e => e.ParamName == "vpnConnectorDriver");
        }

        [Test]
        public void Constructor_ShouldNotThrowException_WhenArgumentsAreNotNull()
        {
            // ARRANGE
            var vpn = new Vpn("MockId", "MockName");
            var driver = Substitute.For<IVpnConnectorDriver>();

            // ACT
            Action action = () => new VpnConnector(vpn, driver);

            // ASSERT
            action.Should().NotThrow();
        }

        [Test]
        public async Task Connect_ShouldCallDriverConnectWithVpnId()
        {
            // ARRANGE
            var vpn = new Vpn("MockId", "MockName");
            var driver = Substitute.For<IVpnConnectorDriver>();
            var sut = new VpnConnector(vpn, driver);

            // ACT
            await sut.Connect();

            // ASSERT
            await driver.Received().Connect(Arg.Is(vpn.Id));
        }

        [Test]
        public async Task Disconnect_ShouldCallDriverDisconnectWithVpnId()
        {
            // ARRANGE
            var vpn = new Vpn("MockId", "MockName");
            var driver = Substitute.For<IVpnConnectorDriver>();
            var sut = new VpnConnector(vpn, driver);

            // ACT
            await sut.Disconnect();

            // ASSERT
            await driver.Received().Disconnect(Arg.Is(vpn.Id));

        }
    }
}