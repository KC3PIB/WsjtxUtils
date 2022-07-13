using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics.CodeAnalysis;
using WsjtxUtils.WsjtxMessages.Messages;
using WsjtxUtils.WsjtxMessages.QsoParsing;

namespace WsjtxUtils.WsjtxMessages.Tests.QsoParsing
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class WsjtxQsoParserTests
    {
        [TestMethod()]
        public void WsjtxQsoParser_ParsesCorrectly_With77BitMode()
        {
            var decode = new Decode()
            {
                Id = "WSJT-X",
                Mode = "~",
                Message = "CQ K1ABC FN42",
            };

            var qso = WsjtxQsoParser.ParseDecode(decode);
            Assert.IsTrue(qso.IsCallingCQ);
            Assert.AreEqual(WsjtxQsoState.CallingCq, qso.QsoState);
            Assert.AreEqual("K1ABC", qso.DECallsign);
            Assert.AreEqual("FN42", qso.GridSquare);

            decode = new Decode()
            {
                Id = "WSJT-X",
                Mode = "`",
                Message = "CQ K1ABC FN42",
            };

            qso = WsjtxQsoParser.ParseDecode(decode);
            Assert.IsTrue(qso.IsCallingCQ);
            Assert.AreEqual(WsjtxQsoState.CallingCq, qso.QsoState);
            Assert.AreEqual("K1ABC", qso.DECallsign);
            Assert.AreEqual("FN42", qso.GridSquare);

            decode = new Decode()
            {
                Id = "WSJT-X",
                Mode = "+",
                Message = "CQ K1ABC FN42",
            };

            qso = WsjtxQsoParser.ParseDecode(decode);
            Assert.IsTrue(qso.IsCallingCQ);
            Assert.AreEqual(WsjtxQsoState.CallingCq, qso.QsoState);
            Assert.AreEqual("K1ABC", qso.DECallsign);
            Assert.AreEqual("FN42", qso.GridSquare);

            decode = new Decode()
            {
                Id = "WSJT-X",
                Mode = "&",
                Message = "CQ K1ABC FN42",
            };

            qso = WsjtxQsoParser.ParseDecode(decode);
            Assert.IsTrue(qso.IsCallingCQ);
            Assert.AreEqual(WsjtxQsoState.CallingCq, qso.QsoState);
            Assert.AreEqual("K1ABC", qso.DECallsign);
            Assert.AreEqual("FN42", qso.GridSquare);

            decode = new Decode()
            {
                Id = "WSJT-X",
                Mode = ":",
                Message = "CQ K1ABC FN42",
            };

            qso = WsjtxQsoParser.ParseDecode(decode);
            Assert.IsTrue(qso.IsCallingCQ);
            Assert.AreEqual(WsjtxQsoState.CallingCq, qso.QsoState);
            Assert.AreEqual("K1ABC", qso.DECallsign);
            Assert.AreEqual("FN42", qso.GridSquare);
        }

        [TestMethod()]
        [ExpectedException(typeof(NotImplementedException))]
        public void WsjtxQsoParser_ThrowsNotImplementedException_WithUnknownMode()
        {
            var decode = new Decode()
            {
                Id = "WSJT-X",
                Mode = " ",
                Message = "CQ K1ABC FN42",
            };

            var qso = WsjtxQsoParser.ParseDecode(decode);
        }
    }
}
