using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics.CodeAnalysis;
using WsjtxUtils.WsjtxMessages.Messages;

namespace WsjtxUtils.WsjtxMessages.Tests.Messages
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class LocationTests
    {
        internal static Memory<byte> LocationMessage = new(new byte[] { 0xAD, 0xBC, 0xCB, 0xDA, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x0B, 0x00, 0x00, 0x00, 0x06, 0x57, 0x53, 0x4A, 0x54, 0x2D, 0x58, 0x00, 0x00, 0x00, 0x06, 0x48, 0x4D, 0x30, 0x33, 0x68, 0x6A });

        [TestMethod()]
        public void WriteLocationMessageWithDefaultConstructorTo_RawBytes_ProducesValidOutput()
        {
            Location message = new()
            {
                Id = "WSJT-X",
                LocationGridSquare = "HM03hj"
            };

            // Allocate memory and write the message
            var buffer = new byte[LocationMessage.Length];
            var bufferMem = buffer.AsMemory();
            var written = message.WriteMessageTo(bufferMem);

            Assert.AreEqual(LocationMessage.Length, written);
            CollectionAssert.AreEqual(LocationMessage.ToArray(), buffer);
        }

        [TestMethod()]
        public void WriteLocationMessageTo_RawBytes_ProducesValidOutput()
        {
            Location message = new("WSJT-X", "HM03hj");

            // Allocate memory and write the message
            var buffer = new byte[LocationMessage.Length];
            var bufferMem = buffer.AsMemory();
            var written = message.WriteMessageTo(bufferMem);

            Assert.AreEqual(LocationMessage.Length, written);
            CollectionAssert.AreEqual(LocationMessage.ToArray(), buffer);
        }
    }
}
