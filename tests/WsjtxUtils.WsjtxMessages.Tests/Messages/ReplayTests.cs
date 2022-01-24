using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics.CodeAnalysis;
using WsjtxUtils.WsjtxMessages.Messages;

namespace WsjtxUtils.WsjtxMessages.Tests.Messages
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class ReplayTests
    {
        internal static Memory<byte> ReplayMessage = new(new byte[] { 0xAD, 0xBC, 0xCB, 0xDA, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x07, 0x00, 0x00, 0x00, 0x10, 0x57, 0x53, 0x4A, 0x54, 0x2D, 0x58, 0x20, 0x2D, 0x20, 0x53, 0x6C, 0x69, 0x63, 0x65, 0x2D, 0x41 });

        [TestMethod()]
        public void WriteReplayMessageWithDefaultConstructorTo_RawBytes_ProducesValidOutput()
        {
            Replay message = new()
            {
                Id = "WSJT-X - Slice-A"
            };

            // Allocate memory and write the message
            var buffer = new byte[ReplayMessage.Length];
            var bufferMem = buffer.AsMemory();
            var written = message.WriteMessageTo(bufferMem);

            Assert.AreEqual(ReplayMessage.Length, written);
            CollectionAssert.AreEqual(ReplayMessage.ToArray(), buffer);
        }
    }
}
