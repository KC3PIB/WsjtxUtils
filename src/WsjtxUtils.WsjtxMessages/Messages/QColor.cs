using System.Drawing;

namespace WsjtxUtils.WsjtxMessages.Messages
{
    /// <summary>
    /// Represents the QT QColor object
    /// </summary>
    public class QColor
    {
        /// <summary>
        /// Constructs a default QColor object
        /// </summary>
        public QColor() : this(Color.Empty)
        {
        }

        /// <summary>
        /// Constructs a QColor object based on hex color
        /// </summary>
        /// <param name="hexValue"></param>
        public QColor(string hexValue) : this(ColorTranslator.FromHtml(hexValue))
        {
        }

        /// <summary>
        /// Constructs a QColor object based on input color
        /// </summary>
        /// <param name="color"></param>
        public QColor(Color color)
        {
            if (!color.IsEmpty)
            {
                // https://github.com/radekp/qt/blob/master/src/gui/painting/qcolor.cpp
                Red = (ushort)(color.R << 8);
                Green = (ushort)(color.G << 8);
                Blue = (ushort)(color.B << 8);
                Alpha = (ushort)(color.A << 8);
                Spec = QColorSpec.Rgb;
            }
            else
            {
                Spec = QColorSpec.Invalid;
            }
        }

        /// <summary>
        /// The type of color specified
        /// </summary>
        public QColorSpec Spec { get; private set; }

        /// <summary>
        /// Alpha channel value
        /// </summary>
        public ushort Alpha { get; private set; }

        /// <summary>
        /// Red channel value
        /// </summary>
        public ushort Red { get; private set; }

        /// <summary>
        /// Green channel value
        /// </summary>
        public ushort Green { get; private set; }

        /// <summary>
        /// Blue channel value
        /// </summary>
        public ushort Blue { get; private set; }

        /// <summary>
        /// Padding value
        /// </summary>
        /// <remarks>Always Zero</remarks>
        public ushort Pad { get; private set; }

        /// <summary>
        /// Create a QColor object from a <see cref="System.Drawing.Color"/>
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        public static QColor FromSystemDrawingColor(Color color) => new(color);
    }
}
