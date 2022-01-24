namespace WsjtxUtils.WsjtxMessages.Messages
{
    /// <summary>
    /// WSJT-X message types
    /// </summary>
    public enum MessageType : uint
    {
        /// <summary>
        /// A WSJT-X <see cref="Heartbeat"/> message
        /// </summary>
        Heartbeat = 0,
        /// <summary>
        /// A WSJT-X <see cref="Status"/> message
        /// </summary>
        Status = 1,
        /// <summary>
        /// A WSJT-X <see cref="Decode"/> message
        /// </summary>
        Decode = 2,
        /// <summary>
        /// A WSJT-X <see cref="Clear"/> message
        /// </summary>
        Clear = 3,
        /// <summary>
        /// A WSJT-X <see cref="Reply"/> message
        /// </summary>
        Reply = 4,
        /// <summary>
        /// A WSJT-X <see cref="QSOLogged"/> message
        /// </summary>
        QSOLogged = 5,
        /// <summary>
        /// A WSJT-X <see cref="Close"/> message
        /// </summary>
        Close = 6,
        /// <summary>
        /// A WSJT-X <see cref="Replay"/> message
        /// </summary>
        Replay = 7,
        /// <summary>
        /// A WSJT-X <see cref="HaltTx"/> message
        /// </summary>
        HaltTx = 8,
        /// <summary>
        /// A WSJT-X <see cref="FreeText"/> message
        /// </summary>
        FreeText = 9,
        /// <summary>
        /// A WSJT-X <see cref="WSPRDecode"/> message
        /// </summary>
        WSPRDecode = 10,
        /// <summary>
        /// A WSJT-X <see cref="Location"/> message
        /// </summary>
        Location = 11,
        /// <summary>
        /// A WSJT-X <see cref="LoggedADIF"/> message
        /// </summary>
        LoggedADIF = 12,
        /// <summary>
        /// A WSJT-X <see cref="HighlightCallsign"/> message
        /// </summary>
        HighlightCallsign = 13,
        /// <summary>
        /// A WSJT-X <see cref="SwitchConfiguration"/> message
        /// </summary>
        SwitchConfiguration = 14,
        /// <summary>
        /// A WSJT-X <see cref="Configure"/> message
        /// </summary>
        Configure = 15
    }
}
