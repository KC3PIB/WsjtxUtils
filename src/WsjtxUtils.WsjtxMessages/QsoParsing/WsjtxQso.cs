using System;
using WsjtxUtils.WsjtxMessages.Messages;

namespace WsjtxUtils.WsjtxMessages.QsoParsing
{
    /// <summary>
    /// A decoded WSJT-X QSO
    /// </summary>
    public class WsjtxQso
    {
        /// <summary>
        /// Decoded WSJT-X QSO
        /// </summary>
        /// <param name="decode"></param>
        public WsjtxQso(Decode decode) : this(decode, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty)
        {

        }

        /// <summary>
        /// Decoded WSJT-X QSO
        /// </summary>
        /// <param name="decode"></param>
        /// <param name="callingModifier"></param>
        /// <param name="dxCallsign"></param>
        /// <param name="deCallsign"></param>
        /// <param name="gridSquare"></param>
        /// <param name="report"></param>
        public WsjtxQso(Decode decode, string callingModifier, string dxCallsign, string deCallsign, string gridSquare, string report)
        {
            Source = decode;
            Mode = decode.Mode;
            LowConfidence = decode.LowConfidence;
            Time = DateTime.UtcNow.Date.AddSeconds(decode.Time / 1000);

            QsoState = WsjtxQsoState.Unknown;
            CallingModifier = callingModifier;
            DXCallsign = dxCallsign;
            DECallsign = deCallsign;
            GridSquare = gridSquare;
            Report = report;
        }

        /// <summary>
        /// The date and time the message was received by the local station
        /// </summary>
        public DateTime Time { get; set; }

        /// <summary>
        /// The WSJT-X mode
        /// </summary>
        public string Mode { get; set; }

        /// <summary>
        /// The signal report or exchange sent by <see cref="DECallsign"/>
        /// </summary>
        public string Report { get; set; }

        /// <summary>
        /// Low confidence decodes are flagged in protocols where the decoder
        /// knows that a decode has a higher than normal probability
        /// of being false, they should not be reported on publicly
        /// accessible services without some attached warning or further validation.
        /// </summary>
        public bool LowConfidence { get; set; }

        /// <summary>
        /// State of the current QSO
        /// </summary>
        public WsjtxQsoState QsoState { get; set; }

        /// <summary>
        /// Was priori information used to complete this call
        /// </summary>
        /// <remarks>https://www.physics.princeton.edu/pulsar/K1JT/wsjtx-doc/wsjtx-main-2.5.4.html#_ap_decoding</remarks>
        public bool UsedPriori { get; set; }

        /// <summary>
        /// Is the station calling CQ
        /// </summary>
        public bool IsCallingCQ { get => QsoState == WsjtxQsoState.CallingCq; }

        /// <summary>
        /// The CQ calling modifier
        /// </summary>
        public string CallingModifier { get; set; }

        /// <summary>
        /// The gridsquare of the calling station
        /// </summary>
        public string GridSquare { get; set; }

        /// <summary>
        /// The local callsign
        /// </summary>
        public string DECallsign { get; set; }

        /// <summary>
        /// The remote callsign
        /// </summary>
        public string DXCallsign { get; set; }

        /// <summary>
        /// The source decoded message
        /// </summary>
        public Decode Source { get; set; }
    }
}
