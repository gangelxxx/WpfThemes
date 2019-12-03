using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ColorsXamlSync
{
    class Program
    {
        static void Main(string[] args)
        {
            string path = args[0];

            List<Tuple<string, string>> listFiles = GetListFile(path);
        }

        private static List<Tuple<string, string>> GetListFile(string path)
        {
            DirectoryInfo d = new DirectoryInfo(path);
            FileInfo[] files = d.GetFiles("*.xaml");
            List<Tuple<string, string>> resfiles = new List<Tuple<string, string>>();

            foreach (FileInfo file in files)
            {
                string pattern = @"(?<name>.+)Colors\\.xaml";
                string fileName = file.Name;

                var reg = new Regex(pattern);
                var match = reg.Match(fileName);

                if (match.Success)
                {
                    var pathToFile = Path.Combine(path, fileName);
                    if (File.Exists(pathToFile))
                    {
                        var name = match.Groups["name"].Value;
                        resfiles.Add(new Tuple<string, string>(name, pathToFile));
                    }
                }
            }

            return resfiles;
        }
    }
}
