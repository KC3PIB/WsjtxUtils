namespace WsjtxUtils.WsjtxMessages.Messages
{
    /// <summary>
    /// WSJT-X Configure message
    /// </summary>
    /// <remarks>
    /// The server may send this message at any time. The message
    /// specifies various configuration options.  For utf8 string
    /// fields an empty value implies no change, for the quint32
    /// Rx DF and Frequency Tolerance fields the <see cref="uint.MaxValue"/>
    /// value implies no change. Invalid or unrecognized values will be
    /// silently ignored.
    /// </remarks>
    public class Configure : WsjtxMessage, IWsjtxDirectionIn
    {
        /// <summary>
        /// Constructs a default WSJT-X Configure message
        /// </summary>
        public Configure() : base(MessageType.Configure)
        {
        }

        /// <summary>
        /// Constructs a WSJT-X Configure message
        /// </summary>
        /// <param name="id"></param>
        public Configure(string id) : base(id, MessageType.Configure)
        {
        }

        /// <summary>
        /// The selected WSJT-X mode
        /// </summary>
        public string Mode { get; set; } = string.Empty;

        /// <summary>
        /// Frequency tolerance for mode
        /// </summary>
        /// <remarks>
        /// May have a value of <see cref="uint.MaxValue"/> which implies the field is not applicable
        /// </remarks>
        public uint FrequencyTolerance { get; set; } = uint.MaxValue;

        /// <summary>
        /// Which WSJT-X submode is in use
        /// </summary>
        public string SubMode { get; set; } = string.Empty;

        /// <summary>
        /// Is WSJT-X using a fast mode
        /// </summary>
        /// <remarks>
        /// The fast modes in WSJT-X send their message frames repeatedly, as many times as will fit into the Tx sequence length.
        /// “Slow” in this sense implies message frames being sent only once per transmission.
        /// </remarks>
        public bool FastMode { get; set; }

        /// <summary>
        /// The transmit/receive period
        /// </summary>
        /// <remarks>
        /// May have a value of <see cref="uint.MaxValue"/> which implies the field is not applicable
        /// </remarks>
        public uint TRPeriod { get; set; } = uint.MaxValue;

        /// <summary>
        /// RX frequency differential
        /// </summary>
        /// <remarks>
        /// May have a value of <see cref="uint.MaxValue"/> which implies the field is not applicable
        /// </remarks>
        public uint RxDF { get; set; } = uint.MaxValue;

        /// <summary>
        /// Remote call sign
        /// </summary>
        public string DXCall { get; set; } = string.Empty;

        /// <summary>
        /// Remote grid square
        /// </summary>
        public string DXGrid { get; set; } = string.Empty;

        /// <summary>
        /// Generate standard messages
        /// </summary>
        public bool GenerateMessages { get; set; }

        #region IWsjtxDirectionIn
        /// <summary>
        /// Using the <see cref="WsjtxMessageWriter"/>, serialize the message to raw bytes
        /// </summary>
        /// <param name="messageWriter"></param>
        public override void WriteMessage(WsjtxMessageWriter messageWriter)
        {
            base.WriteMessage(messageWriter);

            messageWriter.WriteString(Mode);
            messageWriter.WriteUInt32(FrequencyTolerance);
            messageWriter.WriteString(SubMode);
            messageWriter.WriteBool(FastMode);
            messageWriter.WriteUInt32(TRPeriod);
            messageWriter.WriteUInt32(RxDF);
            messageWriter.WriteString(DXCall);
            messageWriter.WriteString(DXGrid);
            messageWriter.WriteBool(GenerateMessages);
        }
        #endregion
    }
}
