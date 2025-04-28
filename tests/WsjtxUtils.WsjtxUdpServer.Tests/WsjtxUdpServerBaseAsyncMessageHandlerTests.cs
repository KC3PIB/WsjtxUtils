using System;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using WsjtxUtils.WsjtxMessages.Messages;

namespace WsjtxUtils.WsjtxUdpServer.Tests
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class WsjtxUdpServerBaseAsyncMessageHandlerTests
    {
        /// <summary>
        /// Test WsjtxUdpServerBaseAsyncMessageHandler Impl
        /// </summary>
        public class TestWsjtxUdpServerBaseAsyncMessageHandlerImpl : WsjtxUdpServerBaseAsyncMessageHandler
        {
        }

        [TestMethod()]
        public async Task
            WsjtxUdpServerLocalhost_WithMockMessageHandler_ParsesMessagesAndExecutesCorrectMessageHandler()
        {
            // Arrange
            var connectedMock = new Mock<Func<WsjtxConnectedClient, Task>>();
            var closedMock = new Mock<Func<WsjtxConnectedClient, Task>>();
            var expiredMock = new Mock<Func<WsjtxConnectedClient, Task>>();

            connectedMock.Setup(m => m(It.IsAny<WsjtxConnectedClient>()))
                .Returns(Task.CompletedTask);
            closedMock.Setup(m => m(It.IsAny<WsjtxConnectedClient>()))
                .Returns(Task.CompletedTask);
            expiredMock.Setup(m => m(It.IsAny<WsjtxConnectedClient>()))
                .Returns(Task.CompletedTask);

            var handler = new TestWsjtxUdpServerBaseAsyncMessageHandlerImpl()
            {
                ClientConnectedCallback = connectedMock.Object,
                ClientClosedCallback = closedMock.Object,
                ClientExpiredCallback = expiredMock.Object,
                ConnectedClientExpiryInSeconds = 1
            };

            var mockServer = new Mock<WsjtxUdpServer>(handler, IPAddress.Loopback, 2237, 1500,
                NullLogger<WsjtxUdpServer>.Instance);

            var clientA = new WsjtxConnectedClient("test-client-a", new IPEndPoint(IPAddress.Loopback, 2237), null);
            var clientB = new WsjtxConnectedClient("test-client-b", new IPEndPoint(IPAddress.Loopback, 2238), null);

            // Act

            // client A connects and sends messages
            await handler.HandleHeartbeatMessageAsync(mockServer.Object, new Heartbeat() { Id = clientA.ClientId },
                clientA.Endpoint);
            await handler.HandleClearMessageAsync(mockServer.Object, new Clear() { Id = clientA.ClientId },
                clientA.Endpoint);
            await handler.HandleDecodeMessageAsync(mockServer.Object, new Decode() { Id = clientA.ClientId },
                clientA.Endpoint);
            await handler.HandleLoggedAdifMessageAsync(mockServer.Object, new LoggedAdif() { Id = clientA.ClientId },
                clientA.Endpoint);
            await handler.HandleQsoLoggedMessageAsync(mockServer.Object, new QsoLogged() { Id = clientA.ClientId },
                clientA.Endpoint);
            await handler.HandleWSPRDecodeMessageAsync(mockServer.Object, new WSPRDecode() { Id = clientA.ClientId },
                clientA.Endpoint);

            // client B connects and sends messages
            await handler.HandleStatusMessageAsync(mockServer.Object, new Status() { Id = clientB.ClientId },
                clientB.Endpoint);
            await handler.HandleDecodeMessageAsync(mockServer.Object, new Decode() { Id = clientB.ClientId },
                clientB.Endpoint);

            // assert that connected clients list is correct
            Assert.IsTrue(handler.ConnectedClients.Count == 2);
            Assert.IsTrue(handler.ConnectedClients.ContainsKey(clientA.ClientId));
            Assert.IsTrue(handler.ConnectedClients.ContainsKey(clientB.ClientId));
            Assert.IsNull(handler.ConnectedClients[clientA.ClientId].Status);
            Assert.IsNotNull(handler.ConnectedClients[clientB.ClientId].Status);

            // client A closes
            await handler.HandleClosedMessageAsync(mockServer.Object, new Close() { Id = clientA.ClientId },
                clientA.Endpoint);

            // assert that connected clients list is correct and sleep to allow for expiry
            Assert.IsTrue(handler.ConnectedClients.Count == 1);
            Thread.Sleep(1000);

            // client A reconnects and client B expires
            await handler.HandleStatusMessageAsync(mockServer.Object, new Status() { Id = clientA.ClientId },
                clientA.Endpoint);

            // assert that connected clients list is correct after expiry
            Assert.IsTrue(handler.ConnectedClients.Count == 1);

            // Verify
            connectedMock.Verify(callback => callback!.Invoke(It.IsAny<WsjtxConnectedClient>()), Times.Exactly(3));
            closedMock.Verify(callback => callback!.Invoke(It.IsAny<WsjtxConnectedClient>()), Times.Once);
            expiredMock.Verify(callback => callback!.Invoke(It.IsAny<WsjtxConnectedClient>()), Times.Once);
        }
    }
}