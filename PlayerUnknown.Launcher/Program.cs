namespace PlayerUnknown.Launcher
{
    using System;
    using System.IO;
    using System.Linq;

    using Gameloop.Vdf;
    using Gameloop.Vdf.JsonConverter;

    using Newtonsoft.Json.Linq;

    public class Program
    {
        /// <summary>
        /// Defines the entry point of the application.
        /// </summary>
        public static void Main()
        {
            string LibraryPath = Program.GetLibraryPath();
            string LibraryFile = File.ReadAllText(LibraryPath);

            if (string.IsNullOrEmpty(LibraryFile) == false)
            {
                VProperty Library = VdfConvert.Deserialize(LibraryFile);
                JProperty Json    = Library.ToJson();
                
                if (Json.Value["1"] != null)
                {
                    string GamesPath = Json.Value["1"].ToObject<string>();

                    if (string.IsNullOrEmpty(GamesPath) == false)
                    {
                        Logging.Info(typeof(Program), "GamesPath : " + GamesPath + ".");
                    }
                    else
                    {
                        Logging.Warning(typeof(Program), "GamesPath property is empty.");
                    }
                }
                else
                {
                    Logging.Warning(typeof(Program), "LibraryFolders property is empty.");
                }
            }
            else
            {
                Logging.Error(typeof(Program), "Empty ?");
            }

            Console.ReadKey(false);
        }

        /// <summary>
        /// Detects the game path.
        /// </summary>
        public static string GetLibraryPath()
        {
            string FilesPath = Environment.GetEnvironmentVariable(Environment.Is64BitOperatingSystem ? "ProgramFiles(x86)" : "ProgramFiles");

            if (Directory.Exists(FilesPath))
            {
                var ProgramFiles = Directory.GetDirectories(FilesPath);

                if (ProgramFiles.Contains("Steam", new PathEndingComparer()))
                {
                    var SteamPath = Path.Combine(FilesPath, "Steam");

                    if (string.IsNullOrEmpty(SteamPath) == false)
                    {
                        var SteamFiles = Directory.GetDirectories(SteamPath);

                        if (SteamFiles.Contains("steamapps", new PathEndingComparer()))
                        {
                            var AppsPath = Path.Combine(SteamPath, "steamapps");

                            if (string.IsNullOrEmpty(AppsPath) == false)
                            {
                                var AppsFiles = Directory.GetFiles(AppsPath);

                                if (AppsFiles.Contains("libraryfolders.vdf", new PathEndingComparer()))
                                {
                                    return Path.Combine(AppsPath, "libraryfolders.vdf");
                                }
                                else
                                {
                                    Logging.Warning(typeof(Program), "LibraryFolders.vdf don't exist.");
                                }
                            }
                            else
                            {
                                Logging.Warning(typeof(Program), "SteamsApps is empty.");
                            }
                        }
                        else
                        {
                            Logging.Warning(typeof(Program), "SteamApps could not be detected.");
                        }
                    }
                    else
                    {
                        Logging.Warning(typeof(Program), "SteamPath is empty.");
                    }
                }
                else
                {
                    Logging.Warning(typeof(Program), "Steam is not installed on this computer.");
                }
            }
            else
            {
                Logging.Warning(typeof(Program), "You don't know how to use a computer, do you?");
            }

            return string.Empty;
        }
    }
}
