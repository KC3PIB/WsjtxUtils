namespace WsjtxUtils.WsjtxMessages.Messages
{
    /// <summary>
    /// WSJT-X FreeText message
    /// </summary>
    /// <remarks>
    /// Allows the server to set the current free text message content. Sending this message
    /// with a non-empty "Text" field is equivalent to typing a new message (old contents
    /// are discarded) in to the WSJT-X free text message field.
    /// </remarks>
    public class FreeText : WsjtxMessage, IWsjtxDirectionIn
    {
        /// <summary>
        /// Constructs a default WSJT-X FreeText message
        /// </summary>
        public FreeText() : base(MessageType.FreeText)
        {
            Text = string.Empty;
        }

        /// <summary>
        /// Constructs a default WSJT-X FreeText message
        /// </summary>
        /// <param name="id"></param>
        /// <param name="text"></param>
        /// <param name="send"></param>
        public FreeText(string id, string text, bool send = true) : base(id, MessageType.FreeText)
        {
            Text = text;
            Send = send;
        }

        /// <summary>
        /// Free text message contents
        /// </summary>
        /// <remarks>
        /// It is the responsibility of the sender to limit the length of
        /// the message text and to limit it to legal message characters.
        /// If the message text is empty the meaning of the message is
        /// refined to send the current free text unchanged when the
        /// <see cref="FreeText.Send"/> flag is set or to clear the
        /// current free text when the <see cref="FreeText.Send"/>
        /// flag is unset.
        /// </remarks>
        public string Text { get; set; }

        /// <summary>
        /// Send the message
        /// </summary>
        public bool Send { get; set; }

        #region IWsjtxDirectionIn
        /// <summary>
        /// Using the <see cref="WsjtxMessageWriter"/>, serialize the message to raw bytes
        /// </summary>
        /// <param name="messageWriter"></param>
        public override void WriteMessage(WsjtxMessageWriter messageWriter)
        {
            base.WriteMessage(messageWriter);

            messageWriter.WriteString(Text);
            messageWriter.WriteBool(Send);
        }
        #endregion
    }
}
