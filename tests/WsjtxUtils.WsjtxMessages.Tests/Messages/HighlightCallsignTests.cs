using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using WsjtxUtils.WsjtxMessages.Messages;

namespace WsjtxUtils.WsjtxMessages.Tests.Messages
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class HighlightCallsignTests
    {
        internal static Memory<byte> HighlightCallsignMessage = new(new byte[] { 0xAD, 0xBC, 0xCB, 0xDA, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x0D, 0x00, 0x00, 0x00, 0x06, 0x57, 0x53, 0x4A, 0x54, 0x2D, 0x58, 0x00, 0x00, 0x00, 0x06, 0x4B, 0x43, 0x33, 0x50, 0x49, 0x42, 0x01, 0xFF, 0x00, 0xFF, 0x00, 0xFF, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0xFF, 0x00, 0xFF, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 });

        [TestMethod()]
        public void WriteHighlightCallsignMessageWithDefaultConstructorTo_RawBytes_ProducesValidOutput()
        {
            HighlightCallsign message = new()
            {
                Id = "WSJT-X",
                Callsign = "KC3PIB",
                BackgroundColor = new QColor(Color.Yellow),
                ForegroundColor = new QColor(Color.Red)
            };

            // Allocate memory and write the message
            var buffer = new byte[HighlightCallsignMessage.Length];
            var bufferMem = buffer.AsMemory();
            var written = message.WriteMessageTo(bufferMem);

            Assert.AreEqual(HighlightCallsignMessage.Length, written);
            CollectionAssert.AreEqual(HighlightCallsignMessage.ToArray(), buffer);
        }

        [TestMethod()]
        public void WriteHighlightCallsignMessageTo_RawBytes_ProducesValidOutput()
        {
            HighlightCallsign message = new("WSJT-X", "KC3PIB", Color.Yellow, Color.Red);

            // Allocate memory and write the message
            var buffer = new byte[HighlightCallsignMessage.Length];
            var bufferMem = buffer.AsMemory();
            var written = message.WriteMessageTo(bufferMem);

            Assert.AreEqual(HighlightCallsignMessage.Length, written);
            CollectionAssert.AreEqual(HighlightCallsignMessage.ToArray(), buffer);
        }
    }
}
