﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics.CodeAnalysis;
using WsjtxUtils.WsjtxMessages.Messages;

namespace WsjtxUtils.WsjtxMessages.Tests.Messages
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class StatusTests
    {
        internal static Memory<byte> StatusMessageNotTxing = new(new byte[] {
            0xad, 0xbc, 0xcb, 0xda, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x01,
            0x00, 0x00, 0x00, 0x06, 0x57, 0x53, 0x4a, 0x54, 0x2d, 0x58, 0x00, 0x00,
            0x00, 0x00, 0x00, 0xd6, 0xc0, 0x90, 0x00, 0x00, 0x00, 0x03, 0x46, 0x54,
            0x38, 0x00, 0x00, 0x00, 0x06, 0x45, 0x41, 0x34, 0x45, 0x4a, 0x50, 0x00,
            0x00, 0x00, 0x02, 0x2d, 0x35, 0x00, 0x00, 0x00, 0x03, 0x46, 0x54, 0x38,
            0x00, 0x00, 0x01, 0x00, 0x00, 0x02, 0xe1, 0x00, 0x00, 0x02, 0xe1, 0x00,
            0x00, 0x00, 0x06, 0x4b, 0x43, 0x33, 0x50, 0x49, 0x42, 0x00, 0x00, 0x00,
            0x06, 0x45, 0x4e, 0x39, 0x30, 0x58, 0x4a, 0x00, 0x00, 0x00, 0x04, 0x49,
            0x4e, 0x38, 0x30, 0x00, 0xff, 0xff, 0xff, 0xff, 0x00, 0x00, 0xff, 0xff,
            0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0x00, 0x00, 0x00, 0x07, 0x44, 0x65,
            0x66, 0x61, 0x75, 0x6c, 0x74, 0x00, 0x00, 0x00, 0x25, 0x45, 0x41, 0x34,
            0x45, 0x4a, 0x50, 0x20, 0x4b, 0x43, 0x33, 0x50, 0x49, 0x42, 0x20, 0x2d,
            0x30, 0x35, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20,
            0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20 });

        [TestMethod()]
        public void CreateStatusMessageFrom_RawBytes_ProducesValidMessage()
        {
            var result = StatusMessageNotTxing.DeserializeWsjtxMessage();
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(Status));

            Status message = result as Status;
            Assert.AreEqual(WsjtxConstants.MagicNumber, message.MagicNumber);
            Assert.AreEqual(SchemaVersion.Version2, message.SchemaVersion);
            Assert.AreEqual(MessageType.Status, message.MessageType);
            Assert.AreEqual("WSJT-X", message.Id);
            Assert.AreEqual(14074000U, message.DialFrequencyInHz);
            Assert.AreEqual("FT8", message.Mode);
            Assert.AreEqual("EA4EJP", message.DXCall);
            Assert.AreEqual("-5", message.Report);
            Assert.AreEqual("FT8", message.TXMode);
            Assert.IsFalse(message.TXEnabled);
            Assert.IsTrue(message.Decoding);
            Assert.AreEqual(737U, message.RXOffsetFrequencyHz);
            Assert.AreEqual(737U, message.TXOffsetFrequencyHz);
            Assert.AreEqual("KC3PIB", message.DECall);
            Assert.AreEqual("EN90XJ", message.DEGrid);
            Assert.AreEqual("IN80", message.DXGrid);
            Assert.IsFalse(message.TXWatchdog);
            Assert.AreEqual(string.Empty, message.SubMode);
            Assert.IsFalse(message.FastMode);
            Assert.AreEqual(SpecialOperationMode.NONE, message.SpecialOperationMode);
            Assert.AreEqual(uint.MaxValue, message.FrequencyTolerance);
            Assert.AreEqual(uint.MaxValue, message.TRPeriod);
            Assert.AreEqual("Default", message.ConfigurationName);
        }
    }
}
