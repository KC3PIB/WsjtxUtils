namespace WsjtxUtils.WsjtxMessages.Messages
{
    /// <summary>
    /// Target WSJT-X window to clear
    /// </summary>
    public enum ClearWindow : byte
    {
        /// <summary>
        /// Clear the WSJT-X "Band activity" window
        /// </summary>
        BandActivity = 0,
        /// <summary>
        /// Clear the WSJT-X "Rx Frequency" window
        /// </summary>
        RxFrequency = 1,
        /// <summary>
        /// Clear both the WSJT-X "Band Activity" and "Rx Frequency" windows
        /// </summary>
        Both = 2
    }
}
