using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace CreateNugetPacket
{
    class Program
    {
        private static string PATH_TO_THEMES_LIB_DEBUG =
            "d:\\programming\\InStat\\Base_ThemesControls\\GenericThemes\\ThemesLib\\bin\\Debug\\";
        private static string PATH_TO_DESTINATION =
            "d:\\nuget_base\\create\\ThemesLib\\lib\\Themes\\";
        private static string WORKING_DIR =
            "d:\\nuget_base\\create\\ThemesLib\\";

        static void Main(string[] args)
        {
            // if (args.Length == 3)
            // {
            //     PATH_TO_THEMES_LIB_DEBUG = 
            // }

            try
            {
                string[] files = Directory.GetFiles(PATH_TO_THEMES_LIB_DEBUG, "*.*");

                foreach (string fullName in files)
                {
                    string libName = Path.GetFileName(fullName);
                    FileInfo sourceFile = new FileInfo(Path.Combine(PATH_TO_THEMES_LIB_DEBUG, libName));
                    FileInfo destinationFile = new FileInfo(Path.Combine(PATH_TO_DESTINATION, libName));

                    File.Copy(sourceFile.FullName, destinationFile.FullName, true);
                }

                UpdateBuildNuSpec();
                RunCmd("nuget.exe", "pack", WORKING_DIR);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private static void UpdateBuildNuSpec()
        {
            string[] files = Directory.GetFiles(WORKING_DIR, "*.nuspec");

            if (files.Length == 0)
            {
                throw new Exception("File *.nuspec not found");
            }

            if (files.Length != 1)
            {
                throw new Exception("File *.nuspec must be single in working dir");
            }

            string fileName = files[0];

            string[] lines = File.ReadAllLines(fileName);

            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i];
                if (line.Contains("version"))
                {
                    Match regex = Regex.Match(line, @"(?<leftStr>\<version\>\d{1,}\.\d{1,}\.\d{1,}\.)(?<build>\d{1,})");
                    if (regex.Success)
                    {
                        string leftStr = regex.Groups["leftStr"].Value;
                        int build = Convert.ToInt32(regex.Groups["build"].Value);
                        string res = leftStr + (++build) + "</version>";
                        lines[i] = res;
                        break;
                    }
                }
            }

            File.WriteAllLines(fileName, lines);
        }

        public static void RunCmd(string exeName, string command, string workingDir = "")
        {

            Process process = new Process();

            var startInfo = new ProcessStartInfo(exeName);
            startInfo.RedirectStandardOutput = true;
            startInfo.RedirectStandardError = true;
            startInfo.CreateNoWindow = true;
            startInfo.UseShellExecute = false;
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            startInfo.Arguments = command;
            startInfo.WorkingDirectory = workingDir;

            process.StartInfo = startInfo;
            process.Start();

            string output = process.StandardOutput.ReadToEnd();
            Debug.WriteLine(output);
            string err = process.StandardError.ReadToEnd();
            Debug.WriteLine(err);
            process.WaitForExit();
        }
    }
}
