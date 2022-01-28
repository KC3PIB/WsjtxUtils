namespace WsjtxUtils.WsjtxMessages.Messages
{
    /// <summary>
    /// WSJT-X Location message
    /// </summary>
    /// <remarks>
    /// This message allows the server to set the current geographical
    /// location of operation. The supplied location is not persistent
    /// but is used as a session lifetime replacement loction that overrides
    /// the Maidenhead grid locater set in the application settings.
    /// The intent is to allow an external application to update the
    /// operating location dynamically during a mobile period of operation.
    /// </remarks>
    public class Location : WsjtxMessage, IWsjtxDirectionIn
    {
        /// <summary>
        /// Constructs a default WSJT-X Location message
        /// </summary>
        public Location() : this(string.Empty, string.Empty)
        {
        }

        /// <summary>
        /// Constructs a default WSJT-X Location message
        /// </summary>
        /// <param name="id"></param>
        /// <param name="gridSquare"></param>
        public Location(string id, string gridSquare) : base(id, MessageType.Location)
        {
            LocationGridSquare = gridSquare;
        }

        /// <summary>
        /// Maidenhead grid square
        /// </summary>
        /// <remarks>
        /// Currently only Maidenhead grid squares or sub-squares are
        /// accepted, i.e. 4- or 6-digit locators. Other formats may be
        /// accepted in future.
        /// </remarks>
        public string LocationGridSquare { get; set; }

        #region IWsjtxDirectionIn
        /// <summary>
        /// Using the <see cref="WsjtxMessageWriter"/>, serialize the message to raw bytes
        /// </summary>
        /// <param name="messageWriter"></param>
        public override void WriteMessage(WsjtxMessageWriter messageWriter)
        {
            base.WriteMessage(messageWriter);

            messageWriter.WriteString(LocationGridSquare);
        }
        #endregion
    }
}
