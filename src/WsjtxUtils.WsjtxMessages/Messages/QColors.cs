using System.Drawing;

namespace WsjtxUtils.WsjtxMessages.Messages
{
    /// <summary>
    /// Common <see cref="QColor"/> colors for use with <see cref="HighlightCallsign"/>
    /// </summary>
    public static class QColors
    {
        /// <summary>
        /// Clears the current color set in <see cref="HighlightCallsign"/>
        /// </summary>
        public static QColor ClearColor { get; private set; } = new QColor();

        /// <summary>
        /// The <see cref="QColor"/> Red
        /// </summary>
        public static QColor Red { get; private set; } = new QColor(Color.Red);

        /// <summary>
        /// The <see cref="QColor"/> Green
        /// </summary>
        public static QColor Green { get; private set; } = new QColor(Color.Green);

        /// <summary>
        /// The <see cref="QColor"/> Blue
        /// </summary>
        public static QColor Blue { get; private set; } = new QColor(Color.Blue);

        /// <summary>
        /// The <see cref="QColor"/> Yellow
        /// </summary>
        public static QColor Yellow { get; private set; } = new QColor(Color.Yellow);

        /// <summary>
        /// The <see cref="QColor"/> White
        /// </summary>
        public static QColor White { get; private set; } = new QColor(Color.White);

        /// <summary>
        /// The <see cref="QColor"/> Black
        /// </summary>
        public static QColor Black { get; private set; } = new QColor(Color.Black);
    }
}
