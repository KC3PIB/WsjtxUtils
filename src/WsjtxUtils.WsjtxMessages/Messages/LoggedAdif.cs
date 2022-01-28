namespace WsjtxUtils.WsjtxMessages.Messages
{
    /// <summary>
    /// WSJT-X LoggedAdif message
    /// </summary>
    /// <remarks>
    /// The logged ADIF message is sent to the server(s) when the
    /// WSJT-X user accepts the "Log QSO" dialog by clicking the "OK"
    /// button. The "ADIF text" field consists of a valid ADIF file
    /// such that the WSJT-X UDP header information is encapsulated
    /// into a valid ADIF header.
    /// 
    /// Note that receiving applications can treat the whole message
    /// as a valid ADIF file with one record without special parsing.
    /// </remarks>
    public class LoggedAdif : WsjtxMessage, IWsjtxDirectionOut
    {
        /// <summary>
        /// Constructs a default WSJT-X LoggedAdif message
        /// </summary>
        public LoggedAdif() : base(string.Empty, MessageType.LoggedADIF)
        {
            AdifText = string.Empty;
        }

        /// <summary>
        /// The ADIF text
        /// </summary>
        public string AdifText { get; set; }

        #region IWsjtxDirectionOut
        /// <summary>
        ///  Using the <see cref="WsjtxMessageReader"/>, deserialize the values to the current message
        /// </summary>
        /// <param name="messageReader"></param>
        public override void ReadMessage(WsjtxMessageReader messageReader)
        {
            base.ReadMessage(messageReader);

            AdifText = messageReader.ReadString();
        }
        #endregion
    }
}
