namespace WsjtxUtils.WsjtxMessages.QsoParsing
{
    /// <summary>
    /// The state of the currently decoded QSO
    /// </summary>
    public enum WsjtxQsoState
    {
        /// <summary>
        /// The QSO is in an unknown state
        /// </summary>
        Unknown = 0,
        /// <summary>
        /// Reply to a station's CQ
        /// </summary>
        /// <remarks>Corresponds with TX 1</remarks>
        CallingStation = 1,
        /// <summary>
        /// Sending a signal report
        /// </summary>
        /// <remarks>Corresponds with TX 2</remarks>
        Report = 2,
        /// <summary>
        /// Sending a roger signal report
        /// </summary>
        /// <remarks>Corresponds with TX 3</remarks>
        RogerReport = 3,
        /// <summary>
        /// Sending a roger signal report
        /// </summary>
        /// <remarks>Corresponds with TX 4</remarks>
        Rogers = 4,
        /// <summary>
        /// Sending 73
        /// </summary>
        /// <remarks>Corresponds with TX 5</remarks>
        Signoff = 5,
        /// <summary>
        /// Calling CQ
        /// </summary>
        /// <remarks>Corresponds with TX 6</remarks>
        CallingCq = 6
    }
}
