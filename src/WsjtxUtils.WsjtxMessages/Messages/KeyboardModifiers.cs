namespace WsjtxUtils.WsjtxMessages.Messages
{
    /// <summary>
    /// Keyboard modifiers for the <see cref="Reply"/> message
    /// </summary>
    public enum KeyboardModifiers : byte
    {
        /// <summary>
        /// No keys
        /// </summary>
        None = 0x00,
        /// <summary>
        /// Shift key
        /// </summary>
        Shift = 0x02,
        /// <summary>
        /// CTRL key
        /// </summary>
        /// <remarks>
        /// CMD on Mac
        /// </remarks>
        Ctrl = 0x04,
        /// <summary>
        /// ALT key
        /// </summary>
        Alt = 0x08,
        /// <summary>
        /// Windows key on MS Windows
        /// </summary>
        Meta = 0x10,
        /// <summary>
        /// Keypad or arrows
        /// </summary>
        Keypad = 0x20,
        /// <summary>
        /// X11 only
        /// </summary>
        GroupSwitch = 0x40
    }
}
