using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using WsjtxUtils.WsjtxMessages.Messages;

namespace WsjtxUtils.WsjtxMessages.Tests.Messages
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class QColorTests
    {
        [TestMethod()]
        public void CreateQColorFrom_WindowsColor_ProducesValidColorValues()
        {
            var red = new QColor(Color.Red);

            Assert.AreEqual(QColorSpec.Rgb, red.Spec);
            Assert.AreEqual((uint)Color.Red.R << 8, red.Red);
            Assert.AreEqual((uint)Color.Red.G << 8, red.Green);
            Assert.AreEqual((uint)Color.Red.B << 8, red.Blue);
            Assert.AreEqual((uint)Color.Red.A << 8, red.Alpha);
            Assert.AreEqual(0, red.Pad);

            var green = new QColor(Color.Green);

            Assert.AreEqual(QColorSpec.Rgb, green.Spec);
            Assert.AreEqual((uint)Color.Green.R << 8, green.Red);
            Assert.AreEqual((uint)Color.Green.G << 8, green.Green);
            Assert.AreEqual((uint)Color.Green.B << 8, green.Blue);
            Assert.AreEqual((uint)Color.Green.A << 8, green.Alpha);
            Assert.AreEqual(0, green.Pad);

            var blue = new QColor(Color.Blue);

            Assert.AreEqual(QColorSpec.Rgb, blue.Spec);
            Assert.AreEqual((uint)Color.Blue.R << 8, blue.Red);
            Assert.AreEqual((uint)Color.Blue.G << 8, blue.Green);
            Assert.AreEqual((uint)Color.Blue.B << 8, blue.Blue);
            Assert.AreEqual((uint)Color.Blue.A << 8, blue.Alpha);
            Assert.AreEqual(0, blue.Pad);

            var chartreuse = new QColor(Color.Chartreuse);

            Assert.AreEqual(QColorSpec.Rgb, chartreuse.Spec);
            Assert.AreEqual((uint)Color.Chartreuse.R << 8, chartreuse.Red);
            Assert.AreEqual((uint)Color.Chartreuse.G << 8, chartreuse.Green);
            Assert.AreEqual((uint)Color.Chartreuse.B << 8, chartreuse.Blue);
            Assert.AreEqual((uint)Color.Chartreuse.A << 8, chartreuse.Alpha);
            Assert.AreEqual(0, chartreuse.Pad);

            var transparent = new QColor(Color.Transparent);

            Assert.AreEqual(QColorSpec.Rgb, transparent.Spec);
            Assert.AreEqual((uint)Color.Transparent.R << 8, transparent.Red);
            Assert.AreEqual((uint)Color.Transparent.G << 8, transparent.Green);
            Assert.AreEqual((uint)Color.Transparent.B << 8, transparent.Blue);
            Assert.AreEqual((uint)Color.Transparent.A << 8, transparent.Alpha);
            Assert.AreEqual(0, transparent.Pad);
        }

        [TestMethod()]
        public void CreateQColorFrom_HtmlColor_ProducesValidColorValues()
        {
            var color = new QColor("#FFBF00");

            Assert.AreEqual(QColorSpec.Rgb, color.Spec);
            Assert.AreEqual((uint)255 << 8, color.Red);
            Assert.AreEqual((uint)191 << 8, color.Green);
            Assert.AreEqual((uint)0 << 8, color.Blue);
            Assert.AreEqual((uint)255 << 8, color.Alpha);

            color = new QColor("IndianRed");

            Assert.AreEqual(QColorSpec.Rgb, color.Spec);
            Assert.AreEqual((uint)205 << 8, color.Red);
            Assert.AreEqual((uint)92 << 8, color.Green);
            Assert.AreEqual((uint)92 << 8, color.Blue);
            Assert.AreEqual((uint)255 << 8, color.Alpha);
        }

        [TestMethod()]
        public void CreateQColorFrom_WithNoParameters_ProducesInvalidColorspec()
        {
            var color = new QColor();

            Assert.AreEqual(QColorSpec.Invalid, color.Spec);
            Assert.AreEqual(0, color.Red);
            Assert.AreEqual(0, color.Green);
            Assert.AreEqual(0, color.Blue);
            Assert.AreEqual(0, color.Alpha);
            Assert.AreEqual(0, color.Pad);
        }

        [TestMethod()]
        public void CreateQColor_WithColorEmpty_ProducesInvalidColorspec()
        {
            var color = new QColor(Color.Empty);

            Assert.AreEqual(QColorSpec.Invalid, color.Spec);
            Assert.AreEqual(0, color.Red);
            Assert.AreEqual(0, color.Green);
            Assert.AreEqual(0, color.Blue);
            Assert.AreEqual(0, color.Alpha);
            Assert.AreEqual(0, color.Pad);
        }
    }
}
