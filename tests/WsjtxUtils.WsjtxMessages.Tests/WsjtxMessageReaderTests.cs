using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics.CodeAnalysis;
using WsjtxUtils.WsjtxMessages.Tests.Messages;

namespace WsjtxUtils.WsjtxMessages.Tests
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class WsjtxMessageReaderTests
    {
        private enum TestEnum : long
        {
            Test = 0
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentException))]
        public void WsjtxMessageReader_ThrowsException_WhenSourceBytesTooSmall()
        {
            var buffer = new Memory<byte>(new byte[4]);
            _ = new WsjtxMessageReader(buffer);
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentException))]
        public void WsjtxMessageReader_ThrowsException_WhenSourceDoesNotHaveMagicValue()
        {
            var buffer = new Memory<byte>(new byte[WsjtxConstants.HeaderLengthInBytes]);
            _ = new WsjtxMessageReader(buffer);
        }

        [TestMethod()]
        [ExpectedException(typeof(NotImplementedException))]
        public void WsjtxMessageReader_ThrowsException_WhenEnumValueNotImplemented()
        {
            var reader = new WsjtxMessageReader(HeartbeatTests.HeartbeatMessage);
            reader.ReadEnum<TestEnum>();
        }
    }
}
