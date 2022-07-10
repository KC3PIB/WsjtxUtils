using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics.CodeAnalysis;
using WsjtxUtils.WsjtxMessages.Messages;
using WsjtxUtils.WsjtxMessages.QsoParsing;

namespace WsjtxUtils.WsjtxMessages.Tests.QsoParsing
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class WsjtxQsoParser77BitModesTests
    {
        [TestMethod()]
        public void WsjtxQsoParser_Parses77BitModeCorrectly_WhenNAVHFContest()
        {
            var decode = new Decode()
            {
                Id = "WSJT-X",
                Mode = "FT8",
                Message = "CQ TEST K1ABC FN42",
            };

            var qso = WsjtxQsoParser.ParseDecode(decode);
            Assert.AreEqual(decode.Mode, qso.Mode);
            Assert.IsTrue(qso.IsCallingCQ);
            Assert.AreEqual(WsjtxQsoState.CallingCq, qso.QsoState);
            Assert.AreEqual(string.Empty, qso.DXCallsign);
            Assert.AreEqual("K1ABC", qso.DECallsign);
            Assert.AreEqual("FN42", qso.GridSquare);

            decode.Message = "K1ABC W9XYZ EN37";
            qso = WsjtxQsoParser.ParseDecode(decode);
            Assert.IsFalse(qso.IsCallingCQ);
            Assert.AreEqual(WsjtxQsoState.CallingStation, qso.QsoState);
            Assert.AreEqual("K1ABC", qso.DXCallsign);
            Assert.AreEqual("W9XYZ", qso.DECallsign);
            Assert.AreEqual("EN37", qso.GridSquare);

            decode.Message = "W9XYZ K1ABC R FN42";
            qso = WsjtxQsoParser.ParseDecode(decode);
            Assert.IsFalse(qso.IsCallingCQ);
            Assert.AreEqual(WsjtxQsoState.RogerReport, qso.QsoState);
            Assert.AreEqual("W9XYZ", qso.DXCallsign);
            Assert.AreEqual("K1ABC", qso.DECallsign);
            Assert.AreEqual("FN42", qso.GridSquare);

            decode.Message = "K1ABC W9XYZ RRR";
            qso = WsjtxQsoParser.ParseDecode(decode);
            Assert.IsFalse(qso.IsCallingCQ);
            Assert.AreEqual(WsjtxQsoState.Rogers, qso.QsoState);
            Assert.AreEqual("K1ABC", qso.DXCallsign);
            Assert.AreEqual("W9XYZ", qso.DECallsign);
            Assert.AreEqual(string.Empty, qso.GridSquare);

            decode.Message = "W9XYZ K1ABC 73";
            qso = WsjtxQsoParser.ParseDecode(decode);
            Assert.IsFalse(qso.IsCallingCQ);
            Assert.AreEqual(WsjtxQsoState.Signoff, qso.QsoState);
            Assert.AreEqual("W9XYZ", qso.DXCallsign);
            Assert.AreEqual("K1ABC", qso.DECallsign);
            Assert.AreEqual(string.Empty, qso.GridSquare);
        }

        [TestMethod()]
        public void WsjtxQsoParser_Parses77BitModeCorrectly_WhenEUVHFContest()
        {
            var decode = new Decode()
            {
                Id = "WSJT-X",
                Mode = "FT8",
                Message = "CQ TEST G4ABC IO91"
            };

            var qso = WsjtxQsoParser.ParseDecode(decode);
            Assert.AreEqual(decode.Mode, qso.Mode);
            Assert.IsTrue(qso.IsCallingCQ);
            Assert.AreEqual(WsjtxQsoState.CallingCq, qso.QsoState);
            Assert.AreEqual("TEST", qso.CallingModifier);
            Assert.AreEqual(string.Empty, qso.DXCallsign);
            Assert.AreEqual("G4ABC", qso.DECallsign);
            Assert.AreEqual("IO91", qso.GridSquare);

            decode.Message = "G4ABC PA9XYZ JO22";
            qso = WsjtxQsoParser.ParseDecode(decode);
            Assert.IsFalse(qso.IsCallingCQ);
            Assert.AreEqual(WsjtxQsoState.CallingStation, qso.QsoState);
            Assert.AreEqual("G4ABC", qso.DXCallsign);
            Assert.AreEqual("PA9XYZ", qso.DECallsign);
            Assert.AreEqual("JO22", qso.GridSquare);

            decode.Message = "<PA9XYZ> <G4ABC> 570123 IO91NP";
            qso = WsjtxQsoParser.ParseDecode(decode);
            Assert.IsFalse(qso.IsCallingCQ);
            Assert.AreEqual(WsjtxQsoState.Report, qso.QsoState);
            Assert.AreEqual("PA9XYZ", qso.DXCallsign);
            Assert.AreEqual("G4ABC", qso.DECallsign);
            Assert.AreEqual("570123 IO91NP", qso.Report);
            Assert.AreEqual("IO91NP", qso.GridSquare);

            decode.Message = "<G4ABC> <PA9XYZ> R 580071 JO22DB";
            qso = WsjtxQsoParser.ParseDecode(decode);
            Assert.IsFalse(qso.IsCallingCQ);
            Assert.AreEqual(WsjtxQsoState.RogerReport, qso.QsoState);
            Assert.AreEqual("G4ABC", qso.DXCallsign);
            Assert.AreEqual("PA9XYZ", qso.DECallsign);
            Assert.AreEqual("R 580071 JO22DB", qso.Report);
            Assert.AreEqual("JO22DB", qso.GridSquare);

            decode.Message = "PA9XYZ G4ABC RR73";
            qso = WsjtxQsoParser.ParseDecode(decode);
            Assert.IsFalse(qso.IsCallingCQ);
            Assert.AreEqual(WsjtxQsoState.Rogers, qso.QsoState);
            Assert.AreEqual("PA9XYZ", qso.DXCallsign);
            Assert.AreEqual("G4ABC", qso.DECallsign);
            Assert.AreEqual(string.Empty, qso.GridSquare);
        }

        [TestMethod()]
        public void WsjtxQsoParser_Parses77BitModeCorrectly_WhenWWDigiContest()
        {
            var decode = new Decode()
            {
                Id = "WSJT-X",
                Mode = "FT8",
                Message = "CQ WW K1ABC FN42"
            };

            var qso = WsjtxQsoParser.ParseDecode(decode);
            Assert.AreEqual(decode.Mode, qso.Mode);
            Assert.IsTrue(qso.IsCallingCQ);
            Assert.AreEqual(WsjtxQsoState.CallingCq, qso.QsoState);
            Assert.AreEqual("WW", qso.CallingModifier);
            Assert.AreEqual(string.Empty, qso.DXCallsign);
            Assert.AreEqual("K1ABC", qso.DECallsign);
            Assert.AreEqual("FN42", qso.GridSquare);

            decode.Message = "K1ABC S52XYZ JN76";
            qso = WsjtxQsoParser.ParseDecode(decode);
            Assert.IsFalse(qso.IsCallingCQ);
            Assert.AreEqual(WsjtxQsoState.CallingStation, qso.QsoState);
            Assert.AreEqual("K1ABC", qso.DXCallsign);
            Assert.AreEqual("S52XYZ", qso.DECallsign);
            Assert.AreEqual("JN76", qso.GridSquare);

            decode.Message = "S52XYZ K1ABC R FN42";
            qso = WsjtxQsoParser.ParseDecode(decode);
            Assert.IsFalse(qso.IsCallingCQ);
            Assert.AreEqual(WsjtxQsoState.RogerReport, qso.QsoState);
            Assert.AreEqual("S52XYZ", qso.DXCallsign);
            Assert.AreEqual("K1ABC", qso.DECallsign);
            Assert.AreEqual("FN42", qso.GridSquare);

            decode.Message = "K1ABC S52XYZ RR73";
            qso = WsjtxQsoParser.ParseDecode(decode);
            Assert.IsFalse(qso.IsCallingCQ);
            Assert.AreEqual(WsjtxQsoState.Rogers, qso.QsoState);
            Assert.AreEqual("K1ABC", qso.DXCallsign);
            Assert.AreEqual("S52XYZ", qso.DECallsign);
            Assert.AreEqual(string.Empty, qso.GridSquare);
        }

        [TestMethod()]
        public void WsjtxQsoParser_Parses77BitModeCorrectly_WhenARRLFieldDay()
        {
            var decode = new Decode()
            {
                Id = "WSJT-X",
                Mode = "FT8",
                Message = "CQ FD K1ABC FN42"
            };

            var qso = WsjtxQsoParser.ParseDecode(decode);
            Assert.AreEqual(decode.Mode, qso.Mode);
            Assert.AreEqual(WsjtxQsoState.CallingCq, qso.QsoState);
            Assert.IsTrue(qso.IsCallingCQ);
            Assert.AreEqual("FD", qso.CallingModifier);
            Assert.AreEqual(string.Empty, qso.DXCallsign);
            Assert.AreEqual("K1ABC", qso.DECallsign);
            Assert.AreEqual("FN42", qso.GridSquare);

            decode.Message = "K1ABC W9XYZ 6A WI";
            qso = WsjtxQsoParser.ParseDecode(decode);
            Assert.IsFalse(qso.IsCallingCQ);
            Assert.AreEqual(WsjtxQsoState.Report, qso.QsoState);
            Assert.AreEqual("K1ABC", qso.DXCallsign);
            Assert.AreEqual("W9XYZ", qso.DECallsign);
            Assert.AreEqual("6A WI", qso.Report);
            Assert.AreEqual(string.Empty, qso.GridSquare);

            decode.Message = "W9XYZ K1ABC R 2B EMA";
            qso = WsjtxQsoParser.ParseDecode(decode);
            Assert.IsFalse(qso.IsCallingCQ);
            Assert.AreEqual(WsjtxQsoState.RogerReport, qso.QsoState);
            Assert.AreEqual("W9XYZ", qso.DXCallsign);
            Assert.AreEqual("K1ABC", qso.DECallsign);
            Assert.AreEqual(string.Empty, qso.GridSquare);

            decode.Message = "K1ABC W9XYZ RR73";
            qso = WsjtxQsoParser.ParseDecode(decode);
            Assert.IsFalse(qso.IsCallingCQ);
            Assert.AreEqual(WsjtxQsoState.Rogers, qso.QsoState);
            Assert.AreEqual("K1ABC", qso.DXCallsign);
            Assert.AreEqual("W9XYZ", qso.DECallsign);
            Assert.AreEqual(string.Empty, qso.GridSquare);
        }

        [TestMethod()]
        public void WsjtxQsoParser_Parses77BitModeCorrectly_WhenNonstandardCallsign()
        {
            var decode = new Decode()
            {
                Id = "WSJT-X",
                Mode = "FT8",
                Message = "CQ PJ4/K1ABC"
            };

            var qso = WsjtxQsoParser.ParseDecode(decode);
            Assert.AreEqual(decode.Mode, qso.Mode);
            Assert.AreEqual(WsjtxQsoState.CallingCq, qso.QsoState);
            Assert.IsTrue(qso.IsCallingCQ);
            Assert.AreEqual(string.Empty, qso.CallingModifier);
            Assert.AreEqual(string.Empty, qso.DXCallsign);
            Assert.AreEqual("PJ4/K1ABC", qso.DECallsign);
            Assert.AreEqual(string.Empty, qso.GridSquare);

            decode.Message = "<PJ4/K1ABC> W9XYZ";
            qso = WsjtxQsoParser.ParseDecode(decode);
            Assert.IsFalse(qso.IsCallingCQ);
            Assert.AreEqual(WsjtxQsoState.CallingStation, qso.QsoState);
            Assert.AreEqual("PJ4/K1ABC", qso.DXCallsign);
            Assert.AreEqual("W9XYZ", qso.DECallsign);
            Assert.AreEqual(string.Empty, qso.Report);
            Assert.AreEqual(string.Empty, qso.GridSquare);

            decode.Message = "W9XYZ <PJ4/K1ABC> +03";
            qso = WsjtxQsoParser.ParseDecode(decode);
            Assert.IsFalse(qso.IsCallingCQ);
            Assert.AreEqual(WsjtxQsoState.Report, qso.QsoState);
            Assert.AreEqual("W9XYZ", qso.DXCallsign);
            Assert.AreEqual("PJ4/K1ABC", qso.DECallsign);
            Assert.AreEqual("+03", qso.Report);
            Assert.AreEqual(string.Empty, qso.GridSquare);

            decode.Message = "<PJ4/K1ABC> W9XYZ R-08";
            qso = WsjtxQsoParser.ParseDecode(decode);
            Assert.IsFalse(qso.IsCallingCQ);
            Assert.AreEqual(WsjtxQsoState.RogerReport, qso.QsoState);
            Assert.AreEqual("PJ4/K1ABC", qso.DXCallsign);
            Assert.AreEqual("W9XYZ", qso.DECallsign);
            Assert.AreEqual("R-08", qso.Report);
            Assert.AreEqual(string.Empty, qso.GridSquare);

            decode.Message = "<W9XYZ> PJ4/K1ABC RRR";
            qso = WsjtxQsoParser.ParseDecode(decode);
            Assert.IsFalse(qso.IsCallingCQ);
            Assert.AreEqual(WsjtxQsoState.Rogers, qso.QsoState);
            Assert.AreEqual("W9XYZ", qso.DXCallsign);
            Assert.AreEqual("PJ4/K1ABC", qso.DECallsign);
            Assert.AreEqual("RRR", qso.Report);
            Assert.AreEqual(string.Empty, qso.GridSquare);

            decode.Message = "PJ4/K1ABC <W9XYZ> 73";
            qso = WsjtxQsoParser.ParseDecode(decode);
            Assert.IsFalse(qso.IsCallingCQ);
            Assert.AreEqual(WsjtxQsoState.Signoff, qso.QsoState);
            Assert.AreEqual("PJ4/K1ABC", qso.DXCallsign);
            Assert.AreEqual("W9XYZ", qso.DECallsign);
            Assert.AreEqual("73", qso.Report);
            Assert.AreEqual(string.Empty, qso.GridSquare);
        }

        [TestMethod()]
        public void WsjtxQsoParser_Parses77BitModeCorrectly_WhenUsingFTCodeOutput()
        {
            #region Free Text
            var decode = new Decode()
            {
                Id = "WSJT-X",
                Mode = "FT8",
                Message = "TNX BOB 73 GL"
            };

            var qso = WsjtxQsoParser.ParseDecode(decode);
            Assert.AreEqual(decode.Mode, qso.Mode);
            Assert.AreEqual(WsjtxQsoState.Unknown, qso.QsoState);
            Assert.IsFalse(qso.IsCallingCQ);
            Assert.AreEqual(string.Empty, qso.CallingModifier);
            Assert.AreEqual(string.Empty, qso.DXCallsign);
            Assert.AreEqual(string.Empty, qso.DECallsign);
            Assert.AreEqual(string.Empty, qso.GridSquare);
            Assert.AreEqual("TNX BOB 73 GL", qso.Report);

            decode.Message = "PA9XYZ 590003";
            qso = WsjtxQsoParser.ParseDecode(decode);
            Assert.IsFalse(qso.IsCallingCQ);
            Assert.AreEqual(WsjtxQsoState.Unknown, qso.QsoState);
            Assert.AreEqual(string.Empty, qso.DXCallsign);
            Assert.AreEqual(string.Empty, qso.DECallsign);
            Assert.AreEqual("PA9XYZ 590003", qso.Report);
            Assert.AreEqual(string.Empty, qso.GridSquare);

            decode.Message = "G4ABC/P R 570";
            qso = WsjtxQsoParser.ParseDecode(decode);
            Assert.IsFalse(qso.IsCallingCQ);
            Assert.AreEqual(WsjtxQsoState.Unknown, qso.QsoState);
            Assert.AreEqual(string.Empty, qso.DXCallsign);
            Assert.AreEqual(string.Empty, qso.DECallsign);
            Assert.AreEqual("G4ABC/P R 570", qso.Report);
            Assert.AreEqual(string.Empty, qso.GridSquare);
            #endregion

            // DXpedition mode
            decode.Message = "K1ABC RR73; W9XYZ <KH1/KH7Z> -08";
            qso = WsjtxQsoParser.ParseDecode(decode);
            Assert.IsFalse(qso.IsCallingCQ);
            Assert.AreEqual(WsjtxQsoState.Report, qso.QsoState);
            Assert.AreEqual("W9XYZ", qso.DXCallsign);
            Assert.AreEqual("KH1/KH7Z", qso.DECallsign);
            Assert.AreEqual("-08", qso.Report);
            Assert.AreEqual(string.Empty, qso.GridSquare);

            //ARRL Field Day
            decode.Message = "W9XYZ K1ABC R 17B EMA";
            qso = WsjtxQsoParser.ParseDecode(decode);
            Assert.IsFalse(qso.IsCallingCQ);
            Assert.AreEqual(WsjtxQsoState.RogerReport, qso.QsoState);
            Assert.AreEqual("W9XYZ", qso.DXCallsign);
            Assert.AreEqual("K1ABC", qso.DECallsign);
            Assert.AreEqual("R 17B EMA", qso.Report);
            Assert.AreEqual(string.Empty, qso.GridSquare);

            //Telemetry
            decode.Message = "123456789ABCDEF012";
            qso = WsjtxQsoParser.ParseDecode(decode);
            Assert.IsFalse(qso.IsCallingCQ);
            Assert.AreEqual(WsjtxQsoState.Unknown, qso.QsoState);
            Assert.AreEqual(string.Empty, qso.DXCallsign);
            Assert.AreEqual(string.Empty, qso.DECallsign);
            Assert.AreEqual("123456789ABCDEF012", qso.Report);
            Assert.AreEqual(string.Empty, qso.GridSquare);

            #region Standard msg
            decode.Message = "CQ K1ABC FN42";
            qso = WsjtxQsoParser.ParseDecode(decode);
            Assert.IsTrue(qso.IsCallingCQ);
            Assert.AreEqual(WsjtxQsoState.CallingCq, qso.QsoState);
            Assert.AreEqual(string.Empty, qso.CallingModifier);
            Assert.AreEqual(string.Empty, qso.DXCallsign);
            Assert.AreEqual("K1ABC", qso.DECallsign);
            Assert.AreEqual(string.Empty, qso.Report);
            Assert.AreEqual("FN42", qso.GridSquare);

            decode.Message = "K1ABC W9XYZ EN37";
            qso = WsjtxQsoParser.ParseDecode(decode);
            Assert.IsFalse(qso.IsCallingCQ);
            Assert.AreEqual(WsjtxQsoState.CallingStation, qso.QsoState);
            Assert.AreEqual(string.Empty, qso.CallingModifier);
            Assert.AreEqual("K1ABC", qso.DXCallsign);
            Assert.AreEqual("W9XYZ", qso.DECallsign);
            Assert.AreEqual(string.Empty, qso.Report);
            Assert.AreEqual("EN37", qso.GridSquare);

            decode.Message = "W9XYZ K1ABC -11";
            qso = WsjtxQsoParser.ParseDecode(decode);
            Assert.IsFalse(qso.IsCallingCQ);
            Assert.AreEqual(WsjtxQsoState.Report, qso.QsoState);
            Assert.AreEqual(string.Empty, qso.CallingModifier);
            Assert.AreEqual("W9XYZ", qso.DXCallsign);
            Assert.AreEqual("K1ABC", qso.DECallsign);
            Assert.AreEqual("-11", qso.Report);
            Assert.AreEqual(string.Empty, qso.GridSquare);

            decode.Message = "K1ABC W9XYZ R-09";
            qso = WsjtxQsoParser.ParseDecode(decode);
            Assert.IsFalse(qso.IsCallingCQ);
            Assert.AreEqual(WsjtxQsoState.RogerReport, qso.QsoState);
            Assert.AreEqual(string.Empty, qso.CallingModifier);
            Assert.AreEqual("K1ABC", qso.DXCallsign);
            Assert.AreEqual("W9XYZ", qso.DECallsign);
            Assert.AreEqual("R-09", qso.Report);
            Assert.AreEqual(string.Empty, qso.GridSquare);

            decode.Message = "W9XYZ K1ABC RRR";
            qso = WsjtxQsoParser.ParseDecode(decode);
            Assert.IsFalse(qso.IsCallingCQ);
            Assert.AreEqual(WsjtxQsoState.Rogers, qso.QsoState);
            Assert.AreEqual(string.Empty, qso.CallingModifier);
            Assert.AreEqual("W9XYZ", qso.DXCallsign);
            Assert.AreEqual("K1ABC", qso.DECallsign);
            Assert.AreEqual("RRR", qso.Report);
            Assert.AreEqual(string.Empty, qso.GridSquare);

            decode.Message = "K1ABC W9XYZ 73";
            qso = WsjtxQsoParser.ParseDecode(decode);
            Assert.IsFalse(qso.IsCallingCQ);
            Assert.AreEqual(WsjtxQsoState.Signoff, qso.QsoState);
            Assert.AreEqual(string.Empty, qso.CallingModifier);
            Assert.AreEqual("K1ABC", qso.DXCallsign);
            Assert.AreEqual("W9XYZ", qso.DECallsign);
            Assert.AreEqual("73", qso.Report);
            Assert.AreEqual(string.Empty, qso.GridSquare);

            decode.Message = "K1ABC W9XYZ RR73";
            qso = WsjtxQsoParser.ParseDecode(decode);
            Assert.IsFalse(qso.IsCallingCQ);
            Assert.AreEqual(WsjtxQsoState.Rogers, qso.QsoState);
            Assert.AreEqual(string.Empty, qso.CallingModifier);
            Assert.AreEqual("K1ABC", qso.DXCallsign);
            Assert.AreEqual("W9XYZ", qso.DECallsign);
            Assert.AreEqual("RR73", qso.Report);
            Assert.AreEqual(string.Empty, qso.GridSquare);

            decode.Message = "CQ FD K1ABC FN42";
            qso = WsjtxQsoParser.ParseDecode(decode);
            Assert.IsTrue(qso.IsCallingCQ);
            Assert.AreEqual(WsjtxQsoState.CallingCq, qso.QsoState);
            Assert.AreEqual("FD", qso.CallingModifier);
            Assert.AreEqual(string.Empty, qso.DXCallsign);
            Assert.AreEqual("K1ABC", qso.DECallsign);
            Assert.AreEqual(string.Empty, qso.Report);
            Assert.AreEqual("FN42", qso.GridSquare);

            decode.Message = "CQ TEST K1ABC/R FN42";
            qso = WsjtxQsoParser.ParseDecode(decode);
            Assert.IsTrue(qso.IsCallingCQ);
            Assert.AreEqual(WsjtxQsoState.CallingCq, qso.QsoState);
            Assert.AreEqual("TEST", qso.CallingModifier);
            Assert.AreEqual(string.Empty, qso.DXCallsign);
            Assert.AreEqual("K1ABC/R", qso.DECallsign);
            Assert.AreEqual(string.Empty, qso.Report);
            Assert.AreEqual("FN42", qso.GridSquare);

            decode.Message = "K1ABC/R W9XYZ EN37";
            qso = WsjtxQsoParser.ParseDecode(decode);
            Assert.IsFalse(qso.IsCallingCQ);
            Assert.AreEqual(WsjtxQsoState.CallingStation, qso.QsoState);
            Assert.AreEqual(string.Empty, qso.CallingModifier);
            Assert.AreEqual("K1ABC/R", qso.DXCallsign);
            Assert.AreEqual("W9XYZ", qso.DECallsign);
            Assert.AreEqual(string.Empty, qso.Report);
            Assert.AreEqual("EN37", qso.GridSquare);

            decode.Message = "W9XYZ K1ABC/R R FN42";
            qso = WsjtxQsoParser.ParseDecode(decode);
            Assert.IsFalse(qso.IsCallingCQ);
            Assert.AreEqual(WsjtxQsoState.RogerReport, qso.QsoState);
            Assert.AreEqual(string.Empty, qso.CallingModifier);
            Assert.AreEqual("W9XYZ", qso.DXCallsign);
            Assert.AreEqual("K1ABC/R", qso.DECallsign);
            Assert.AreEqual("R FN42", qso.Report);
            Assert.AreEqual("FN42", qso.GridSquare);

            decode.Message = "K1ABC/R W9XYZ RR73";
            qso = WsjtxQsoParser.ParseDecode(decode);
            Assert.IsFalse(qso.IsCallingCQ);
            Assert.AreEqual(WsjtxQsoState.Rogers, qso.QsoState);
            Assert.AreEqual(string.Empty, qso.CallingModifier);
            Assert.AreEqual("K1ABC/R", qso.DXCallsign);
            Assert.AreEqual("W9XYZ", qso.DECallsign);
            Assert.AreEqual("RR73", qso.Report);
            Assert.AreEqual(string.Empty, qso.GridSquare);

            decode.Message = "CQ TEST K1ABC FN42";
            qso = WsjtxQsoParser.ParseDecode(decode);
            Assert.IsTrue(qso.IsCallingCQ);
            Assert.AreEqual(WsjtxQsoState.CallingCq, qso.QsoState);
            Assert.AreEqual("TEST", qso.CallingModifier);
            Assert.AreEqual(string.Empty, qso.DXCallsign);
            Assert.AreEqual("K1ABC", qso.DECallsign);
            Assert.AreEqual(string.Empty, qso.Report);
            Assert.AreEqual("FN42", qso.GridSquare);

            decode.Message = "W9XYZ <PJ4/K1ABC> -11";
            qso = WsjtxQsoParser.ParseDecode(decode);
            Assert.IsFalse(qso.IsCallingCQ);
            Assert.AreEqual(WsjtxQsoState.Report, qso.QsoState);
            Assert.AreEqual(string.Empty, qso.CallingModifier);
            Assert.AreEqual("W9XYZ", qso.DXCallsign);
            Assert.AreEqual("PJ4/K1ABC", qso.DECallsign);
            Assert.AreEqual("-11", qso.Report);
            Assert.AreEqual(string.Empty, qso.GridSquare);

            decode.Message = "<PJ4/K1ABC> W9XYZ R-09";
            qso = WsjtxQsoParser.ParseDecode(decode);
            Assert.IsFalse(qso.IsCallingCQ);
            Assert.AreEqual(WsjtxQsoState.RogerReport, qso.QsoState);
            Assert.AreEqual(string.Empty, qso.CallingModifier);
            Assert.AreEqual("PJ4/K1ABC", qso.DXCallsign);
            Assert.AreEqual("W9XYZ", qso.DECallsign);
            Assert.AreEqual("R-09", qso.Report);
            Assert.AreEqual(string.Empty, qso.GridSquare);

            decode.Message = "CQ W9XYZ EN37";
            qso = WsjtxQsoParser.ParseDecode(decode);
            Assert.IsTrue(qso.IsCallingCQ);
            Assert.AreEqual(WsjtxQsoState.CallingCq, qso.QsoState);
            Assert.AreEqual(string.Empty, qso.CallingModifier);
            Assert.AreEqual(string.Empty, qso.DXCallsign);
            Assert.AreEqual("W9XYZ", qso.DECallsign);
            Assert.AreEqual(string.Empty, qso.Report);
            Assert.AreEqual("EN37", qso.GridSquare);

            decode.Message = "<YW18FIFA> W9XYZ -11";
            qso = WsjtxQsoParser.ParseDecode(decode);
            Assert.IsFalse(qso.IsCallingCQ);
            Assert.AreEqual(WsjtxQsoState.Report, qso.QsoState);
            Assert.AreEqual(string.Empty, qso.CallingModifier);
            Assert.AreEqual("YW18FIFA", qso.DXCallsign);
            Assert.AreEqual("W9XYZ", qso.DECallsign);
            Assert.AreEqual("-11", qso.Report);
            Assert.AreEqual(string.Empty, qso.GridSquare);

            decode.Message = "W9XYZ <YW18FIFA> R-09";
            qso = WsjtxQsoParser.ParseDecode(decode);
            Assert.IsFalse(qso.IsCallingCQ);
            Assert.AreEqual(WsjtxQsoState.RogerReport, qso.QsoState);
            Assert.AreEqual(string.Empty, qso.CallingModifier);
            Assert.AreEqual("W9XYZ", qso.DXCallsign);
            Assert.AreEqual("YW18FIFA", qso.DECallsign);
            Assert.AreEqual("R-09", qso.Report);
            Assert.AreEqual(string.Empty, qso.GridSquare);

            decode.Message = "<YW18FIFA> KA1ABC";
            qso = WsjtxQsoParser.ParseDecode(decode);
            Assert.IsFalse(qso.IsCallingCQ);
            Assert.AreEqual(WsjtxQsoState.CallingStation, qso.QsoState);
            Assert.AreEqual(string.Empty, qso.CallingModifier);
            Assert.AreEqual("YW18FIFA", qso.DXCallsign);
            Assert.AreEqual("KA1ABC", qso.DECallsign);
            Assert.AreEqual(string.Empty, qso.Report);
            Assert.AreEqual(string.Empty, qso.GridSquare);

            decode.Message = "KA1ABC <YW18FIFA> -11";
            qso = WsjtxQsoParser.ParseDecode(decode);
            Assert.IsFalse(qso.IsCallingCQ);
            Assert.AreEqual(WsjtxQsoState.Report, qso.QsoState);
            Assert.AreEqual(string.Empty, qso.CallingModifier);
            Assert.AreEqual("KA1ABC", qso.DXCallsign);
            Assert.AreEqual("YW18FIFA", qso.DECallsign);
            Assert.AreEqual("-11", qso.Report);
            Assert.AreEqual(string.Empty, qso.GridSquare);

            decode.Message = "<YW18FIFA> KA1ABC R-17";
            qso = WsjtxQsoParser.ParseDecode(decode);
            Assert.IsFalse(qso.IsCallingCQ);
            Assert.AreEqual(WsjtxQsoState.RogerReport, qso.QsoState);
            Assert.AreEqual(string.Empty, qso.CallingModifier);
            Assert.AreEqual("YW18FIFA", qso.DXCallsign);
            Assert.AreEqual("KA1ABC", qso.DECallsign);
            Assert.AreEqual("R-17", qso.Report);
            Assert.AreEqual(string.Empty, qso.GridSquare);

            decode.Message = "<YW18FIFA> KA1ABC 73";
            qso = WsjtxQsoParser.ParseDecode(decode);
            Assert.IsFalse(qso.IsCallingCQ);
            Assert.AreEqual(WsjtxQsoState.Signoff, qso.QsoState);
            Assert.AreEqual(string.Empty, qso.CallingModifier);
            Assert.AreEqual("YW18FIFA", qso.DXCallsign);
            Assert.AreEqual("KA1ABC", qso.DECallsign);
            Assert.AreEqual("73", qso.Report);
            Assert.AreEqual(string.Empty, qso.GridSquare);
            #endregion

            #region EU VHF Contest
            decode.Message = "CQ G4ABC/P IO91";
            qso = WsjtxQsoParser.ParseDecode(decode);
            Assert.IsTrue(qso.IsCallingCQ);
            Assert.AreEqual(WsjtxQsoState.CallingCq, qso.QsoState);
            Assert.AreEqual(string.Empty, qso.CallingModifier);
            Assert.AreEqual(string.Empty, qso.DXCallsign);
            Assert.AreEqual("G4ABC/P", qso.DECallsign);
            Assert.AreEqual(string.Empty, qso.Report);
            Assert.AreEqual("IO91", qso.GridSquare);

            decode.Message = "G4ABC/P PA9XYZ JO22";
            qso = WsjtxQsoParser.ParseDecode(decode);
            Assert.IsFalse(qso.IsCallingCQ);
            Assert.AreEqual(WsjtxQsoState.CallingStation, qso.QsoState);
            Assert.AreEqual(string.Empty, qso.CallingModifier);
            Assert.AreEqual("G4ABC/P", qso.DXCallsign);
            Assert.AreEqual("PA9XYZ", qso.DECallsign);
            Assert.AreEqual(string.Empty, qso.Report);
            Assert.AreEqual("JO22", qso.GridSquare);

            decode.Message = "PA9XYZ G4ABC/P RR73";
            qso = WsjtxQsoParser.ParseDecode(decode);
            Assert.IsFalse(qso.IsCallingCQ);
            Assert.AreEqual(WsjtxQsoState.Rogers, qso.QsoState);
            Assert.AreEqual(string.Empty, qso.CallingModifier);
            Assert.AreEqual("PA9XYZ", qso.DXCallsign);
            Assert.AreEqual("G4ABC/P", qso.DECallsign);
            Assert.AreEqual("RR73", qso.Report);
            Assert.AreEqual(string.Empty, qso.GridSquare);
            #endregion

            #region ARRL RTTY Roundup
            decode.Message = "K1ABC W9XYZ 579 WI";
            qso = WsjtxQsoParser.ParseDecode(decode);
            Assert.IsFalse(qso.IsCallingCQ);
            Assert.AreEqual(WsjtxQsoState.Report, qso.QsoState);
            Assert.AreEqual(string.Empty, qso.CallingModifier);
            Assert.AreEqual("K1ABC", qso.DXCallsign);
            Assert.AreEqual("W9XYZ", qso.DECallsign);
            Assert.AreEqual("579 WI", qso.Report);
            Assert.AreEqual(string.Empty, qso.GridSquare);

            decode.Message = "W9XYZ K1ABC R 589 MA";
            qso = WsjtxQsoParser.ParseDecode(decode);
            Assert.IsFalse(qso.IsCallingCQ);
            Assert.AreEqual(WsjtxQsoState.RogerReport, qso.QsoState);
            Assert.AreEqual(string.Empty, qso.CallingModifier);
            Assert.AreEqual("W9XYZ", qso.DXCallsign);
            Assert.AreEqual("K1ABC", qso.DECallsign);
            Assert.AreEqual("R 589 MA", qso.Report);
            Assert.AreEqual(string.Empty, qso.GridSquare);

            decode.Message = "K1ABC KA0DEF 559 MO";
            qso = WsjtxQsoParser.ParseDecode(decode);
            Assert.IsFalse(qso.IsCallingCQ);
            Assert.AreEqual(WsjtxQsoState.Report, qso.QsoState);
            Assert.AreEqual(string.Empty, qso.CallingModifier);
            Assert.AreEqual("K1ABC", qso.DXCallsign);
            Assert.AreEqual("KA0DEF", qso.DECallsign);
            Assert.AreEqual("559 MO", qso.Report);
            Assert.AreEqual(string.Empty, qso.GridSquare);

            decode.Message = "TU; KA0DEF K1ABC R 569 MA";
            qso = WsjtxQsoParser.ParseDecode(decode);
            Assert.IsFalse(qso.IsCallingCQ);
            Assert.AreEqual(WsjtxQsoState.RogerReport, qso.QsoState);
            Assert.AreEqual(string.Empty, qso.CallingModifier);
            Assert.AreEqual("KA0DEF", qso.DXCallsign);
            Assert.AreEqual("K1ABC", qso.DECallsign);
            Assert.AreEqual("R 569 MA", qso.Report);
            Assert.AreEqual(string.Empty, qso.GridSquare);

            decode.Message = "KA1ABC G3AAA 529 0013";
            qso = WsjtxQsoParser.ParseDecode(decode);
            Assert.IsFalse(qso.IsCallingCQ);
            Assert.AreEqual(WsjtxQsoState.Report, qso.QsoState);
            Assert.AreEqual(string.Empty, qso.CallingModifier);
            Assert.AreEqual("KA1ABC", qso.DXCallsign);
            Assert.AreEqual("G3AAA", qso.DECallsign);
            Assert.AreEqual("529 0013", qso.Report);
            Assert.AreEqual(string.Empty, qso.GridSquare);

            decode.Message = "TU; G3AAA K1ABC R 559 MA";
            qso = WsjtxQsoParser.ParseDecode(decode);
            Assert.IsFalse(qso.IsCallingCQ);
            Assert.AreEqual(WsjtxQsoState.RogerReport, qso.QsoState);
            Assert.AreEqual(string.Empty, qso.CallingModifier);
            Assert.AreEqual("G3AAA", qso.DXCallsign);
            Assert.AreEqual("K1ABC", qso.DECallsign);
            Assert.AreEqual("R 559 MA", qso.Report);
            Assert.AreEqual(string.Empty, qso.GridSquare);
            #endregion

            #region Nonstandard call
            decode.Message = "CQ KH1/KH7Z";
            qso = WsjtxQsoParser.ParseDecode(decode);
            Assert.IsTrue(qso.IsCallingCQ);
            Assert.AreEqual(WsjtxQsoState.CallingCq, qso.QsoState);
            Assert.AreEqual(string.Empty, qso.CallingModifier);
            Assert.AreEqual(string.Empty, qso.DXCallsign);
            Assert.AreEqual("KH1/KH7Z", qso.DECallsign);
            Assert.AreEqual(string.Empty, qso.Report);
            Assert.AreEqual(string.Empty, qso.GridSquare);

            decode.Message = "CQ PJ4/K1ABC";
            qso = WsjtxQsoParser.ParseDecode(decode);
            Assert.IsTrue(qso.IsCallingCQ);
            Assert.AreEqual(WsjtxQsoState.CallingCq, qso.QsoState);
            Assert.AreEqual(string.Empty, qso.CallingModifier);
            Assert.AreEqual(string.Empty, qso.DXCallsign);
            Assert.AreEqual("PJ4/K1ABC", qso.DECallsign);
            Assert.AreEqual(string.Empty, qso.Report);
            Assert.AreEqual(string.Empty, qso.GridSquare);

            decode.Message = "PJ4/K1ABC <W9XYZ>";
            qso = WsjtxQsoParser.ParseDecode(decode);
            Assert.IsFalse(qso.IsCallingCQ);
            Assert.AreEqual(WsjtxQsoState.CallingStation, qso.QsoState);
            Assert.AreEqual(string.Empty, qso.CallingModifier);
            Assert.AreEqual("PJ4/K1ABC", qso.DXCallsign);
            Assert.AreEqual("W9XYZ", qso.DECallsign);
            Assert.AreEqual(string.Empty, qso.Report);
            Assert.AreEqual(string.Empty, qso.GridSquare);

            decode.Message = "<W9XYZ> PJ4/K1ABC RRR";
            qso = WsjtxQsoParser.ParseDecode(decode);
            Assert.IsFalse(qso.IsCallingCQ);
            Assert.AreEqual(WsjtxQsoState.Rogers, qso.QsoState);
            Assert.AreEqual(string.Empty, qso.CallingModifier);
            Assert.AreEqual("W9XYZ", qso.DXCallsign);
            Assert.AreEqual("PJ4/K1ABC", qso.DECallsign);
            Assert.AreEqual("RRR", qso.Report);
            Assert.AreEqual(string.Empty, qso.GridSquare);

            decode.Message = "PJ4/K1ABC <W9XYZ> 73";
            qso = WsjtxQsoParser.ParseDecode(decode);
            Assert.IsFalse(qso.IsCallingCQ);
            Assert.AreEqual(WsjtxQsoState.Signoff, qso.QsoState);
            Assert.AreEqual(string.Empty, qso.CallingModifier);
            Assert.AreEqual("PJ4/K1ABC", qso.DXCallsign);
            Assert.AreEqual("W9XYZ", qso.DECallsign);
            Assert.AreEqual("73", qso.Report);
            Assert.AreEqual(string.Empty, qso.GridSquare);

            decode.Message = "<W9XYZ> YW18FIFA";
            qso = WsjtxQsoParser.ParseDecode(decode);
            Assert.IsFalse(qso.IsCallingCQ);
            Assert.AreEqual(WsjtxQsoState.CallingStation, qso.QsoState);
            Assert.AreEqual(string.Empty, qso.CallingModifier);
            Assert.AreEqual("W9XYZ", qso.DXCallsign);
            Assert.AreEqual("YW18FIFA", qso.DECallsign);
            Assert.AreEqual(string.Empty, qso.Report);
            Assert.AreEqual(string.Empty, qso.GridSquare);

            decode.Message = "YW18FIFA <W9XYZ> RRR";
            qso = WsjtxQsoParser.ParseDecode(decode);
            Assert.IsFalse(qso.IsCallingCQ);
            Assert.AreEqual(WsjtxQsoState.Rogers, qso.QsoState);
            Assert.AreEqual(string.Empty, qso.CallingModifier);
            Assert.AreEqual("YW18FIFA", qso.DXCallsign);
            Assert.AreEqual("W9XYZ", qso.DECallsign);
            Assert.AreEqual("RRR", qso.Report);
            Assert.AreEqual(string.Empty, qso.GridSquare);

            decode.Message = "<W9XYZ> YW18FIFA 73";
            qso = WsjtxQsoParser.ParseDecode(decode);
            Assert.IsFalse(qso.IsCallingCQ);
            Assert.AreEqual(WsjtxQsoState.Signoff, qso.QsoState);
            Assert.AreEqual(string.Empty, qso.CallingModifier);
            Assert.AreEqual("W9XYZ", qso.DXCallsign);
            Assert.AreEqual("YW18FIFA", qso.DECallsign);
            Assert.AreEqual("73", qso.Report);
            Assert.AreEqual(string.Empty, qso.GridSquare);

            decode.Message = "CQ YW18FIFA";
            qso = WsjtxQsoParser.ParseDecode(decode);
            Assert.IsTrue(qso.IsCallingCQ);
            Assert.AreEqual(WsjtxQsoState.CallingCq, qso.QsoState);
            Assert.AreEqual(string.Empty, qso.CallingModifier);
            Assert.AreEqual(string.Empty, qso.DXCallsign);
            Assert.AreEqual("YW18FIFA", qso.DECallsign);
            Assert.AreEqual(string.Empty, qso.Report);
            Assert.AreEqual(string.Empty, qso.GridSquare);

            decode.Message = "<KA1ABC> YW18FIFA RR73";
            qso = WsjtxQsoParser.ParseDecode(decode);
            Assert.IsFalse(qso.IsCallingCQ);
            Assert.AreEqual(WsjtxQsoState.Rogers, qso.QsoState);
            Assert.AreEqual(string.Empty, qso.CallingModifier);
            Assert.AreEqual("KA1ABC", qso.DXCallsign);
            Assert.AreEqual("YW18FIFA", qso.DECallsign);
            Assert.AreEqual("RR73", qso.Report);
            Assert.AreEqual(string.Empty, qso.GridSquare);
            #endregion
        }
    }
}
