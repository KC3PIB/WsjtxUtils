namespace WsjtxUtils.WsjtxMessages.Messages
{
    /// <summary>
    /// WSJT-X Clear message
    /// </summary>
    /// <remarks>
    /// This message is sent when all prior messages have been discarded.
    /// It may also be sent to a WSJT-X instance in which case it clears
    /// one or both of the "Band Activity" and "Rx Frequency" windows.
    /// The server should discard all prior decoded messages upon receipt
    /// of this message
    /// </remarks>
    public class Clear : WsjtxMessage, IWsjtxDirectionOut, IWsjtxDirectionIn
    {
        /// <summary>
        /// Constructs a default WSJT-X Clear message
        /// </summary>
        public Clear() : base(MessageType.Clear)
        {
        }

        /// <summary>
        /// Constructs a default WSJT-X Clear message
        /// </summary>
        /// <param name="id"></param>
        /// <param name="window"></param>
        public Clear(string id, ClearWindow window) : base(MessageType.Clear)
        {
            Id = id;
            Window = window;
        }

        /// <summary>
        /// Specifies which window to clear
        /// </summary>
        public ClearWindow Window { get; set; }

        #region IWsjtxDirectionOut
        /// <summary>
        ///  Using the <see cref="WsjtxMessageReader"/>, deserialize the values to the current message
        /// </summary>
        /// <param name="messageReader"></param>
        public override void ReadMessage(WsjtxMessageReader messageReader)
        {
            base.ReadMessage(messageReader);
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
            messageWriter.WriteEnum(Window);
        }
        #endregion
    }
}
