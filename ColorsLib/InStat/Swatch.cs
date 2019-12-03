using System;
using System.Collections.Generic;
using System.Linq;

namespace ColorsLib.InStat
{
    public class Swatch
    {
        public Swatch(string name)
        {
            if (string.Equals(name, null, StringComparison.Ordinal)) throw new ArgumentNullException(nameof(name));

            Hues = new List<Hue>();
            Name = name;
        }

        public void AddHue(Hue hue)
        {
            if (hue == null) throw new ArgumentNullException(nameof(hue));
            Hues.Add(hue);
        }

        public string Name { get; }

        public List<Hue> Hues { get; }

        public bool IsHues => Hues.Any();

        public override string ToString()
        {
            return Name;
        }
    }
}