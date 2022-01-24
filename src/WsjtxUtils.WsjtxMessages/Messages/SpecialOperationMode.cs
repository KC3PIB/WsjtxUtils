namespace WsjtxUtils.WsjtxMessages.Messages
{
    /// <summary>
    /// Value selected in the WSJT-X "Settings->Advanced->Special operating activity" panel
    /// </summary>
    public enum SpecialOperationMode : byte
    {
        /// <summary>
        /// No special operating mode
        /// </summary>
        NONE = 0,
        /// <summary>
        /// North American VHF contest
        /// </summary>
        NAVHF = 1,
        /// <summary>
        /// European Union VHF contest
        /// </summary>
        EUVHF = 3,
        /// <summary>
        /// ARRL Field Day
        /// </summary>
        FIELDDAY = 4,
        /// <summary>
        /// RTTY Round up
        /// </summary>
        RTTYRU = 5,
        /// <summary>
        /// Fox mode
        /// </summary>
        FOX = 6,
        /// <summary>
        /// Hound mode
        /// </summary>
        HOUND = 7
    }
}
