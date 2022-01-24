using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using WsjtxUtils.WsjtxMessages.Messages;

namespace WsjtxUtils.WsjtxUdpServer.Tests
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class WsjtxUdpServerTests
    {
        internal static Memory<byte> HeartbeatMessage = new(new byte[] { 0xad, 0xbc, 0xcb, 0xda, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x06, 0x57, 0x53, 0x4a, 0x54, 0x2d, 0x58, 0x00, 0x00, 0x00, 0x03, 0x00, 0x00, 0x00, 0x05, 0x32, 0x2e, 0x34, 0x2e, 0x30, 0x00, 0x00, 0x00, 0x06, 0x63, 0x31, 0x39, 0x64, 0x36, 0x32 });
        internal static Memory<byte> StatusMessage = new(new byte[] { 0xad, 0xbc, 0xcb, 0xda, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x06, 0x57, 0x53, 0x4a, 0x54, 0x2d, 0x58, 0x00, 0x00, 0x00, 0x00, 0x00, 0xd6, 0xc0, 0x90, 0x00, 0x00, 0x00, 0x03, 0x46, 0x54, 0x38, 0x00, 0x00, 0x00, 0x06, 0x45, 0x41, 0x34, 0x45, 0x4a, 0x50, 0x00, 0x00, 0x00, 0x02, 0x2d, 0x35, 0x00, 0x00, 0x00, 0x03, 0x46, 0x54, 0x38, 0x00, 0x00, 0x01, 0x00, 0x00, 0x02, 0xe1, 0x00, 0x00, 0x02, 0xe1, 0x00, 0x00, 0x00, 0x06, 0x4b, 0x43, 0x33, 0x50, 0x49, 0x42, 0x00, 0x00, 0x00, 0x06, 0x45, 0x4e, 0x39, 0x30, 0x58, 0x4a, 0x00, 0x00, 0x00, 0x04, 0x49, 0x4e, 0x38, 0x30, 0x00, 0xff, 0xff, 0xff, 0xff, 0x00, 0x00, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0x00, 0x00, 0x00, 0x07, 0x44, 0x65, 0x66, 0x61, 0x75, 0x6c, 0x74, 0x00, 0x00, 0x00, 0x25, 0x45, 0x41, 0x34, 0x45, 0x4a, 0x50, 0x20, 0x4b, 0x43, 0x33, 0x50, 0x49, 0x42, 0x20, 0x2d, 0x30, 0x35, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20 });
        internal static Memory<byte> DecodeMessage = new(new byte[] { 0xad, 0xbc, 0xcb, 0xda, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x06, 0x57, 0x53, 0x4a, 0x54, 0x2d, 0x58, 0x01, 0x04, 0xf6, 0x4b, 0x50, 0xff, 0xff, 0xff, 0xfe, 0x3f, 0xc9, 0x99, 0x99, 0xa0, 0x00, 0x00, 0x00, 0x00, 0x00, 0x05, 0xdb, 0x00, 0x00, 0x00, 0x01, 0x7e, 0x00, 0x00, 0x00, 0x0f, 0x57, 0x33, 0x55, 0x53, 0x20, 0x45, 0x41, 0x39, 0x41, 0x43, 0x52, 0x20, 0x2d, 0x31, 0x35, 0x00, 0x00 });
        internal static Memory<byte> ClearMessage = new(new byte[] { 0xAD, 0xBC, 0xCB, 0xDA, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x03, 0x00, 0x00, 0x00, 0x10, 0x57, 0x53, 0x4A, 0x54, 0x2D, 0x58, 0x20, 0x2D, 0x20, 0x53, 0x6C, 0x69, 0x63, 0x65, 0x2D, 0x41, 0x00 });
        internal static Memory<byte> OsoLoggedMessage = new(new byte[] { 0xAD, 0xBC, 0xCB, 0xDA, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x05, 0x00, 0x00, 0x00, 0x10, 0x57, 0x53, 0x4A, 0x54, 0x2D, 0x58, 0x20, 0x2D, 0x20, 0x53, 0x6C, 0x69, 0x63, 0x65, 0x2D, 0x41, 0x00, 0x00, 0x00, 0x00, 0x00, 0x25, 0x87, 0x9E, 0x01, 0x2A, 0xEB, 0xFA, 0x01, 0x00, 0x00, 0x00, 0x05, 0x45, 0x41, 0x35, 0x52, 0x57, 0x00, 0x00, 0x00, 0x04, 0x49, 0x4D, 0x39, 0x39, 0x00, 0x00, 0x00, 0x00, 0x00, 0x6B, 0xFA, 0x41, 0x00, 0x00, 0x00, 0x03, 0x46, 0x54, 0x38, 0x00, 0x00, 0x00, 0x03, 0x2D, 0x30, 0x36, 0x00, 0x00, 0x00, 0x03, 0x2D, 0x31, 0x38, 0x00, 0x00, 0x00, 0x02, 0x35, 0x30, 0x00, 0x00, 0x00, 0x19, 0x46, 0x54, 0x38, 0x20, 0x20, 0x53, 0x65, 0x6E, 0x74, 0x3A, 0x20, 0x2D, 0x30, 0x36, 0x20, 0x20, 0x52, 0x63, 0x76, 0x64, 0x3A, 0x20, 0x2D, 0x31, 0x38, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x25, 0x87, 0x9E, 0x01, 0x2A, 0x03, 0x19, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x06, 0x4B, 0x43, 0x33, 0x50, 0x49, 0x42, 0x00, 0x00, 0x00, 0x04, 0x45, 0x4E, 0x39, 0x30, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0xFF, 0xFF, 0xFF, 0xFF });
        internal static Memory<byte> CloseMessage = new(new byte[] { 0xAD, 0xBC, 0xCB, 0xDA, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x06, 0x00, 0x00, 0x00, 0x10, 0x57, 0x53, 0x4A, 0x54, 0x2D, 0x58, 0x20, 0x2D, 0x20, 0x53, 0x6C, 0x69, 0x63, 0x65, 0x2D, 0x41 });
        internal static Memory<byte> WSPRDecodeMessage = new(new byte[] { 0xAD, 0xBC, 0xCB, 0xDA, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x0A, 0x00, 0x00, 0x00, 0x10, 0x57, 0x53, 0x4A, 0x54, 0x2D, 0x58, 0x20, 0x2D, 0x20, 0x53, 0x6C, 0x69, 0x63, 0x65, 0x2D, 0x41, 0x01, 0x04, 0x95, 0xB4, 0xC0, 0xFF, 0xFF, 0xFF, 0xF5, 0x3F, 0xD9, 0x99, 0x99, 0xA0, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0xD7, 0x1A, 0x9A, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x04, 0x4B, 0x35, 0x58, 0x4C, 0x00, 0x00, 0x00, 0x04, 0x45, 0x4D, 0x31, 0x32, 0x00, 0x00, 0x00, 0x21, 0x00 });
        internal static Memory<byte> LoggedAdiMessage = new(new byte[] { 0xAD, 0xBC, 0xCB, 0xDA, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x0C, 0x00, 0x00, 0x00, 0x10, 0x57, 0x53, 0x4A, 0x54, 0x2D, 0x58, 0x20, 0x2D, 0x20, 0x53, 0x6C, 0x69, 0x63, 0x65, 0x2D, 0x41, 0x00, 0x00, 0x01, 0x53, 0x0A, 0x3C, 0x61, 0x64, 0x69, 0x66, 0x5F, 0x76, 0x65, 0x72, 0x3A, 0x35, 0x3E, 0x33, 0x2E, 0x31, 0x2E, 0x30, 0x0A, 0x3C, 0x70, 0x72, 0x6F, 0x67, 0x72, 0x61, 0x6D, 0x69, 0x64, 0x3A, 0x36, 0x3E, 0x57, 0x53, 0x4A, 0x54, 0x2D, 0x58, 0x0A, 0x3C, 0x45, 0x4F, 0x48, 0x3E, 0x0A, 0x3C, 0x63, 0x61, 0x6C, 0x6C, 0x3A, 0x35, 0x3E, 0x45, 0x41, 0x35, 0x52, 0x57, 0x20, 0x3C, 0x67, 0x72, 0x69, 0x64, 0x73, 0x71, 0x75, 0x61, 0x72, 0x65, 0x3A, 0x34, 0x3E, 0x49, 0x4D, 0x39, 0x39, 0x20, 0x3C, 0x6D, 0x6F, 0x64, 0x65, 0x3A, 0x33, 0x3E, 0x46, 0x54, 0x38, 0x20, 0x3C, 0x72, 0x73, 0x74, 0x5F, 0x73, 0x65, 0x6E, 0x74, 0x3A, 0x33, 0x3E, 0x2D, 0x30, 0x36, 0x20, 0x3C, 0x72, 0x73, 0x74, 0x5F, 0x72, 0x63, 0x76, 0x64, 0x3A, 0x33, 0x3E, 0x2D, 0x31, 0x38, 0x20, 0x3C, 0x71, 0x73, 0x6F, 0x5F, 0x64, 0x61, 0x74, 0x65, 0x3A, 0x38, 0x3E, 0x32, 0x30, 0x32, 0x31, 0x31, 0x32, 0x30, 0x31, 0x20, 0x3C, 0x74, 0x69, 0x6D, 0x65, 0x5F, 0x6F, 0x6E, 0x3A, 0x36, 0x3E, 0x30, 0x35, 0x32, 0x35, 0x33, 0x30, 0x20, 0x3C, 0x71, 0x73, 0x6F, 0x5F, 0x64, 0x61, 0x74, 0x65, 0x5F, 0x6F, 0x66, 0x66, 0x3A, 0x38, 0x3E, 0x32, 0x30, 0x32, 0x31, 0x31, 0x32, 0x30, 0x31, 0x20, 0x3C, 0x74, 0x69, 0x6D, 0x65, 0x5F, 0x6F, 0x66, 0x66, 0x3A, 0x36, 0x3E, 0x30, 0x35, 0x32, 0x36, 0x33, 0x30, 0x20, 0x3C, 0x62, 0x61, 0x6E, 0x64, 0x3A, 0x33, 0x3E, 0x34, 0x30, 0x6D, 0x20, 0x3C, 0x66, 0x72, 0x65, 0x71, 0x3A, 0x38, 0x3E, 0x37, 0x2E, 0x30, 0x37, 0x36, 0x34, 0x31, 0x37, 0x20, 0x3C, 0x73, 0x74, 0x61, 0x74, 0x69, 0x6F, 0x6E, 0x5F, 0x63, 0x61, 0x6C, 0x6C, 0x73, 0x69, 0x67, 0x6E, 0x3A, 0x36, 0x3E, 0x4B, 0x43, 0x33, 0x50, 0x49, 0x42, 0x20, 0x3C, 0x6D, 0x79, 0x5F, 0x67, 0x72, 0x69, 0x64, 0x73, 0x71, 0x75, 0x61, 0x72, 0x65, 0x3A, 0x34, 0x3E, 0x45, 0x4E, 0x39, 0x30, 0x20, 0x3C, 0x74, 0x78, 0x5F, 0x70, 0x77, 0x72, 0x3A, 0x32, 0x3E, 0x35, 0x30, 0x20, 0x3C, 0x63, 0x6F, 0x6D, 0x6D, 0x65, 0x6E, 0x74, 0x3A, 0x32, 0x35, 0x3E, 0x46, 0x54, 0x38, 0x20, 0x20, 0x53, 0x65, 0x6E, 0x74, 0x3A, 0x20, 0x2D, 0x30, 0x36, 0x20, 0x20, 0x52, 0x63, 0x76, 0x64, 0x3A, 0x20, 0x2D, 0x31, 0x38, 0x20, 0x3C, 0x45, 0x4F, 0x52, 0x3E });

        [TestMethod()]
        public void WsjtxUdpServerLocalhost_WithMockMessageHandler_ParsesMessagesAndExecutesCorrectMessageHandler()
        {
            // Arrange
            var mockHandler = new Mock<IWsjtxUdpMessageHandler>();
            mockHandler.Setup(handler => handler.HandleHeartbeatMessageAsync(It.IsAny<WsjtxUdpServer>(), It.IsAny<Heartbeat>(), It.IsAny<EndPoint>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask).Verifiable();
            mockHandler.Setup(handler => handler.HandleStatusMessageAsync(It.IsAny<WsjtxUdpServer>(), It.IsAny<Status>(), It.IsAny<EndPoint>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask).Verifiable();
            mockHandler.Setup(handler => handler.HandleDecodeMessageAsync(It.IsAny<WsjtxUdpServer>(), It.IsAny<Decode>(), It.IsAny<EndPoint>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask).Verifiable();
            mockHandler.Setup(handler => handler.HandleClearMessageAsync(It.IsAny<WsjtxUdpServer>(), It.IsAny<Clear>(), It.IsAny<EndPoint>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask).Verifiable();
            mockHandler.Setup(handler => handler.HandleQsoLoggedMessageAsync(It.IsAny<WsjtxUdpServer>(), It.IsAny<QsoLogged>(), It.IsAny<EndPoint>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask).Verifiable();
            mockHandler.Setup(handler => handler.HandleClosedMessageAsync(It.IsAny<WsjtxUdpServer>(), It.IsAny<Close>(), It.IsAny<EndPoint>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask).Verifiable();
            mockHandler.Setup(handler => handler.HandleWSPRDecodeMessageAsync(It.IsAny<WsjtxUdpServer>(), It.IsAny<WSPRDecode>(), It.IsAny<EndPoint>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask).Verifiable();
            mockHandler.Setup(handler => handler.HandleLoggedAdifMessageAsync(It.IsAny<WsjtxUdpServer>(), It.IsAny<LoggedAdif>(), It.IsAny<EndPoint>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask).Verifiable();

            var port = new Random().Next(1024, 65534);

            using (var server = new WsjtxUdpServer(mockHandler.Object, IPAddress.Loopback, port))
            using (var client = new UdpClient())
            {
                var serverEndpoint = new IPEndPoint(IPAddress.Loopback, port);
                var cancellationTokenSource = new CancellationTokenSource();

                // Act
                server.Start(cancellationTokenSource);

                client.Send(HeartbeatMessage.Span, serverEndpoint);
                client.Send(StatusMessage.Span, serverEndpoint);
                client.Send(DecodeMessage.Span, serverEndpoint);
                client.Send(ClearMessage.Span, serverEndpoint);
                client.Send(OsoLoggedMessage.Span, serverEndpoint);
                client.Send(CloseMessage.Span, serverEndpoint);
                client.Send(WSPRDecodeMessage.Span, serverEndpoint);
                client.Send(LoggedAdiMessage.Span, serverEndpoint);

                // Assert
                Assert.IsTrue(server.IsRunning);
                Assert.IsFalse(server.IsMulticast);

                Thread.Sleep(1000);

                // Verify 
                mockHandler.Verify(handler => handler.HandleHeartbeatMessageAsync(It.IsAny<WsjtxUdpServer>(), It.IsAny<Heartbeat>(), It.IsAny<EndPoint>(), It.IsAny<CancellationToken>()));
                mockHandler.Verify(handler => handler.HandleStatusMessageAsync(It.IsAny<WsjtxUdpServer>(), It.IsAny<Status>(), It.IsAny<EndPoint>(), It.IsAny<CancellationToken>()));
                mockHandler.Verify(handler => handler.HandleDecodeMessageAsync(It.IsAny<WsjtxUdpServer>(), It.IsAny<Decode>(), It.IsAny<EndPoint>(), It.IsAny<CancellationToken>()), Times.Exactly(1));
                mockHandler.Verify(handler => handler.HandleClearMessageAsync(It.IsAny<WsjtxUdpServer>(), It.IsAny<Clear>(), It.IsAny<EndPoint>(), It.IsAny<CancellationToken>()), Times.Exactly(1));
                mockHandler.Verify(handler => handler.HandleQsoLoggedMessageAsync(It.IsAny<WsjtxUdpServer>(), It.IsAny<QsoLogged>(), It.IsAny<EndPoint>(), It.IsAny<CancellationToken>()), Times.Exactly(1));
                mockHandler.Verify(handler => handler.HandleClosedMessageAsync(It.IsAny<WsjtxUdpServer>(), It.IsAny<Close>(), It.IsAny<EndPoint>(), It.IsAny<CancellationToken>()), Times.Exactly(1));
                mockHandler.Verify(handler => handler.HandleWSPRDecodeMessageAsync(It.IsAny<WsjtxUdpServer>(), It.IsAny<WSPRDecode>(), It.IsAny<EndPoint>(), It.IsAny<CancellationToken>()), Times.Exactly(1));
                mockHandler.Verify(handler => handler.HandleLoggedAdifMessageAsync(It.IsAny<WsjtxUdpServer>(), It.IsAny<LoggedAdif>(), It.IsAny<EndPoint>(), It.IsAny<CancellationToken>()), Times.Exactly(1));
            }
        }

        [TestMethod()]
        public void WsjtxUdpServerMulticast_WithMockMessageHandler_ParsesMessagesAndExecutesCorrectMessageHandler()
        {
            // Arrange
            var mockHandler = new Mock<IWsjtxUdpMessageHandler>();
            mockHandler.Setup(handler => handler.HandleHeartbeatMessageAsync(It.IsAny<WsjtxUdpServer>(), It.IsAny<Heartbeat>(), It.IsAny<EndPoint>(), It.IsAny<CancellationToken>()));

            var server = new WsjtxUdpServer(mockHandler.Object, IPAddress.Parse("239.1.1.1"), 2237);
            var serverEndpoint = new IPEndPoint(IPAddress.Parse("239.1.1.1"), 2237);
            var client = new UdpClient();
            var cancellationTokenSource = new CancellationTokenSource();

            // Act
            server.Start(cancellationTokenSource);

            client.Send(HeartbeatMessage.Span, serverEndpoint);

            // allow any remaing packets to be processed
            Thread.Sleep(1000);

            // Assert
            Assert.IsTrue(server.IsRunning);
            Assert.IsTrue(server.IsMulticast);

            cancellationTokenSource.Cancel();
            server.Stop();
            Assert.IsFalse(server.IsRunning);
        }
    }
}