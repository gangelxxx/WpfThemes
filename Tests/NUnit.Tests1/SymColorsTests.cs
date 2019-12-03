#region

using System.Collections.Generic;
using ThemesLib.InStat.Helpers;

#endregion

namespace NUnit.Tests1
{
    [TestFixture]
    public class SymColorsTests
    {
        [Test]
        public void TestMethod()
        {
            ISymColors symColors = new SymColors();
            Assert.IsTrue(((SolidColorBrush) symColors[-1]).Color == Colors.Transparent);
            Assert.IsTrue(((SolidColorBrush) symColors[16]).Color == Colors.Transparent);


            List<Brush> _colors = new List<Brush>();
            foreach (var symColor in symColors)
            {
                _colors.Add(symColor);
            }

            for (int i = 0; i < _colors.Count; i++)
            {
                Assert.IsTrue(((SolidColorBrush) symColors[i]).Color == ((SolidColorBrush) _colors[i]).Color);
            }
        }

        [Test]
        public void TestMethod2()
        {
            List<Brush> colorBrushes = new List<Brush>
            {
                "Transparent".ToBrush(),
                "#000000".ToBrush(),
                "#FFFFFF".ToBrush(),
                "#00CD00".ToBrush(),
                "#FF0000".ToBrush(),
                "#FFCD00".ToBrush(),
                "#00CDFF".ToBrush(),
                "#00CD00".ToBrush(),
                "#141448".ToBrush(),
                "#902914".ToBrush(),
                "#F44E69".ToBrush(),
                "#F997C2".ToBrush(),
                "#FFF794".ToBrush(),
                "#F1F794".ToBrush()
            };

            ISymColors symColors = new SymColors(colorBrushes);
            Assert.IsTrue(((SolidColorBrush) symColors[-1]).Color == Colors.Transparent);
            Assert.IsTrue(((SolidColorBrush) symColors[colorBrushes.Count]).Color == Colors.Transparent);
            Assert.IsTrue(colorBrushes.Count == symColors.Count);


            for (int i = 0; i < colorBrushes.Count; i++)
            {
                Assert.IsTrue(((SolidColorBrush) symColors[i]).Color == ((SolidColorBrush) colorBrushes[i]).Color);
            }
        }
    }
}