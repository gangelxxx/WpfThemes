#region

using System;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;
using ColorsLib.InStat;

#endregion

namespace ThemesLib.InStat.Helpers
{
    public static class PaletteHelper
    {
        #region Public

        public static void ChangePallete(PalettesEnumeration palettesEnumeration, SwatchesProvider swatchesProvider)
        {
            var namePalette = Enum.GetName(typeof(PalettesEnumeration), palettesEnumeration);
            var swatch = swatchesProvider.Swatches.FirstOrDefault(x =>
                namePalette != null && x.Name.ToLower().Contains(namePalette.ToLower()));

            if (swatch == null) return;

            foreach (ResourceDictionary res in Application.Current.Resources.MergedDictionaries)
            {
                if (res.Count > 0)
                {
                    foreach (var hue in swatch.Hues)
                    {
                        ReplaceEntry(hue.Name, hue.Color, res);
                    }
                }
            }
        }

        #endregion

        #region Private methods

        private static void ReplaceEntry(string entryName, Color newValue, ResourceDictionary parentDictionary = null)
        {
            if (parentDictionary == null)
                parentDictionary = Application.Current.Resources;

            string nameC = entryName;
            string nameCb = entryName + "b";

            if (parentDictionary.Contains(nameC))
            {
                parentDictionary[nameC] = newValue;
            }

            if (parentDictionary.Contains(nameCb))
            {
                var newValueCb = new SolidColorBrush(newValue);
                if (parentDictionary[nameCb] is SolidColorBrush brush && !brush.IsFrozen)
                {
                    var animation = new ColorAnimation
                    {
                        From = ((SolidColorBrush) parentDictionary[nameCb]).Color,
                        To = newValueCb.Color,
                        Duration = new Duration(TimeSpan.FromMilliseconds(300))
                    };
                    brush.BeginAnimation(SolidColorBrush.ColorProperty, animation);
                }
                else
                    parentDictionary[nameCb] = newValueCb;
            }

            foreach (var dictionary in parentDictionary.MergedDictionaries)
                ReplaceEntry(entryName, newValue, dictionary);
        }

        #endregion
    }
}