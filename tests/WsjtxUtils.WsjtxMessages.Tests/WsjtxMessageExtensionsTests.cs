using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics.CodeAnalysis;
using WsjtxUtils.WsjtxMessages.Messages;
using WsjtxUtils.WsjtxMessages.Tests.Messages;

namespace WsjtxUtils.WsjtxMessages.Tests
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class WsjtxMessageExtensionsTests
    {
        [TestMethod()]
        [ExpectedException(typeof(ArgumentException))]
        public void CreateDefaultMessage_ThrowsException_WhenInvalidMessageType()
        {
            WsjtxMessageExtensions.CreateDefaultMessage((MessageType)248);
        }

        [TestMethod()]
        public void DeserializeWsjtxMessage_WithRawBytes_ProducesValidMessage()
        {
            var message = WsjtxMessageExtensions.DeserializeWsjtxMessage(StatusTests.StatusMessageNotTxing);
            Assert.IsInstanceOfType(message, typeof(Status));
            Assert.AreEqual(message.MagicNumber, 0xadbccbda);
            Assert.AreEqual(message.SchemaVersion, SchemaVersion.Version2);
            Assert.AreEqual(message.MessageType, MessageType.Status);
            Assert.AreEqual(message.Id, "WSJT-X");
        }
    }
}
