using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using FluentAssertions.Common;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using NUnit.Framework;

namespace VpnTray.Domain.Tests
{
    public class VpnEnumeratorUnitTests
    {
        [Test]
        public void Constructor_ShouldThrowArgumentNullException_WhenDriverIsNull()
        {
            // ARRANGE
            IVpnEnumeratorDriver driver = null;

            // ACT
            Action action = () => new VpnEnumerator(driver);

            // ASSERT
            action.Should().Throw<ArgumentNullException>().Where(e => e.ParamName == "vpnEnumeratorDriver");
        }

        [Test]
        public void Constructor_ShouldNotThrowException_WhenDriverIsNotNull()
        {
            // ARRANGE
            var driver = Substitute.For<IVpnEnumeratorDriver>();

            // ACT
            Action action = () => new VpnEnumerator(driver);

            // ASSERT
            action.Should().NotThrow();
        }

        [Test]
        public async Task Refresh_ShouldCallDriverGetVpns()
        {
            // ARRANGE
            var driver = Substitute.For<IVpnEnumeratorDriver>();

            var sut = new VpnEnumerator(driver);

            // ACT
            await sut.Refresh();

            // ASSERT
            await driver.Received(1).GetVpns();
        }

        [Test]
        public async Task Refresh_ShouldRaiseAddedEvents_WhenNewVpnsAppear()
        {
            // ARRANGE
            var driver = Substitute.For<IVpnEnumeratorDriver>();

            driver.GetVpns().Returns(Task.FromResult(new[]
            {
                new Vpn("ID1", "VPN1"),
                new Vpn("ID2", "VPN2"),
                new Vpn("ID3", "VPN3"),
                new Vpn("ID4", "VPN4")
            }));

            var sut = new VpnEnumerator(driver)
            {
                Vpns =
                {
                    new Vpn("ID1", "VPN1"),
                    new Vpn("ID2", "VPN2")
                }
            };

            using (var monitor = sut.Monitor())
            {
                // ACT
                await sut.Refresh();

                // ASSERT
                monitor.Should().Raise(nameof(VpnEnumerator.Added))
                    .WithSender(sut)
                    .WithArgs<VpnEventArgs>(e => e.Vpn.Id == "ID3" && e.Vpn.Name == "VPN3");
                monitor.Should().Raise(nameof(VpnEnumerator.Added))
                    .WithSender(sut)
                    .WithArgs<VpnEventArgs>(e => e.Vpn.Id == "ID4" && e.Vpn.Name == "VPN4");

                monitor.Should().NotRaise(nameof(VpnEnumerator.Removed));
                monitor.Should().NotRaise(nameof(VpnEnumerator.Error));
            }
        }

        [Test]
        public async Task Refresh_ShouldRaiseRemovedEvents_WhenVpnsDisappear()
        {
            // ARRANGE
            var driver = Substitute.For<IVpnEnumeratorDriver>();

            driver.GetVpns().Returns(Task.FromResult(new[]
            {
                new Vpn("ID1", "VPN1"),
                new Vpn("ID2", "VPN2"),
            }));

            var sut = new VpnEnumerator(driver)
            {
                Vpns =
                {
                    new Vpn("ID1", "VPN1"),
                    new Vpn("ID2", "VPN2"),
                    new Vpn("ID3", "VPN3"),
                    new Vpn("ID4", "VPN4")
                }
            };

            using (var monitor = sut.Monitor())
            {
                // ACT
                await sut.Refresh();

                // ASSERT
                monitor.Should().Raise(nameof(VpnEnumerator.Removed))
                    .WithSender(sut)
                    .WithArgs<VpnEventArgs>(e => e.Vpn.Id == "ID3" && e.Vpn.Name == "VPN3");
                monitor.Should().Raise(nameof(VpnEnumerator.Removed))
                    .WithSender(sut)
                    .WithArgs<VpnEventArgs>(e => e.Vpn.Id == "ID4" && e.Vpn.Name == "VPN4");

                monitor.Should().NotRaise(nameof(VpnEnumerator.Added));
                monitor.Should().NotRaise(nameof(VpnEnumerator.Error));
            }
        }

        [Test]
        public async Task Refresh_ShouldRaiseAddedAndRemovedEvents_WhenSomeVpnsAppearAndSomeDisappear()
        {
            // ARRANGE
            var driver = Substitute.For<IVpnEnumeratorDriver>();

            driver.GetVpns().Returns(Task.FromResult(new[]
            {
                new Vpn("ID3", "VPN3"),
                new Vpn("ID4", "VPN4"),
                new Vpn("ID5", "VPN5"),
                new Vpn("ID6", "VPN6")
            }));

            var sut = new VpnEnumerator(driver)
            {
                Vpns =
                {
                    new Vpn("ID1", "VPN1"),
                    new Vpn("ID2", "VPN2"),
                    new Vpn("ID3", "VPN3"),
                    new Vpn("ID4", "VPN4")
                }
            };

            using (var monitor = sut.Monitor())
            {
                // ACT
                await sut.Refresh();

                // ASSERT
                monitor.Should().Raise(nameof(VpnEnumerator.Added))
                    .WithSender(sut)
                    .WithArgs<VpnEventArgs>(e => e.Vpn.Id == "ID5" && e.Vpn.Name == "VPN5");
                monitor.Should().Raise(nameof(VpnEnumerator.Added))
                    .WithSender(sut)
                    .WithArgs<VpnEventArgs>(e => e.Vpn.Id == "ID6" && e.Vpn.Name == "VPN6");

                monitor.Should().Raise(nameof(VpnEnumerator.Removed))
                    .WithSender(sut)
                    .WithArgs<VpnEventArgs>(e => e.Vpn.Id == "ID1" && e.Vpn.Name == "VPN1");
                monitor.Should().Raise(nameof(VpnEnumerator.Removed))
                    .WithSender(sut)
                    .WithArgs<VpnEventArgs>(e => e.Vpn.Id == "ID2" && e.Vpn.Name == "VPN2");

                monitor.Should().NotRaise(nameof(VpnEnumerator.Error));
            }
        }

        [Test]
        public async Task Refresh_ShouldRaiseErrorEvent_WhenDriverReturnsNull()
        {
            // ARRANGE
            var driver = Substitute.For<IVpnEnumeratorDriver>();

            driver.GetVpns().Returns(Task.FromResult<Vpn[]>(null));

            var sut = new VpnEnumerator(driver);

            using (var monitor = sut.Monitor())
            {
                // ACT
                await sut.Refresh();

                // ASSERT
                monitor.Should().Raise(nameof(VpnEnumerator.Error))
                    .WithSender(sut);

                monitor.Should().NotRaise(nameof(VpnEnumerator.Added));
                monitor.Should().NotRaise(nameof(VpnEnumerator.Removed));
            }
        }

        [Test]
        public async Task Refresh_ShouldRaiseErrorEvent_WhenDriverThrowsException()
        {
            // ARRANGE
            var driver = Substitute.For<IVpnEnumeratorDriver>();

            var exception = new Exception("IVpnEnumeratorDriver Exception");
            driver.GetVpns().Throws(exception);

            var sut = new VpnEnumerator(driver);

            using (var monitor = sut.Monitor())
            {
                // ACT
                await sut.Refresh();

                // ASSERT
                monitor.Should().Raise(nameof(VpnEnumerator.Error))
                    .WithSender(sut)
                    .WithArgs<ErrorEventArgs>(e => e.GetException().IsSameOrEqualTo(exception));

                monitor.Should().NotRaise(nameof(VpnEnumerator.Added));
                monitor.Should().NotRaise(nameof(VpnEnumerator.Removed));
            }
        }

        [Test]
        public async Task Refresh_ShouldNotRaiseEvents_WhenNothingChanged()
        {
            // ARRANGE
            var driver = Substitute.For<IVpnEnumeratorDriver>();

            driver.GetVpns().Returns(Task.FromResult(new[]
            {
                new Vpn("ID1", "VPN1"),
                new Vpn("ID2", "VPN2"),
                new Vpn("ID3", "VPN3"),
                new Vpn("ID4", "VPN4")
            }));

            var sut = new VpnEnumerator(driver)
            {
                Vpns =
                {
                    new Vpn("ID1", "VPN1"),
                    new Vpn("ID2", "VPN2"),
                    new Vpn("ID3", "VPN3"),
                    new Vpn("ID4", "VPN4")
                }
            };

            using (var monitor = sut.Monitor())
            {
                // ACT
                await sut.Refresh();

                // ASSERT
                monitor.Should().NotRaise(nameof(VpnEnumerator.Added));
                monitor.Should().NotRaise(nameof(VpnEnumerator.Removed));
                monitor.Should().NotRaise(nameof(VpnEnumerator.Error));
            }
        }

        [Test]
        [TestCase(new int[0], new[] { 1,2,3 }, new[] { 1,2,3 })]
        [TestCase(new[] { 1,2,3 }, new int[0], new int[0])]
        [TestCase(new[] { 1,2,3 }, new[] { 1,2 }, new[] { 1,2 })]
        [TestCase(new[] { 1,2,3 }, new[] { 2,1 }, new[] { 1,2 })]
        [TestCase(new[] { 1,2,3 }, new[] { 3,2 }, new[] { 2,3 })]
        [TestCase(new[] { 1,2,3 }, new[] { 1,2,3,4 }, new[] { 1,2,3,4 })]
        [TestCase(new[] { 1,2,3 }, new[] { 4,3,2,1 }, new[] { 1,2,3,4 })]
        [TestCase(new[] { 1,2,3 }, new[] { 2,3,4 }, new[] { 2,3,4 })]
        [TestCase(new[] { 1,2,3 }, new[] { 3,4,2 }, new[] { 2,3,4 })]
        [TestCase(new[] { 1,2,3 }, new[] { 4,2,3 }, new[] { 2,3,4 })]
        [TestCase(new[] { 1,2,3 }, new[] { 3,1,4 }, new[] { 1,3,4 })]
        [TestCase(new[] { 1,2,3 }, new[] { 4,2,1 }, new[] { 1,2,4 })]
        [TestCase(new[] { 1,2,3 }, new[] { 1,5,4 }, new[] { 1,5,4 })]
        public async Task Refresh_ShouldSynchronizeVpns(int[] initialIds, int[] fromDriverIds, int[] expectedIds)
        {
            // ARRANGE
            var driver = Substitute.For<IVpnEnumeratorDriver>();

            driver.GetVpns().Returns(
                Task.FromResult(
                    fromDriverIds
                        .Select(i => new Vpn($"ID{i}", $"VPN{i}"))
                        .ToArray()));

            var sut = new VpnEnumerator(driver);
            sut.Vpns.AddRange(initialIds
                .Select(i => new Vpn($"ID{i}", $"VPN{i}")));

            var expected = expectedIds
                .Select(i => new Vpn($"ID{i}", $"VPN{i}"))
                .ToList();

            // ACT
            await sut.Refresh();

            // ASSERT
            sut.Vpns.Should().BeEquivalentTo(expected);
        }
    }
}
