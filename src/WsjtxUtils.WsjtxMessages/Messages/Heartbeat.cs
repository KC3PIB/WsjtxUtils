namespace WsjtxUtils.WsjtxMessages.Messages
{
    /// <summary>
    /// WSJT-X Heartbeat message
    /// </summary>
    /// <remarks>
    /// Message sent on a periodic basis to detect the presence 
    /// of a client and also the unexpected disappearance of a client
    /// and by clients to learn the schema negotiated by the server
    /// after it receives the initial heartbeat message from a client.
    /// </remarks>
    public class Heartbeat : WsjtxMessage, IWsjtxDirectionIn, IWsjtxDirectionOut
    {
        /// <summary>
        /// Constructs a default WSJT-X Heartbeat message
        /// </summary>
        public Heartbeat() : this(string.Empty, string.Empty, string.Empty)
        {
        }

        /// <summary>
        /// Constructs a default WSJT-X Heartbeat message
        /// </summary>
        /// <param name="id"></param>
        /// <param name="version"></param>
        /// <param name="revision"></param>
        /// <param name="maximumSchemaNumber"></param>
        public Heartbeat(string id, string version, string revision, SchemaVersion maximumSchemaNumber = SchemaVersion.Version3) : base(id, MessageType.Heartbeat)
        {
            Version = version;
            Revision = revision;
            MaximumSchemaNumber = maximumSchemaNumber;
        }

        /// <summary>
        /// Maximum schema number common to the client and server
        /// </summary>
        public SchemaVersion MaximumSchemaNumber { get; set; }

        /// <summary>
        /// Application version
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// Application revision
        /// </summary>
        public string Revision { get; set; }

        #region IWsjtxDirectionOut
        /// <summary>
        ///  Using the <see cref="WsjtxMessageReader"/>, deserialize the values to the current message
        /// </summary>
        /// <param name="reader"></param>
        public override void ReadMessage(WsjtxMessageReader reader)
        {
            base.ReadMessage(reader);

            MaximumSchemaNumber = reader.ReadSchemaVersion();
            Version = reader.ReadString();
            Revision = reader.ReadString();
        }
        #endregion

        #region IWsjtxDirectionIn
        /// <summary>
        /// Using the <see cref="WsjtxMessageWriter"/>, serialize the message to raw bytes
        /// </summary>
        /// <param name="writer"></param>
        public override void WriteMessage(WsjtxMessageWriter writer)
        {
            base.WriteMessage(writer);

            writer.WriteEnum(MaximumSchemaNumber);
            writer.WriteString(Version);
            writer.WriteString(Revision);
        }
        #endregion
    }
}