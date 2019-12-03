using System.Collections.Generic;
using System.Windows.Media;

namespace ThemesLib.InStat.Helpers
{
    public interface ISymColors : IEnumerable<Brush>
    {
        Brush GetColor(int idx);
        void SetColor(int idx, Brush brush);
        int Count { get; }
        Brush this[int idx] { get; set; }
    }
}