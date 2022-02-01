using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics.CodeAnalysis;
using WsjtxUtils.WsjtxMessages.Messages;

namespace WsjtxUtils.WsjtxMessages.Tests.Messages
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class FreeTextTests
    {
        internal static Memory<byte> FreeTextMessage = new(new byte[] { 0xAD, 0xBC, 0xCB, 0xDA, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x09, 0x00, 0x00, 0x00, 0x06, 0x57, 0x53, 0x4A, 0x54, 0x2D, 0x58, 0x00, 0x00, 0x00, 0x09, 0x54, 0x4E, 0x58, 0x20, 0x37, 0x33, 0x20, 0x47, 0x4C, 0x01 });

        [TestMethod()]
        public void WriteFreeTextMessageWithDefaultConstructorTo_RawBytes_ProducesValidOutput()
        {
            FreeText message = new()
            {
                Id = "WSJT-X",
                Text = "TNX 73 GL",
                Send = true
            };

            // Allocate memory and write the message
            var buffer = new byte[FreeTextMessage.Length];
            var bufferMem = buffer.AsMemory();
            var written = message.WriteMessageTo(bufferMem);

            Assert.AreEqual(FreeTextMessage.Length, written);
            CollectionAssert.AreEqual(FreeTextMessage.ToArray(), buffer);
        }

        [TestMethod()]
        public void WriteFreeTextMessageTo_RawBytes_ProducesValidOutput()
        {
            FreeText message = new("WSJT-X", "TNX 73 GL", true);

            // Allocate memory and write the message
            var buffer = new byte[FreeTextMessage.Length];
            var bufferMem = buffer.AsMemory();
            var written = message.WriteMessageTo(bufferMem);

            Assert.AreEqual(FreeTextMessage.Length, written);
            CollectionAssert.AreEqual(FreeTextMessage.ToArray(), buffer);
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentException))]
        public void FreeTextMessage_ThrowsException_WhenConstructedWithStringThatExceedsLimits()
        {
            FreeText message = new FreeText("WSJT-X", "1234567890ABCD");
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentException))]
        public void FreeTextMessage_ThrowsException_WhenSettingPropWithStringThatExceedsLimits()
        {
            FreeText message = new FreeText();
            message.Text = "1234567890ABCD";
        }
    }
}
