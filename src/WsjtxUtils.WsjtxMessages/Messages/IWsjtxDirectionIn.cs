namespace WsjtxUtils.WsjtxMessages.Messages
{
    /// <summary>
    /// Writeable WSJT-X messages
    /// </summary>
    /// <remarks>
    /// Messages in the WSJT-X docs with the direction 'In'
    /// </remarks>
    public interface IWsjtxDirectionIn
    {
        /// <summary>
        /// Using the <see cref="WsjtxMessageWriter"/>, serialize the message to raw bytes
        /// </summary>
        /// <param name="writer"></param>
        void WriteMessage(WsjtxMessageWriter writer);
    }
}
