using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics.CodeAnalysis;
using WsjtxUtils.WsjtxMessages.Messages;

namespace WsjtxUtils.WsjtxMessages.Tests.Messages
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class ReplyTests
    {
        internal static Memory<byte> DecodeMessage = new(new byte[] { 0xad, 0xbc, 0xcb, 0xda, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x06, 0x57, 0x53, 0x4a, 0x54, 0x2d, 0x58, 0x01, 0x04, 0xf6, 0x4b, 0x50, 0xff, 0xff, 0xff, 0xfe, 0x3f, 0xc9, 0x99, 0x99, 0xa0, 0x00, 0x00, 0x00, 0x00, 0x00, 0x05, 0xdb, 0x00, 0x00, 0x00, 0x01, 0x7e, 0x00, 0x00, 0x00, 0x0f, 0x57, 0x33, 0x55, 0x53, 0x20, 0x45, 0x41, 0x39, 0x41, 0x43, 0x52, 0x20, 0x2d, 0x31, 0x35, 0x00, 0x00 });

        internal static Memory<byte> ReplyMessage = new(new byte[] { 0xAD, 0xBC, 0xCB, 0xDA, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x04, 0x00, 0x00, 0x00, 0x10, 0x57, 0x53, 0x4A, 0x54, 0x2D, 0x58, 0x20, 0x2D, 0x20, 0x53, 0x6C, 0x69, 0x63, 0x65, 0x2D, 0x41, 0x00, 0xB5, 0xBB, 0x70, 0xFF, 0xFF, 0xFF, 0xF6, 0x3F, 0xC9, 0x99, 0x99, 0xA0, 0x00, 0x00, 0x00, 0x00, 0x00, 0x06, 0x3E, 0x00, 0x00, 0x00, 0x01, 0x7E, 0x00, 0x00, 0x00, 0x0D, 0x43, 0x51, 0x20, 0x4B, 0x33, 0x41, 0x44, 0x55, 0x20, 0x46, 0x4E, 0x31, 0x30, 0x00, 0x00 });

        [TestMethod()]
        public void CreateReplyMessage_FromDecode_ProducesValidMessage()
        {
            var result = DecodeMessage.DeserializeWsjtxMessage();
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(Decode));

            Decode decode = result as Decode;
            Assert.IsNotNull(decode);

            var message = new Reply(decode);
            Assert.IsNotNull(message);
            Assert.IsInstanceOfType(message, typeof(Reply));
            Assert.AreEqual(WsjtxConstants.MagicNumber, message.MagicNumber);
            Assert.AreEqual(SchemaVersion.Version2, message.SchemaVersion);
            Assert.AreEqual(MessageType.Reply, message.MessageType);
            Assert.AreEqual("WSJT-X", message.Id);
            Assert.AreEqual(83250000U, message.Time);
            Assert.AreEqual(-2, message.Snr);
            Assert.AreEqual(0.2f, message.OffsetTimeSeconds);
            Assert.AreEqual(1499U, message.OffsetFrequencyHz);
            Assert.AreEqual("~", message.Mode);
            Assert.AreEqual("W3US EA9ACR -15", message.Message);
            Assert.IsFalse(message.LowConfidence);
        }

        [TestMethod()]
        public void WriteReplyMessageTo_RawBytes_ProducesValidOutput()
        {
            var decode = new Decode
            {
                Id = "WSJT-X - Slice-A",
                New = true,
                Time = 11910000,
                Snr = -10,
                OffsetTimeSeconds = 0.2f,
                OffsetFrequencyHz = 1598,
                Mode = "~",
                Message = "CQ K3ADU FN10",
                LowConfidence = false,
                OffAir = false
            };

            Reply message = new(decode);

            // Allocate memory and write the message
            var buffer = new byte[ReplyMessage.Length];
            var bufferMem = buffer.AsMemory();
            var written = message.WriteMessageTo(bufferMem);

            Assert.AreEqual(ReplyMessage.Length, written);
            CollectionAssert.AreEqual(ReplyMessage.ToArray(), buffer);
        }
    }
}
