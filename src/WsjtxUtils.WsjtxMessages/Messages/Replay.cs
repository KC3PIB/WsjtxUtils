namespace WsjtxUtils.WsjtxMessages.Messages
{
    /// <summary>
    /// WSJT-X Replay message
    /// </summary>
    /// <remarks>
    /// When a server starts it may be useful for it to determine the
    /// state of preexisting clients. Sending this message to each
    /// client as it is discovered will cause that client (WSJT-X) to
    /// send a "Decode" message for each decode currently in its
    /// "Band activity" window.
    /// </remarks>
    public class Replay : WsjtxMessage, IWsjtxDirectionIn
    {
        /// <summary>
        /// Constructs a default WSJT-X Replay message
        /// </summary>
        public Replay() : base(MessageType.Replay)
        {
        }

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
