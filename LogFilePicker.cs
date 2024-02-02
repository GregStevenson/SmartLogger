using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;

namespace SmartLogger
{
    public class LogFilePicker
    {

        public static string ConfigFile(string targetExe)
        {
            Bootstrap.Log.LogDebug($"SmartInspect Package: {Assembly.GetExecutingAssembly().FullName}");
            string configExtension = ".lgcfg";
            if (string.IsNullOrEmpty(targetExe))
            {
              targetExe = Process.GetCurrentProcess().MainModule.FileName;
            }

            Bootstrap.Log.LogString("Target Application", targetExe);
            string filename = System.IO.Path.GetFileNameWithoutExtension(targetExe);
            Bootstrap.Log.LogString("filename", filename);
            string folderPath = System.IO.Path.GetDirectoryName(targetExe);
            Bootstrap.Log.LogString("fullPath", folderPath);
            string exeConfigFile;
            exeConfigFile = $"{folderPath}\\{filename}{configExtension}";
            Bootstrap.Log.LogString("EXE Config File", exeConfigFile);
            if (File.Exists(exeConfigFile))
            {
                Bootstrap.Log.LogColored(Color.SkyBlue,"EXE Config File exists");
                return exeConfigFile;

            }
            Bootstrap.Log.LogColored(Color.Goldenrod, "Looking for ProgramData location");
            string dataConfigFile;
            string dataPath = LocateDataFolder(targetExe);
            Bootstrap.Log.LogString("dataPath", dataPath);
            dataConfigFile = $"{dataPath}\\{filename}{configExtension}";
            Bootstrap.Log.LogString("DataPath Config File", dataConfigFile);
            if (File.Exists(dataConfigFile))
            {
                Bootstrap.Log.LogColored(Color.PowderBlue, "ProgramData location File exists");
                return dataConfigFile;

            }
            Bootstrap.Log.LogColored(Color.DarkOrange, "Config file not found");
            return exeConfigFile;  // path portion will be used for default type
        }


        public static string LocateDataFolder(string targetExe)
        {
            string dataFolder = string.Empty;
            string bestLogConfig = string.Empty;
            // Get path of exe
            Bootstrap.Log.LogDebug($"Locating Data Folder for: {targetExe}");
            string exePath = Path.GetDirectoryName(targetExe) + @"\";  // this will preserve the previous path
            string programFiles86Path = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86).ToString().ToLower();
            string programFilesPath = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles).ToString().ToLower();
            string binFolder = @"bin\";
            if (exePath.ToLower().StartsWith(programFilesPath))
            {
                dataFolder = exePath.Remove(0, programFilesPath.Length+1);
                Bootstrap.Log.LogColored(Color.Tan, $"ProgFiles Path: {dataFolder}");
            }
            else if (exePath.ToLower().StartsWith(programFiles86Path))
            {
                dataFolder = exePath.Remove(0, programFiles86Path.Length+1);
                Bootstrap.Log.LogColored(Color.Tan, $"ProgFiles86 Path: {dataFolder}");
            }
            Bootstrap.Log.LogColored(Color.Brown,$"dataFolder after Prog Files check: {dataFolder}");
            if (!string.IsNullOrEmpty(dataFolder))
            {
                string rootDataFolder = Environment.GetFolderPath(Environment.SpecialFolder.CommonProgramFiles);
                
                dataFolder = Path.Combine(rootDataFolder, dataFolder);
                if (dataFolder.ToLower().EndsWith(binFolder))
                {
                    dataFolder = exePath.Remove(exePath.Length-binFolder.Length, binFolder.Length);
                }
                            }
            else
            {
                dataFolder = exePath;
            }
            Bootstrap.Log.LogWarning($"dataFolder after checks: {dataFolder}");
            return dataFolder;
        }


    }
}
