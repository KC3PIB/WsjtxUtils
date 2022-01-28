using System.Drawing;

namespace WsjtxUtils.WsjtxMessages.Messages
{
    /// <summary>
    /// WSJT-X HighlightCallsign message
    /// </summary>
    /// <remarks>
    /// The server may send this message at any time. The message
    /// specifies the background and foreground color that will be
    /// used to highlight the specified callsign in the decoded
    /// messages printed in the Band Activity panel. The  WSJT-X
    /// clients maintain a list of such instructions and apply them to
    /// all decoded messages in the  band activity window. To clear
    /// and cancel highlighting send an invalid QColor value for
    /// either or both of the background and foreground fields.
    /// 
    /// When using this mode the total number of callsign highlighting
    /// requests should be limited otherwise the performance of WSJT-X
    /// decoding may be impacted. A rough rule of thumb might be too
    /// limit the number of active highlighting requests to no more
    /// than 100.
    /// </remarks>
    public class HighlightCallsign : WsjtxMessage, IWsjtxDirectionIn
    {
        /// <summary>
        /// Constructs a default WSJT-X HighlightCallsign message
        /// </summary>
        public HighlightCallsign() : this(string.Empty, string.Empty, QColors.ClearColor, QColors.ClearColor)
        {
        }

        /// <summary>
        /// Constructs a WSJT-X HighlightCallsign message
        /// </summary>
        /// <param name="id"></param>
        /// <param name="callsign"></param>
        /// <param name="backgroundColor"></param>
        /// <param name="foregroundColor"></param>
        /// <param name="highlightLast"></param>
        public HighlightCallsign(string id, string callsign, Color backgroundColor, Color foregroundColor, bool highlightLast = false) : this(id, callsign, QColor.FromSystemDrawingColor(backgroundColor), QColor.FromSystemDrawingColor(foregroundColor), highlightLast)
        {
        }

        /// <summary>
        /// Constructs a WSJT-X HighlightCallsign message
        /// </summary>
        /// <param name="id"></param>
        /// <param name="callsign"></param>
        /// <param name="backgroundColor"></param>
        /// <param name="foregroundColor"></param>
        /// <param name="highlightLast"></param>
        public HighlightCallsign(string id, string callsign, QColor backgroundColor, QColor foregroundColor, bool highlightLast = false) : base(id, MessageType.HighlightCallsign)
        {
            Callsign = callsign;
            BackgroundColor = backgroundColor;
            ForegroundColor = foregroundColor;
            HighlightLast = highlightLast;
        }

        /// <summary>
        /// The specified callsign to highlight
        /// </summary>
        public string Callsign { get; set; }

        /// <summary>
        /// Background color
        /// </summary>
        public QColor BackgroundColor { get; set; }

        /// <summary>
        /// Foreground color
        /// </summary>
        public QColor ForegroundColor { get; set; }

        /// <summary>
        /// Highlight in the last  period only
        /// </summary>
        public bool HighlightLast { get; set; }

        #region IWsjtxDirectionIn
        /// <summary>
        /// Using the <see cref="WsjtxMessageWriter"/>, serialize the message to raw bytes
        /// </summary>
        /// <param name="messageWriter"></param>
        public override void WriteMessage(WsjtxMessageWriter messageWriter)
        {
            base.WriteMessage(messageWriter);

            messageWriter.WriteString(Callsign);
            messageWriter.WriteColor(BackgroundColor);
            messageWriter.WriteColor(ForegroundColor);
            messageWriter.WriteBool(HighlightLast);
        }
        #endregion
    }
}
