namespace WsjtxUtils.WsjtxMessages.Messages
{
    /// <summary>
    /// WSJT-X WSPRDecode message
    /// </summary>
    /// <remarks>
    /// The decode message is sent when a new decode is completed, in
    /// this case the <see cref="WSPRDecode.New"/> field is true.
    /// It is also used in response to a <see cref="Replay"/> message
    /// where each old decode in the "Band activity" window, that has
    /// not been erased, is sent in order as a one of these messages
    /// with the <see cref="WSPRDecode.New"/> field set to false. See
    /// the <see cref="Replay"/> message for details of usage.The off
    /// air field indicates that the decode was decoded from a played
    /// back recording.
    /// </remarks>
    public class WSPRDecode : WsjtxMessage, IWsjtxDirectionOut
    {
        /// <summary>
        /// Constructs a default WSJT-X WSPRDecode message
        /// </summary>
        public WSPRDecode() : base(MessageType.WSPRDecode)
        {
            Callsign = string.Empty;
            Grid = string.Empty;
        }

        /// <summary>
        /// Is this a new decode or a replay
        /// </summary>
        public bool New { get; set; }

        /// <summary>
        /// Milli-seconds since midnight
        /// </summary>
        public uint Time { get; set; }

        /// <summary>
        /// Signal-to-noise ratio in dB
        /// </summary>
        public int Snr { get; set; }

        /// <summary>
        /// Time difference in seconds
        /// </summary>
        public float DeltaTimeSeconds { get; set; }

        /// <summary>
        /// Frequency in Hertz
        /// </summary>
        public ulong FrequencyHz { get; set; }

        /// <summary>
        /// Frequency drift in Hertz
        /// </summary>
        public int FrequencyDriftHz { get; set; }

        /// <summary>
        /// Remote station call sign
        /// </summary>
        public string Callsign { get; set; }

        /// <summary>
        /// Remote station Maidenhead grid square
        /// </summary>
        public string Grid { get; set; }

        /// <summary>
        /// Remote station power
        /// </summary>
        public int Power { get; set; }

        /// <summary>
        /// Is OffAir decode
        /// </summary>
        /// <remarks>
        /// Off air decodes are those that result from playing back a.WAV file
        /// </remarks>
        public bool OffAir { get; set; }

        #region IWsjtxDirectionOut
        /// <summary>
        ///  Using the <see cref="WsjtxMessageReader"/>, deserialize the values to the current message
        /// </summary>
        /// <param name="messageReader"></param>
        public override void ReadMessage(WsjtxMessageReader messageReader)
        {
            base.ReadMessage(messageReader);

            New = messageReader.ReadBool();
            Time = messageReader.ReadUInt32();
            Snr = messageReader.ReadInt32();
            DeltaTimeSeconds = (float)messageReader.ReadDouble();
            FrequencyHz = messageReader.ReadUInt64();
            FrequencyDriftHz = messageReader.ReadInt32();
            Callsign = messageReader.ReadString();
            Grid = messageReader.ReadString();
            Power = messageReader.ReadInt32();
            OffAir = messageReader.ReadBool();
        }
        #endregion
    }
}
