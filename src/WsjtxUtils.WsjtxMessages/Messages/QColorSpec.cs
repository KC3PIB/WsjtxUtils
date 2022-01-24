namespace WsjtxUtils.WsjtxMessages.Messages
{
    /// <summary>
    /// The type of color specified, either RGB, HSV, CMYK or HSL.
    /// </summary>
    public enum QColorSpec : byte
    {
        /// <summary>
        /// Invalid QColor
        /// </summary>
        Invalid = 0,
        /// <summary>
        /// Red, Green, Blue
        /// </summary>
        Rgb = 1,
        /// <summary>
        /// Hue, Saturation, Value
        /// </summary>
        Hsv = 2,
        /// <summary>
        /// Cyan, Magenta, Yellow, and Key
        /// </summary>
        Cmyk = 3,
        /// <summary>
        /// Hue, Saturation, Lightness
        /// </summary>
        Hsl = 4,
        /// <summary>
        /// Red, Green, Blue, Alpha
        /// </summary>
        ExtendedRgb = 5
    }
}
