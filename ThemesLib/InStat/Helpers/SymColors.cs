#region

using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Media;
using ThemesLib.Properties;

#endregion

namespace ThemesLib.InStat.Helpers
{
    public class SymColors : ISymColors
    {
        #region Private fields

        private readonly List<Brush> _colorsSource = new List<Brush>
        {
//            "Transparent".ToBrush(),
            "#ffffff".ToBrush(),
            "#b4b4b4".ToBrush(),
            "#666666".ToBrush(),
            "#000000".ToBrush(),
            "#ffcc33".ToBrush(),
            "#ff9900".ToBrush(),
            "#993300".ToBrush(),
            "#330000".ToBrush(),
            "#99ff99".ToBrush(),
            "#99cc66".ToBrush(),
            "#009900".ToBrush(),
            "#073b00".ToBrush(),
            "#99ffff".ToBrush(),
            "#66ccff".ToBrush(),
            "#0066cc".ToBrush(),
            "#003e6d".ToBrush()
        };

        #endregion

        #region Construct

        public SymColors()
        {}

        public SymColors([NotNull] List<Brush> colorsList)
        {
            _colorsSource = colorsList ?? throw new ArgumentNullException(nameof(colorsList));
        }

        #endregion

        #region Public

        public Brush GetColor(int idx)
        {
            return IsHaveColor(idx) ? _colorsSource[idx] : "Transparent".ToBrush();
        }

        public void SetColor(int idx, Brush brush)
        {
            if (IsHaveColor(idx))
            {
                _colorsSource[idx] = brush;
            }
        }

        public Brush this[int idx]
        {
            get => GetColor(idx);
            set => SetColor(idx, value);
        }

        public int Count => _colorsSource.Count;

        #endregion

        #region Private methods

        private bool IsHaveColor(int idx)
        {
            return _colorsSource.Count > idx && idx >= 0;
        }

        #endregion

        public IEnumerator<Brush> GetEnumerator()
        {
            return _colorsSource.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}