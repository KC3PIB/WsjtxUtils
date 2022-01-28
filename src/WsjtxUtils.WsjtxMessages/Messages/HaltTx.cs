namespace WsjtxUtils.WsjtxMessages.Messages
{
    /// <summary>
    /// WSJT-X HaltTx message
    /// </summary>
    /// <remarks>
    /// The server may stop a client from transmitting messages either
    /// immediately or at the end of the current transmission period
    /// using this message.
    /// </remarks>
    public class HaltTx : WsjtxMessage, IWsjtxDirectionIn
    {
        /// <summary>
        /// Constructs a default WSJT-X HaltT message
        /// </summary>
        public HaltTx() : this(string.Empty)
        {
        }

        /// <summary>
        /// Constructs a default WSJT-X HaltT message
        /// </summary>
        /// <param name="id"></param>
        /// <param name="autoTxOnly"></param>
        public HaltTx(string id, bool autoTxOnly = false) : base(id, MessageType.HaltTx)
        {
            AutoTxOnly = autoTxOnly;
        }

        /// <summary>
        /// Halt immediately or at the end of thecurrent transmission period
        /// </summary>
        public bool AutoTxOnly { get; set; }

        #region IWsjtxDirectionIn
        /// <summary>
        /// Using the <see cref="WsjtxMessageWriter"/>, serialize the message to raw bytes
        /// </summary>
        /// <param name="messageWriter"></param>
        public override void WriteMessage(WsjtxMessageWriter messageWriter)
        {
            base.WriteMessage(messageWriter);

            messageWriter.WriteBool(AutoTxOnly);
        }
        #endregion
    }
}
