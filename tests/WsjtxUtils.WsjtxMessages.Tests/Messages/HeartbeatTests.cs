using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics.CodeAnalysis;
using WsjtxUtils.WsjtxMessages.Messages;

namespace WsjtxUtils.WsjtxMessages.Tests.Messages
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class HeartbeatTests
    {
        internal static Memory<byte> HeartbeatMessage = new(new byte[] {
            0xad, 0xbc, 0xcb, 0xda, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x06, 0x57, 0x53, 0x4a, 0x54, 0x2d, 0x58, 0x00, 0x00,
            0x00, 0x03, 0x00, 0x00, 0x00, 0x05, 0x32, 0x2e, 0x34, 0x2e, 0x30, 0x00,
            0x00, 0x00, 0x06, 0x63, 0x31, 0x39, 0x64, 0x36, 0x32 });

        [TestMethod()]
        public void CreateHeartbeatMessageFrom_RawBytes_ProducesValidMessage()
        {
            var result = HeartbeatMessage.DeserializeWsjtxMessage();
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(Heartbeat));

            Heartbeat message = result as Heartbeat;
            Assert.AreEqual(WsjtxConstants.MagicNumber, message.MagicNumber);
            Assert.AreEqual(SchemaVersion.Version2, message.SchemaVersion);
            Assert.AreEqual(MessageType.Heartbeat, message.MessageType);
            Assert.AreEqual("WSJT-X", message.Id);
            Assert.AreEqual(SchemaVersion.Version3, message.MaximumSchemaNumber);
            Assert.AreEqual("2.4.0", message.Version);
            Assert.AreEqual("c19d62", message.Revision);
        }

        [TestMethod()]
        public void WriteHeartbeatMessageTo_RawBytes_ProducesValidOutput()
        {
            Heartbeat message = new("WSJT-X", "2.4.0", "c19d62");

            // Allocate memory and write the message
            var buffer = new byte[HeartbeatMessage.Length];
            var bufferMem = buffer.AsMemory();
            var written = message.WriteMessageTo(bufferMem);

            Assert.AreEqual(HeartbeatMessage.Length, written);
            CollectionAssert.AreEqual(HeartbeatMessage.ToArray(), buffer);
        }

        [TestMethod()]
        public void WriteHeartbeatMessageWithDefaultConstructorTo_RawBytes_ProducesValidOutput()
        {
            Heartbeat message = new()
            {
                Id = "WSJT-X",
                MaximumSchemaNumber = SchemaVersion.Version3,
                Version = "2.4.0",
                Revision = "c19d62"
            };

            // Allocate memory and write the message
            var buffer = new byte[HeartbeatMessage.Length];
            var bufferMem = buffer.AsMemory();
            var written = message.WriteMessageTo(bufferMem);

            Assert.AreEqual(HeartbeatMessage.Length, written);
            CollectionAssert.AreEqual(HeartbeatMessage.ToArray(), buffer);
        }
    }
}
