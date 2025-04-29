namespace WsjtxUtils.WsjtxMessages.Messages
{
    /// <summary>
    /// WSJT-X Status message
    /// </summary>
    /// <remarks>
    /// WSJT-X sends this status message when various internal state
    /// changes to allow the server to track the relevant state of each
    /// client without the need for polling commands.
    /// </remarks>
    public class Status : WsjtxMessage, IWsjtxDirectionOut
    {
        /// <summary>
        /// Constructs a default WSJT-X Status message
        /// </summary>
        public Status() : base(string.Empty, MessageType.Status)
        {
            Mode = string.Empty;
            DXCall = string.Empty;
            Report = string.Empty;
            TXMode = string.Empty;
            DECall = string.Empty;
            DEGrid = string.Empty;
            DXGrid = string.Empty;
            SubMode = string.Empty;
            ConfigurationName = string.Empty;
            TXMessage = string.Empty;
        }

        /// <summary>
        /// Dial Frequency (Hz)
        /// </summary>
        public ulong DialFrequencyInHz { get; set; }

        /// <summary>
        /// WSJT-X Operating mode
        /// </summary>
        public string Mode { get; set; }

        /// <summary>
        /// Value of the DX Call field
        /// </summary>
        public string DXCall { get; set; }

        /// <summary>
        /// Signal report
        /// </summary>
        public string Report { get; set; }

        /// <summary>
        /// Transmitting mode
        /// </summary>
        public string TXMode { get; set; }

        /// <summary>
        /// Is transmit enabled
        /// </summary>
        public bool TXEnabled { get; set; }

        /// <summary>
        /// Is WSJT-X currently transmitting
        /// </summary>
        public bool Transmitting { get; set; }

        /// <summary>
        /// Is WSJT-X currently decoding
        /// </summary>
        public bool Decoding { get; set; }

        /// <summary>
        /// RX frequency offset from nominal in Hertz
        /// </summary>
        public uint RXOffsetFrequencyHz { get; set; }

        /// <summary>
        /// TX frequency offset from nominal in Hertz
        /// </summary>
        public uint TXOffsetFrequencyHz { get; set; }

        /// <summary>
        /// Local call sign
        /// </summary>
        public string DECall { get; set; }

        /// <summary>
        /// Local grid square
        /// </summary>
        public string DEGrid { get; set; }

        /// <summary>
        /// Remote grid square
        /// </summary>
        public string DXGrid { get; set; }

        /// <summary>
        /// Has the watchdog timer been hit
        /// </summary>
        public bool TXWatchdog { get; set; }

        /// <summary>
        /// Which WSJT-X submode is in use
        /// </summary>
        public string SubMode { get; set; }

        /// <summary>
        /// Is WSJT-X using a fast mode
        /// </summary>
        /// <remarks>
        /// The fast modes in WSJT-X send their message frames repeatedly, as many times as will fit into the Tx sequence length.
        /// “Slow” in this sense implies message frames being sent only once per transmission.
        /// </remarks>
        public bool FastMode { get; set; }

        /// <summary>
        /// Special operation mode
        /// </summary>
        /// <remarks>
        /// The Special operation mode is an enumeration that indicates the
        /// setting selected in the WSJT-X "Settings->Advanced->Special
        /// operating activity" panel. For possible values see <see cref="SpecialOperationMode"/>.
        /// </remarks>
        public SpecialOperationMode SpecialOperationMode { get; set; }

        /// <summary>
        /// Frequency tolerance range over which decoding is attempted, centered on the Rx frequency.
        /// </summary>
        /// <remarks>
        /// May have a value of <see cref="uint.MaxValue"/> which implies the field is not applicable
        /// </remarks>
        public uint FrequencyTolerance { get; set; }

        /// <summary>
        /// The transmit/receive period
        /// </summary>
        /// <remarks>
        /// May have a value of <see cref="uint.MaxValue"/> which implies the field is not applicable
        /// </remarks>
        public uint TRPeriod { get; set; }

        /// <summary>
        /// Configuration name in use
        /// </summary>
        public string ConfigurationName { get; set; }

        /// <summary>
        /// The message being transmitted
        /// </summary>
        public string TXMessage { get; set; }

        #region IWsjtxDirectionOut
        /// <summary>
        ///  Using the <see cref="WsjtxMessageReader"/>, deserialize the values to the current message
        /// </summary>
        /// <param name="reader"></param>
        public override void ReadMessage(WsjtxMessageReader reader)
        {
            base.ReadMessage(reader);

            DialFrequencyInHz = reader.ReadUInt64();
            Mode = reader.ReadString();
            DXCall = reader.ReadString();
            Report = reader.ReadString();
            TXMode = reader.ReadString();
            TXEnabled = reader.ReadBool();
            Transmitting = reader.ReadBool();
            Decoding = reader.ReadBool();
            RXOffsetFrequencyHz = reader.ReadUInt32();
            TXOffsetFrequencyHz = reader.ReadUInt32();
            DECall = reader.ReadString();
            DEGrid = reader.ReadString();
            DXGrid = reader.ReadString();
            TXWatchdog = reader.ReadBool();
            SubMode = reader.ReadString();
            FastMode = reader.ReadBool();
            SpecialOperationMode = reader.ReadSpecialOperationMode();

            // check if there is any remaining data. this is a workaround for #113, JTDX status packets
            if(!reader.IsDataAvailable())
                return;

            FrequencyTolerance = reader.ReadUInt32();
            TRPeriod = reader.ReadUInt32();
            ConfigurationName = reader.ReadString();
            TXMessage = reader.ReadString();
        }
        #endregion
    }
}
