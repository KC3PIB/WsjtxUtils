using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics.CodeAnalysis;
using WsjtxUtils.WsjtxMessages.Messages;

namespace WsjtxUtils.WsjtxMessages.Tests.Messages
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class ConfigureTests
    {
        internal static Memory<byte> ConfigureMessage = new(new byte[] { 0xAD, 0xBC, 0xCB, 0xDA, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x0F, 0x00, 0x00, 0x00, 0x10, 0x57, 0x53, 0x4A, 0x54, 0x2D, 0x58, 0x20, 0x2D, 0x20, 0x53, 0x6C, 0x69, 0x63, 0x65, 0x2D, 0x41, 0x00, 0x00, 0x00, 0x03, 0x46, 0x54, 0x34, 0xFF, 0xFF, 0xFF, 0xFF, 0x00, 0x00, 0x00, 0x00, 0x00, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 });

        [TestMethod()]
        public void WriteFreeTextMessageWithDefaultConstructorTo_RawBytes_ProducesValidOutput()
        {
            Configure message = new()
            {
                Id = "WSJT-X - Slice-A",
                Mode = "FT4"

            };

            // Allocate memory and write the message
            var buffer = new byte[ConfigureMessage.Length];
            var bufferMem = buffer.AsMemory();
            var written = message.WriteMessageTo(bufferMem);

            Assert.AreEqual(ConfigureMessage.Length, written);
            CollectionAssert.AreEqual(ConfigureMessage.ToArray(), buffer);
        }

        [TestMethod()]
        public void WriteFreeTextMessageTo_RawBytes_ProducesValidOutput()
        {
            Configure message = new("WSJT-X - Slice-A") { Mode = "FT4"};

            // Allocate memory and write the message
            var buffer = new byte[ConfigureMessage.Length];
            var bufferMem = buffer.AsMemory();
            var written = message.WriteMessageTo(bufferMem);

            Assert.AreEqual(ConfigureMessage.Length, written);
            CollectionAssert.AreEqual(ConfigureMessage.ToArray(), buffer);
        }
    }
}
