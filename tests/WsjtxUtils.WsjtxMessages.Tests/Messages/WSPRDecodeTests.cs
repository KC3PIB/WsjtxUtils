using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics.CodeAnalysis;
using WsjtxUtils.WsjtxMessages.Messages;

namespace WsjtxUtils.WsjtxMessages.Tests.Messages
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class WSPRDecodeTests
    {
        internal static Memory<byte> WSPRDecodeMessage = new(new byte[] { 0xAD, 0xBC, 0xCB, 0xDA, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x0A, 0x00, 0x00, 0x00, 0x10, 0x57, 0x53, 0x4A, 0x54, 0x2D, 0x58, 0x20, 0x2D, 0x20, 0x53, 0x6C, 0x69, 0x63, 0x65, 0x2D, 0x41, 0x01, 0x04, 0x95, 0xB4, 0xC0, 0xFF, 0xFF, 0xFF, 0xF5, 0x3F, 0xD9, 0x99, 0x99, 0xA0, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0xD7, 0x1A, 0x9A, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x04, 0x4B, 0x35, 0x58, 0x4C, 0x00, 0x00, 0x00, 0x04, 0x45, 0x4D, 0x31, 0x32, 0x00, 0x00, 0x00, 0x21, 0x00 });

        [TestMethod()]
        public void CreateWSPRDecodeMessageFrom_RawBytes_ProducesValidMessage()
        {
            var result = WSPRDecodeMessage.DeserializeWsjtxMessage();
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(WSPRDecode));

            WSPRDecode message = result as WSPRDecode;
            Assert.AreEqual(WsjtxConstants.MagicNumber, message.MagicNumber);
            Assert.AreEqual(SchemaVersion.Version2, message.SchemaVersion);
            Assert.AreEqual(MessageType.WSPRDecode, message.MessageType);
            Assert.AreEqual("WSJT-X - Slice-A", message.Id);
            Assert.IsTrue(message.New);
            Assert.AreEqual(76920000U, message.Time);
            Assert.AreEqual(-11, message.Snr);
            Assert.AreEqual(0.4f, message.DeltaTimeSeconds);
            Assert.AreEqual(14097050U, message.FrequencyHz);
            Assert.AreEqual(0, message.FrequencyDriftHz);
            Assert.AreEqual("K5XL", message.Callsign);
            Assert.AreEqual("EM12", message.Grid);
            Assert.AreEqual(33, message.Power);
            Assert.IsFalse(message.OffAir);
        }
    }
}
