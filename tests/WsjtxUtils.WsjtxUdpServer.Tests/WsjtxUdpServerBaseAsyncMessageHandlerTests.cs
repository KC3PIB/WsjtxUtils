using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
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
        public class TestWsjtxUdpServerBaseAsyncMessageHandlerImpl : WsjtxUdpServerBaseAsyncMessageHandler { }

        [TestMethod()]
        public void WsjtxUdpServerLocalhost_WithMockMessageHandler_ParsesMessagesAndExecutesCorrectMessageHandler()
        {
            // Arrange
            var clientConnectedFunc = new Mock<Func<WsjtxConnectedClient, Task>?>();
            var clientClosedFunc = new Mock<Func<WsjtxConnectedClient, Task>?>();
            var clientExpiredFunc = new Mock<Func<WsjtxConnectedClient, Task>?>();

            var handler = new TestWsjtxUdpServerBaseAsyncMessageHandlerImpl()
            {
                ClientConnectedCallback = clientConnectedFunc.Object,
                ClientClosedCallback = clientClosedFunc.Object,
                ClientExpiredCallback = clientExpiredFunc.Object,
                ConnectedClientExpiryInSeconds = 1

            };

            var mockServer = new Mock<WsjtxUdpServer>(handler, IPAddress.Loopback, 2237, 1500);

            var clientA = new WsjtxConnectedClient("test-client-a", new IPEndPoint(IPAddress.Loopback, 2237), null);
            var clientB = new WsjtxConnectedClient("test-client-b", new IPEndPoint(IPAddress.Loopback, 2238), null);

            clientConnectedFunc.Setup(callback => callback.Invoke(It.IsAny<WsjtxConnectedClient>()));
            clientClosedFunc.Setup(callback => callback.Invoke(It.IsAny<WsjtxConnectedClient>()));
            clientExpiredFunc.Setup(callback => callback.Invoke(It.IsAny<WsjtxConnectedClient>()));

            // Act

            // client A connects and sends messages
            handler.HandleHeartbeatMessageAsync(mockServer.Object, new Heartbeat() { Id = clientA.ClientId }, clientA.Endpoint);
            handler.HandleClearMessageAsync(mockServer.Object, new Clear() { Id = clientA.ClientId }, clientA.Endpoint);
            handler.HandleDecodeMessageAsync(mockServer.Object, new Decode() { Id = clientA.ClientId }, clientA.Endpoint);
            handler.HandleLoggedAdifMessageAsync(mockServer.Object, new LoggedAdif() { Id = clientA.ClientId }, clientA.Endpoint);
            handler.HandleQsoLoggedMessageAsync(mockServer.Object, new QsoLogged() { Id = clientA.ClientId }, clientA.Endpoint);
            handler.HandleWSPRDecodeMessageAsync(mockServer.Object, new WSPRDecode() { Id = clientA.ClientId }, clientA.Endpoint);

            // client B connects and sends messages
            handler.HandleStatusMessageAsync(mockServer.Object, new Status() { Id = clientB.ClientId }, clientB.Endpoint);
            handler.HandleDecodeMessageAsync(mockServer.Object, new Decode() { Id = clientB.ClientId }, clientB.Endpoint);

            // assert that connected clients list is correct
            Assert.IsTrue(handler.ConnectedClients.Count == 2);
            Assert.IsTrue(handler.ConnectedClients.Keys.Contains(clientA.ClientId));
            Assert.IsTrue(handler.ConnectedClients.Keys.Contains(clientB.ClientId));
            Assert.IsNull(handler.ConnectedClients[clientA.ClientId].Status);
            Assert.IsNotNull(handler.ConnectedClients[clientB.ClientId].Status);

            // client A closes
            handler.HandleClosedMessageAsync(mockServer.Object, new Close() { Id = clientA.ClientId }, clientA.Endpoint);

            // assert that connected clients list is correct and sleep to allow for expiry
            Assert.IsTrue(handler.ConnectedClients.Count == 1);
            Thread.Sleep(1000);

            // client A reconnects and client B expires
            handler.HandleStatusMessageAsync(mockServer.Object, new Status() { Id = clientA.ClientId }, clientA.Endpoint);

            // assert that connected clients list is correct after expiry
            Assert.IsTrue(handler.ConnectedClients.Count == 1);

            // Verify
            clientConnectedFunc.Verify(callback => callback.Invoke(It.IsAny<WsjtxConnectedClient>()), Times.Exactly(3));
            clientClosedFunc.Verify(callback => callback.Invoke(It.IsAny<WsjtxConnectedClient>()), Times.Once);
            clientExpiredFunc.Verify(callback => callback.Invoke(It.IsAny<WsjtxConnectedClient>()), Times.Once);
        }
    }
}