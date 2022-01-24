namespace WsjtxUtils.WsjtxMessages.Messages
{
    /// <summary>
    /// WSJT-X Close message
    /// </summary>
    /// <remarks>
    /// Close is sent by a client immediately prior to it shutting down gracefully
    /// </remarks>
    public class Close : WsjtxMessage, IWsjtxDirectionOut, IWsjtxDirectionIn
    {
        /// <summary>
        /// Constructs a default WSJT-X Close message
        /// </summary>
        public Close() : base(MessageType.Close)
        {
        }

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
        }
        #endregion
    }
}
