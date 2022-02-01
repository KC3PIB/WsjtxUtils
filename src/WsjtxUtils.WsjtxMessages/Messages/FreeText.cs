using System;

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
        private string _text;

        /// <summary>
        /// Constructs a default WSJT-X FreeText message
        /// </summary>
        public FreeText() : this(string.Empty, string.Empty)
        {
        }

        /// <summary>
        /// Constructs a default WSJT-X FreeText message
        /// </summary>
        /// <param name="id"></param>
        /// <param name="text"></param>
        /// <param name="send"></param>
        public FreeText(string id, string text, bool send = false) : base(id, MessageType.FreeText)
        {
            if (text.Length > 13)
                throw new ArgumentException($"The free text message can not exceed 13 characters. There are {text.Length} characters in the message: '{text}'");

            _text = text;
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
        /// Up to a maximum of 13 characters, including spaces. In general
        /// you should avoid the character / in free-text messages, as WSJT-X
        /// may then try to interpret your construction as part of a compound callsign.
        /// </remarks>
        public string Text
        {
            get => _text;
            set => _text = (value.Length <= 13) ? value : throw new ArgumentException($"The free text message can not exceed 13 characters. There are {value.Length} characters in the message: '{value}'");
        }

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
