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
        /// <param name="messageReader"></param>
        public override void ReadMessage(WsjtxMessageReader messageReader)
        {
            base.ReadMessage(messageReader);

            // Note: the "Maximum schema number" field was introduced at the same time as
            // schema 3, therefore servers and clients must assume schema 2 is the highest
            // schema number supported if the Heartbeat message does not contain the
            // "Maximum schema number" field.

            if (messageReader.Position < messageReader.BufferLength)
                MaximumSchemaNumber = messageReader.ReadSchemaVersion();
            else
                MaximumSchemaNumber = SchemaVersion.Version2;

            if (messageReader.Position < messageReader.BufferLength)
                Version = messageReader.ReadString();

            if (messageReader.Position < messageReader.BufferLength)
                Revision = messageReader.ReadString();
        }
        #endregion

        #region IWsjtxDirectionIn
        /// <summary>
        /// Using the <see cref="WsjtxMessageWriter"/>, serialize the message to raw bytes
        /// </summary>
        /// <param name="messageWriter"></param>
        public override void WriteMessage(WsjtxMessageWriter messageWriter)
        {
            base.WriteMessage(messageWriter);

            messageWriter.WriteEnum(MaximumSchemaNumber);
            messageWriter.WriteString(Version);
            messageWriter.WriteString(Revision);
        }
        #endregion
    }
}