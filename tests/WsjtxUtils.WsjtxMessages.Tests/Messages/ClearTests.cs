using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics.CodeAnalysis;
using WsjtxUtils.WsjtxMessages.Messages;

namespace WsjtxUtils.WsjtxMessages.Tests.Messages
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class ClearTests
    {
        internal static Memory<byte> ClearMessage = new(new byte[] { 0xAD, 0xBC, 0xCB, 0xDA, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x03, 0x00, 0x00, 0x00, 0x10, 0x57, 0x53, 0x4A, 0x54, 0x2D, 0x58, 0x20, 0x2D, 0x20, 0x53, 0x6C, 0x69, 0x63, 0x65, 0x2D, 0x41, 0x00 });

        [TestMethod()]
        public void CreateClearMessageFrom_RawBytes_ProducesValidMessage()
        {
            var result = ClearMessage.DeserializeWsjtxMessage();
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(Clear));

            Clear message = result as Clear;
            Assert.AreEqual(WsjtxConstants.MagicNumber, message.MagicNumber);
            Assert.AreEqual(SchemaVersion.Version2, message.SchemaVersion);
            Assert.AreEqual(MessageType.Clear, message.MessageType);
            Assert.AreEqual("WSJT-X - Slice-A", message.Id);

        }

        [TestMethod()]
        public void WriteClearMessageWithDefaultConstructorTo_RawBytes_ProducesValidOutput()
        {
            Clear message = new()
            {
                Id = "WSJT-X - Slice-A",
                Window = ClearWindow.BandActivity
            };

            // Allocate memory and write the message
            var buffer = new byte[ClearMessage.Length];
            var bufferMem = buffer.AsMemory();
            var written = message.WriteMessageTo(bufferMem);

            Assert.AreEqual(ClearMessage.Length, written);
            CollectionAssert.AreEqual(ClearMessage.ToArray(), buffer);
        }

        [TestMethod()]
        public void WriteClearMessageTo_RawBytes_ProducesValidOutput()
        {
            Clear message = new("WSJT-X - Slice-A", ClearWindow.BandActivity);

            // Allocate memory and write the message
            var buffer = new byte[ClearMessage.Length];
            var bufferMem = buffer.AsMemory();
            var written = message.WriteMessageTo(bufferMem);

            Assert.AreEqual(ClearMessage.Length, written);
            CollectionAssert.AreEqual(ClearMessage.ToArray(), buffer);
        }
    }
}
