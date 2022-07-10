using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics.CodeAnalysis;

namespace WsjtxUtils.WsjtxMessages.Tests
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class WsjtxMessageWriterTests
    {
        [TestMethod()]
        [ExpectedException(typeof(ArgumentException))]
        public void WsjtxMessageWriter_ThrowsException_WhenSourceBytesTooSmall()
        {
            var buffer = new Memory<byte>(new byte[4]);
            _ = new WsjtxMessageWriter(buffer);
        }
    }
}
