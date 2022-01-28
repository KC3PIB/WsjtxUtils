namespace WsjtxUtils.WsjtxMessages.Messages
{
    /// <summary>
    /// A base class of common elements related to all WSJT-X messages
    /// </summary>
    public abstract class WsjtxMessage
    {
        /// <summary>
        /// Constructor for base WSJT-X message
        /// </summary>
        /// <param name="id"></param>
        /// <param name="messageType"></param>
        /// <param name="schemaVersion"></param>
        protected WsjtxMessage(string id, MessageType messageType, SchemaVersion schemaVersion = SchemaVersion.Version2)
        {
            MessageType = messageType;
            SchemaVersion = schemaVersion;
            Id = id;
        }

        /// <summary>
        /// The magic number that delineates messages
        /// </summary>
        /// <remarks>
        /// For valid packets, this should be the 32-bit unsigned integer 0xadbccbda
        /// </remarks>
        public uint MagicNumber { get; set; } = 0xadbccbda;

        /// <summary>
        /// The QDataStream version used to encode the message values
        /// </summary>
        /// <remarks>
        /// http://doc.qt.io/qt-5/datastreamformat.html
        /// </remarks>
        public SchemaVersion SchemaVersion { get; set; }

        /// <summary>
        /// The type of WSJT-X message
        /// </summary>
        public MessageType MessageType { get; private set; }

        /// <summary>
        /// The unique id for the client
        /// </summary>
        public string Id { get; set; }

        #region IWsjtxDirectionOut
        /// <summary>
        ///  Using the <see cref="WsjtxMessageReader"/>, deserialize the values to the current message
        /// </summary>
        /// <param name="reader"></param>
        public virtual void ReadMessage(WsjtxMessageReader reader)
        {
            MagicNumber = reader.ReadUInt32();
            SchemaVersion = reader.ReadSchemaVersion();
            MessageType = reader.ReadMessageType();
            Id = reader.ReadString();
        }
        #endregion

        #region IWsjtxDirectionIn
        /// <summary>
        /// Using the <see cref="WsjtxMessageWriter"/>, serialize the message to raw bytes
        /// </summary>
        /// <param name="writer"></param>
        public virtual void WriteMessage(WsjtxMessageWriter writer)
        {
            writer.WriteUInt32(MagicNumber);
            writer.WriteEnum(SchemaVersion);
            writer.WriteEnum(MessageType);
            writer.WriteString(Id);
        }
        #endregion
    }
}
