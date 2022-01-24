namespace WsjtxUtils.WsjtxMessages.Messages
{
    /// <summary>
    /// WSJT-X Reply message
    /// </summary>
    /// <remarks>
    /// Initiate a QSO by sending this message to a client. WSJT-X filters this message and only
    /// acts upon it if the message exactly describes a prior decode and that decode is a CQ
    /// or QRZ message.The action taken is exactly equivalent to the user double clicking the
    /// message in the "Band activity" window.
    /// </remarks>
    public class Reply : WsjtxMessage, IWsjtxDirectionIn
    {
        /// <summary>
        /// Constructs a default WSJT-X Reply message
        /// </summary>
        public Reply() : base(MessageType.Reply)
        {
            Mode = string.Empty;
            Message = string.Empty;
        }

        /// <summary>
        /// Constructs a WSJT-X Reply message
        /// </summary>
        /// <param name="decode"></param>
        /// <param name="modifiers"></param>
        public Reply(Decode decode, KeyboardModifiers modifiers = KeyboardModifiers.None) : this(decode.Id, decode.Time, decode.Snr, decode.OffsetTimeSeconds, decode.OffsetFrequencyHz, decode.Mode, decode.Message, decode.LowConfidence, modifiers)
        {
        }

        /// <summary>
        /// Constructs a WSJT-X Reply message
        /// </summary>
        /// <param name="id"></param>
        /// <param name="time"></param>
        /// <param name="snr"></param>
        /// <param name="offsetTimeSeconds"></param>
        /// <param name="offsetFrequencyHz"></param>
        /// <param name="mode"></param>
        /// <param name="message"></param>
        /// <param name="lowConfidence"></param>
        /// <param name="modifiers"></param>
        public Reply(string id, uint time, int snr, float offsetTimeSeconds, uint offsetFrequencyHz, string mode, string message, bool lowConfidence, KeyboardModifiers modifiers = KeyboardModifiers.None) : base(MessageType.Reply)
        {
            Id = id;
            Time = time;
            Snr = snr;
            OffsetTimeSeconds = offsetTimeSeconds;
            OffsetFrequencyHz = offsetFrequencyHz;
            Mode = mode;
            Message = message;
            LowConfidence = lowConfidence;
            Modifiers = modifiers;
        }

        /// <summary>
        /// Milliseconds since midnight
        /// </summary>
        public uint Time { get; set; }

        /// <summary>
        /// Signal-to-noise ratio in dB
        /// </summary>
        public int Snr { get; set; }

        /// <summary>
        /// Time offset in seconds relative to your computer clock
        /// </summary>
        public float OffsetTimeSeconds { get; set; }

        /// <summary>
        ///  Frequency offset from nominal in Hertz
        /// </summary>
        public uint OffsetFrequencyHz { get; set; }

        /// <summary>
        /// The selected WSJT-X mode
        /// </summary>
        public string Mode { get; set; }

        /// <summary>
        /// The message exchanged
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Low confidence decode flag
        /// </summary>
        /// <remarks>
        /// Low confidence decodes are flagged in protocols where the decoder
        /// has knows that a decode has a higher than normal probability
        /// of being false, they should not be reported on publicly
        /// accessible services without some attached warning or further validation.
        /// </remarks>
        public bool LowConfidence { get; set; }

        /// <summary>
        /// Keyboard modifiers specified during reply
        /// </summary>
        /// <remarks>
        /// Allows the equivalent of the keyboard modifiers "as if" those
        /// modifier keys were pressed while double-clicking the specified
        /// decoded message.
        /// </remarks>
        public KeyboardModifiers Modifiers { get; set; }

        #region IWsjtxDirectionIn
        /// <summary>
        /// Using the <see cref="WsjtxMessageWriter"/>, serialize the message to raw bytes
        /// </summary>
        /// <param name="messageWriter"></param>
        public override void WriteMessage(WsjtxMessageWriter messageWriter)
        {
            base.WriteMessage(messageWriter);

            messageWriter.WriteUInt32(Time);
            messageWriter.WriteInt32(Snr);
            messageWriter.WriteDouble(OffsetTimeSeconds);
            messageWriter.WriteUInt32(OffsetFrequencyHz);
            messageWriter.WriteString(Mode);
            messageWriter.WriteString(Message);
            messageWriter.WriteBool(LowConfidence);
            messageWriter.WriteEnum(Modifiers);
        }
        #endregion
    }
}
