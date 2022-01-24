using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics.CodeAnalysis;
using WsjtxUtils.WsjtxMessages.Messages;

namespace WsjtxUtils.WsjtxMessages.Tests.Messages
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class HaltTxTests
    {
        internal static Memory<byte> HaltTxMessage = new(new byte[] { 0xAD, 0xBC, 0xCB, 0xDA, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x08, 0x00, 0x00, 0x00, 0x06, 0x57, 0x53, 0x4A, 0x54, 0x2D, 0x58, 0x01 });



        [TestMethod()]
        public void WriteHaltTxMessageWithDefaultConstructorTo_RawBytes_ProducesValidOutput()
        {
            HaltTx message = new()
            {
                Id = "WSJT-X",
                AutoTxOnly = true
            };

            // Allocate memory and write the message
            var buffer = new byte[HaltTxMessage.Length];
            var bufferMem = buffer.AsMemory();
            var written = message.WriteMessageTo(bufferMem);

            Assert.AreEqual(HaltTxMessage.Length, written);
            CollectionAssert.AreEqual(HaltTxMessage.ToArray(), buffer);
        }

        [TestMethod()]
        public void WriteHaltTxMessageTo_RawBytes_ProducesValidOutput()
        {
            HaltTx message = new("WSJT-X", true);

            // Allocate memory and write the message
            var buffer = new byte[HaltTxMessage.Length];
            var bufferMem = buffer.AsMemory();
            var written = message.WriteMessageTo(bufferMem);

            Assert.AreEqual(HaltTxMessage.Length, written);
            CollectionAssert.AreEqual(HaltTxMessage.ToArray(), buffer);
        }
    }
}
