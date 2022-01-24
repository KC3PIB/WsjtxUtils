namespace WsjtxUtils.WsjtxMessages.Messages
{
    /// <summary>
    /// Readable WSJT-X messages
    /// </summary>
    /// <remarks>
    /// Messages in the WSJT-X docs with the direction 'Out'
    /// </remarks>
    public interface IWsjtxDirectionOut
    {
        /// <summary>
        /// Using the <see cref="WsjtxMessageReader"/>, deserialize the values to the current message
        /// </summary>
        /// <param name="reader"></param>
        void ReadMessage(WsjtxMessageReader reader);
    }
}
