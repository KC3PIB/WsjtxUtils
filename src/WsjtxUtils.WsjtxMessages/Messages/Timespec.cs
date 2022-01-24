namespace WsjtxUtils.WsjtxMessages.Messages
{
    /// <summary>
    /// QDateTime Timespec
    /// </summary>
    public enum Timespec : byte
    {
        /// <summary>
        /// Local time, controlled by a system time-zone setting.
        /// </summary>
        Local = 0,
        /// <summary>
        /// Coordinated Universal Time.
        /// </summary>
        UTC = 1,
        /// <summary>
        /// An offset in seconds from Coordinated Universal Time.
        /// </summary>
        OffsetFromUTCSeconds = 2,
        /// <summary>
        /// A named time zone.
        /// </summary>
        TimeZone = 3
    }
}
