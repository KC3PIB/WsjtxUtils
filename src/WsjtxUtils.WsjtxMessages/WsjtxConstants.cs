namespace WsjtxUtils.WsjtxMessages
{
    /// <summary>
    /// Useful constant values for WSJT-X
    /// </summary>
    public static class WsjtxConstants
    {
        /// <summary>
        /// Size of a short value
        /// </summary>
        public const int SizeOfShort = sizeof(short);

        /// <summary>
        /// Size of an int value
        /// </summary>
        public const int SizeOfInt = sizeof(int);

        /// <summary>
        /// Size of a long value
        /// </summary>
        public const int SizeOfLong = sizeof(long);

        /// <summary>
        /// Value for true
        /// </summary>
        public const byte TrueValue = 0x01;

        /// <summary>
        /// Value for false
        /// </summary>
        public const byte FalseValue = 0x00;

        /// <summary>
        /// Julian date constant
        /// </summary>
        public const double JulianConstant = 2415018.5;

        /// <summary>
        /// WSJT-X packet header magic number
        /// </summary>
        public const uint MagicNumber = 0xadbccbda;

        /// <summary>
        /// Length of wsjtx packet header in bytes
        /// </summary>
        public const int HeaderLengthInBytes = 8;

        /// <summary>
        /// Position of message type in a valid WSJTX message
        /// </summary>
        public const int MessageTypePosition = HeaderLengthInBytes;
    }
}
