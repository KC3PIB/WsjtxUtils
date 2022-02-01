using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics.CodeAnalysis;
using WsjtxUtils.WsjtxMessages.Messages;

namespace WsjtxUtils.WsjtxMessages.Tests.Messages
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class CloseTests
    {
        internal static Memory<byte> CloseMessage = new(new byte[] { 0xAD, 0xBC, 0xCB, 0xDA, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x06, 0x00, 0x00, 0x00, 0x10, 0x57, 0x53, 0x4A, 0x54, 0x2D, 0x58, 0x20, 0x2D, 0x20, 0x53, 0x6C, 0x69, 0x63, 0x65, 0x2D, 0x41 });

        [TestMethod()]
        public void CreateCloseMessageFrom_RawBytes_ProducesValidMessage()
        {
            var result = CloseMessage.DeserializeWsjtxMessage();
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(Close));

            Close message = result as Close;
            Assert.AreEqual(WsjtxConstants.MagicNumber, message.MagicNumber);
            Assert.AreEqual(SchemaVersion.Version2, message.SchemaVersion);
            Assert.AreEqual(MessageType.Close, message.MessageType);
            Assert.AreEqual("WSJT-X - Slice-A", message.Id);
        }

        [TestMethod()]
        public void WriteCloseTo_RawBytes_ProducesValidOutput()
        {
            Close message = new()
            {
                Id = "WSJT-X - Slice-A"
            };

            // Allocate memory and write the message
            var buffer = new byte[CloseMessage.Length];
            var bufferMem = buffer.AsMemory();
            var written = message.WriteMessageTo(bufferMem);

            Assert.AreEqual(CloseMessage.Length, written);
            CollectionAssert.AreEqual(CloseMessage.ToArray(), buffer);
        }
    }
}
