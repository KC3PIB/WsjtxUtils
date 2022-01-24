using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics.CodeAnalysis;
using WsjtxUtils.WsjtxMessages.Messages;

namespace WsjtxUtils.WsjtxMessages.Tests.Messages
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class DecodeTests
    {
        internal static Memory<byte> DecodeMessage = new(new byte[] {
            0xad, 0xbc, 0xcb, 0xda, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x02,
            0x00, 0x00, 0x00, 0x06, 0x57, 0x53, 0x4a, 0x54, 0x2d, 0x58, 0x01, 0x04,
            0xf6, 0x4b, 0x50, 0xff, 0xff, 0xff, 0xfe, 0x3f, 0xc9, 0x99, 0x99, 0xa0,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x05, 0xdb, 0x00, 0x00, 0x00, 0x01, 0x7e,
            0x00, 0x00, 0x00, 0x0f, 0x57, 0x33, 0x55, 0x53, 0x20, 0x45, 0x41, 0x39,
            0x41, 0x43, 0x52, 0x20, 0x2d, 0x31, 0x35, 0x00, 0x00 });

        [TestMethod()]
        public void CreateDecodeMessageFrom_RawBytes_ProducesValidMessage()
        {
            var result = DecodeMessage.DeserializeWsjtxMessage();
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(Decode));

            Decode message = result as Decode;
            Assert.AreEqual(WsjtxConstants.MagicNumber, message.MagicNumber);
            Assert.AreEqual(SchemaVersion.Version2, message.SchemaVersion);
            Assert.AreEqual(MessageType.Decode, message.MessageType);
            Assert.AreEqual("WSJT-X", message.Id);
            Assert.IsTrue(message.New);
            Assert.AreEqual(83250000U, message.Time);
            Assert.AreEqual(-2, message.Snr);
            Assert.AreEqual(0.2f, message.OffsetTimeSeconds);
            Assert.AreEqual(1499U, message.OffsetFrequencyHz);
            Assert.AreEqual("~", message.Mode);
            Assert.AreEqual("W3US EA9ACR -15", message.Message);
            Assert.IsFalse(message.LowConfidence);
            Assert.IsFalse(message.OffAir);
        }
    }
}
