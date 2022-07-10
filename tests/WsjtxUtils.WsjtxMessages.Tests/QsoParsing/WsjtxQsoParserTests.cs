using Microsoft.VisualStudio.TestTools.UnitTesting;
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
        public void WsjtxQsoParser_ParsesCorrectly_WhenCallingCQ()
        {
            var decode = new Decode()
            {
                Id = "WSJT-X",
                Mode = "FT8",
                Message = "CQ K1ABC FN42",
                Snr = 34
            };

            var qso = WsjtxQsoParser.ParseDecode(decode);
            Assert.IsTrue(qso.IsCallingCQ);
            Assert.AreEqual(WsjtxQsoState.CallingCq, qso.QsoState);
            Assert.AreEqual("K1ABC", qso.DECallsign);
            Assert.AreEqual("FN42", qso.GridSquare);

            decode.Message = "CQ FD K1ABC FN42";
            qso = WsjtxQsoParser.ParseDecode(decode);
            Assert.IsTrue(qso.IsCallingCQ);
            Assert.AreEqual(WsjtxQsoState.CallingCq, qso.QsoState);
            Assert.AreEqual("K1ABC", qso.DECallsign);
            Assert.AreEqual("FN42", qso.GridSquare);
            Assert.AreEqual("FD", qso.CallingModifier);

            decode.Message = "CQ TEST K1ABC/R FN42";
            qso = WsjtxQsoParser.ParseDecode(decode);
            Assert.IsTrue(qso.IsCallingCQ);
            Assert.AreEqual(WsjtxQsoState.CallingCq, qso.QsoState);
            Assert.AreEqual("K1ABC/R", qso.DECallsign);
            Assert.AreEqual("FN42", qso.GridSquare);
            Assert.AreEqual("TEST", qso.CallingModifier);

            decode.Message = "CQ TEST K1ABC FN42";
            qso = WsjtxQsoParser.ParseDecode(decode);
            Assert.IsTrue(qso.IsCallingCQ);
            Assert.AreEqual(WsjtxQsoState.CallingCq, qso.QsoState);
            Assert.AreEqual("K1ABC", qso.DECallsign);
            Assert.AreEqual("FN42", qso.GridSquare);
            Assert.AreEqual("TEST", qso.CallingModifier);

            decode.Message = "CQ W9XYZ EN37";
            qso = WsjtxQsoParser.ParseDecode(decode);
            Assert.IsTrue(qso.IsCallingCQ);
            Assert.AreEqual(WsjtxQsoState.CallingCq, qso.QsoState);
            Assert.AreEqual("W9XYZ", qso.DECallsign);
            Assert.AreEqual("EN37", qso.GridSquare);

            decode.Message = "CQ G4ABC/P IO91";
            qso = WsjtxQsoParser.ParseDecode(decode);
            Assert.IsTrue(qso.IsCallingCQ);
            Assert.AreEqual(WsjtxQsoState.CallingCq, qso.QsoState);
            Assert.AreEqual("G4ABC/P", qso.DECallsign);
            Assert.AreEqual("IO91", qso.GridSquare);

            decode.Message = "CQ KH1/KH7Z";
            qso = WsjtxQsoParser.ParseDecode(decode);
            Assert.IsTrue(qso.IsCallingCQ);
            Assert.AreEqual(WsjtxQsoState.CallingCq, qso.QsoState);
            Assert.AreEqual("KH1/KH7Z", qso.DECallsign);
            Assert.AreEqual(string.Empty, qso.GridSquare);

            decode.Message = "CQ PJ4/K1ABC";
            qso = WsjtxQsoParser.ParseDecode(decode);
            Assert.IsTrue(qso.IsCallingCQ);
            Assert.AreEqual(WsjtxQsoState.CallingCq, qso.QsoState);
            Assert.AreEqual("PJ4/K1ABC", qso.DECallsign);
            Assert.AreEqual(string.Empty, qso.GridSquare);

            decode.Message = "CQ YW18FIFA";
            qso = WsjtxQsoParser.ParseDecode(decode);
            Assert.IsTrue(qso.IsCallingCQ);
            Assert.AreEqual(WsjtxQsoState.CallingCq, qso.QsoState);
            Assert.AreEqual("YW18FIFA", qso.DECallsign);
            Assert.AreEqual(string.Empty, qso.GridSquare);
        }
    }
}
