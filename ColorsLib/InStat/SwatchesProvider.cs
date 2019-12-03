#region

using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Media;

#endregion

namespace ColorsLib.InStat
{
    public class SwatchesProvider
    {
        #region Construct

        public SwatchesProvider(Assembly assembly)
        {
            var resourcesName = assembly.GetName().Name + ".g";
            var manager = new ResourceManager(resourcesName, assembly);
            var resourceSet = manager.GetResourceSet(CultureInfo.CurrentUICulture, true, true);
            var dictionaryEntries = resourceSet.OfType<DictionaryEntry>().ToList();
            var assemblyName = assembly.GetName().Name;

            var regex = new Regex(@"instat\/colors\/(?<fileName>[a-z]+).baml$");

            IGrouping<string, SelBaml>[] groupRes = dictionaryEntries
                .Select(x => new SelBaml {key = x.Key.ToString(), match = regex.Match(x.Key.ToString())})
                .Where(x => x.match.Success)
                .GroupBy(x => x.match.Groups["fileName"].Value).ToArray();

            Swatch classicColors = GetClassicColorsList(groupRes, assemblyName);
            Swatches = GetSwatches(classicColors, groupRes, assemblyName);
        }

        public SwatchesProvider() : this(Assembly.GetExecutingAssembly())
        { }

        #endregion

        #region Public

        public List<Swatch> Swatches { get; }

        #endregion

        #region Private methods

        /// <summary>
        /// Загружает цвета из baml ресурса
        /// </summary>
        /// <param name="res"></param>
        /// <returns></returns>
        private static Dictionary<string, Color> GetColors(ResourceDictionary res)
        {
            //todo: добавить проверку имени цвета на соотвествие шалблону <name>_<C><b>

            Dictionary<string, object> colr = new Dictionary<string, object>();

            foreach (var item in res.OfType<DictionaryEntry>())
            {
                if (item.Key is ComponentResourceKey component)
                {
                    string keyName = component.ResourceId.ToString();

                    if (colr.ContainsKey(keyName))
                        throw new Exception("Key already exists:" + keyName + " in " + res.Source);

                    colr.Add(component.ResourceId.ToString(), item.Value);
                }
                else if (item.Key is string color)
                {
                    colr.Add(color, item.Value);
                }
            }

            var dic = colr.OrderBy(de => de.Key.ToString()).ToList();
            var resColors = dic.Where(x => x.Value.GetType().FullName == typeof(Color).FullName).ToList();
            var resSolidColors = dic.Where(x => x.Value.GetType().FullName == typeof(SolidColorBrush).FullName).ToList();

            Dictionary<string, Color> colors = new Dictionary<string, Color>();

            foreach (var item in resColors)
            {
                if (!colors.ContainsKey(item.Key.ToString()))
                {
                    colors.Add(item.Key.ToString(), (Color) item.Value);
                }
            }

            foreach (var item in resSolidColors)
            {
                var strKey = item.Key.ToString();
                string str = strKey.Remove(strKey.Length - 1, 1);

                if (!colors.ContainsKey(str))
                {
                    colors.Add(str, ((SolidColorBrush) item.Value).Color);
                }
            }

            return colors;
        }

        /// <summary>
        /// Загружает цвета по имени файла baml ресурса
        /// </summary>
        /// <param name="name"></param>
        /// <param name="groupRes"></param>
        /// <param name="assemblyName"></param>
        /// <returns></returns>
        private static ResourceDictionary GetResourceDictionaryBaml(string name, IEnumerable<IGrouping<string, SelBaml>> groupRes,
            string assemblyName)
        {
            foreach (IEnumerable<SelBaml> item in groupRes)
            {
                SelBaml selBaml = item.First();

                if (String.Equals(selBaml.match.Groups["fileName"].Value, name, StringComparison.CurrentCultureIgnoreCase))
                {
                    var res = Read(assemblyName, selBaml.key);
                    return res;
                }
            }

            return null;
        }
        
        /// <summary>
        /// Загружает ресурсы
        /// </summary>
        /// <param name="assemblyName"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        private static ResourceDictionary Read(string assemblyName, string path)
        {
            if (assemblyName == null || path == null)
                return null;

            return (ResourceDictionary) Application.LoadComponent(new Uri(
                $"/{assemblyName};component/{path.Replace(".baml", ".xaml")}",
                UriKind.RelativeOrAbsolute));
        }
        
        /// <summary>
        /// Создает наборы цветов и проверят их с класическими цветами
        /// </summary>
        /// <param name="classicColors"></param>
        /// <param name="groupRes"></param>
        /// <param name="assemblyName"></param>
        /// <returns></returns>
        private List<Swatch> GetSwatches(Swatch classicColors, IGrouping<string, SelBaml>[] groupRes, string assemblyName)
        {
            List<Swatch> swatches = new List<Swatch>();

            foreach (IEnumerable<SelBaml> grp in groupRes)
            {
                SelBaml selBaml = grp.First();
                var name = selBaml.match.Groups["fileName"].Value;

                if (!string.Equals(name, classicColors.Name, StringComparison.CurrentCultureIgnoreCase))
                {
                    var res = GetResourceDictionaryBaml(name, groupRes, assemblyName);

                    if (res == null)
                    {
                        throw new Exception(name + " not found");
                    }

                    if (res.Count == 0)
                    {
                        continue;
                        // throw new Exception(name + " is empty");
                    }

                    if (res.Count > 0)
                    {
                        Dictionary<string, Color> colors = GetColors(res);

                        Swatch swatch = new Swatch(name);
                        swatches.Add(swatch);
                        foreach (Hue hue in classicColors.Hues)
                        {
                            if (!colors.ContainsKey(hue.Name))
                            {
                                swatch.AddHue(new Hue(hue.Name, hue.Color));
                            }
                            else
                            {
                                swatch.AddHue(new Hue(hue.Name, colors[hue.Name]));
                            }
                        }
                    }
                }
            }

            return swatches;
        }

        /// <summary>
        /// Загружает ClassicColors цвета, как базовые для разных цветовых тем
        /// </summary>
        /// <param name="groupRes"></param>
        /// <param name="assemblyName"></param>
        /// <returns></returns>
        private Swatch GetClassicColorsList(IGrouping<string, SelBaml>[] groupRes, string assemblyName)
        {
            string name = "ClassicColors";
            var res = GetResourceDictionaryBaml(name, groupRes, assemblyName);

            if (res == null)
            {
                throw new Exception("ClassicColors.xaml not found");
            }

            if (res.Count == 0)
            {
                throw new Exception("ClassicColors.xaml is empty");
            }

            if (res.Count > 0)
            {
                Dictionary<string, Color> colors = GetColors(res);

                Swatch swatch = new Swatch(name);
                foreach (KeyValuePair<string, Color> pair in colors)
                {
                    swatch.AddHue(new Hue(pair.Key, pair.Value));
                }

                return swatch;
            }

            return null;
        }

        #endregion

        #region Struct

        private struct SelBaml
        {
            public string key;
            public Match match;
        }

        #endregion
    }
}