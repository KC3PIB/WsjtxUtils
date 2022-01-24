using System;
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
        public QColor()
        {
            Spec = QColorSpec.Invalid;
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
        public QColor(Color color) : this(color.R, color.G, color.B, color.A)
        {
        }

        /// <summary>
        /// Constructs a QColor object based on input color values
        /// </summary>
        /// <param name="red"></param>
        /// <param name="green"></param>
        /// <param name="blue"></param>
        /// <param name="alpha"></param>
        public QColor(int red, int green, int blue, int alpha = 255)
        {
            if (red < 0 || red > 255)
                throw new ArgumentException($"Red value must be less than or equal to 255 and greater than 0, currently {red}");

            if (green < 0 || green > 255)
                throw new ArgumentException($"Green value must be less than or equal to 255 and greater than 0, currently {green}");

            if (blue < 0 || blue > 255)
                throw new ArgumentException($"Blue value must be less than or equal to 255 and greater than 0, currently {blue}");

            if (alpha < 0 || alpha > 255)
                throw new ArgumentException($"Alpha value must be less than or equal to 255, currently {alpha}");

            // https://github.com/radekp/qt/blob/master/src/gui/painting/qcolor.cpp
            Red = (ushort)(red << 8);
            Green = (ushort)(green << 8);
            Blue = (ushort)(blue << 8);
            Alpha = (ushort)(alpha << 8);
            Spec = QColorSpec.ExtendedRgb;
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
