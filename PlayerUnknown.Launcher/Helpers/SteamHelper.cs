namespace PlayerUnknown.Launcher.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    using Gameloop.Vdf;
    using Gameloop.Vdf.JsonConverter;

    using Newtonsoft.Json.Linq;

    using PlayerUnknown.Helpers.Comparers;

    public static class SteamHelper
    {
        /// <summary>
        /// Detects the steam path.
        /// </summary>
        public static string GetSteamPath()
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
                        return SteamPath;
                    }
                    else
                    {
                        Logging.Warning(typeof(SteamHelper), "SteamPath is empty.");
                    }
                }
                else
                {
                    Logging.Warning(typeof(SteamHelper), "Steam is not installed on this computer.");
                }
            }
            else
            {
                Logging.Warning(typeof(SteamHelper), "You don't know how to use a computer, do you?");
            }

            return string.Empty;
        }

        /// <summary>
        /// Detects the steam apps path.
        /// </summary>
        public static string GetSteamAppsPath()
        {
            var SteamPath = SteamHelper.GetSteamPath();

            if (string.IsNullOrEmpty(SteamPath) == false)
            {
                var SteamFiles = Directory.GetDirectories(SteamPath);

                if (SteamFiles.Contains("steamapps", new PathEndingComparer()))
                {
                    return Path.Combine(SteamPath, "steamapps");
                }
                else
                {
                    Logging.Warning(typeof(SteamHelper), "SteamApps could not be detected.");
                }
            }
            else
            {
                Logging.Warning(typeof(SteamHelper), "SteamPath is empty.");
            }

            return string.Empty;
        }

        /// <summary>
        /// Detects the steam games library path.
        /// </summary>
        public static string GetLibraryPath()
        {
            var AppsPath = SteamHelper.GetSteamAppsPath();

            if (string.IsNullOrEmpty(AppsPath) == false)
            {
                var AppsFiles = Directory.GetFiles(AppsPath);

                if (AppsFiles.Contains("libraryfolders.vdf", new PathEndingComparer()))
                {
                    return Path.Combine(AppsPath, "libraryfolders.vdf");
                }
                else
                {
                    Logging.Warning(typeof(SteamHelper), "LibraryFolders.vdf don't exist.");
                }
            }
            else
            {
                Logging.Warning(typeof(SteamHelper), "SteamApps is empty.");
            }

            return string.Empty;
        }

        /// <summary>
        /// Detects the steam config path.
        /// </summary>
        public static string GetConfigPath()
        {
            var SteamPath = SteamHelper.GetSteamPath();

            if (string.IsNullOrEmpty(SteamPath) == false)
            {
                return Path.Combine(SteamPath, "config");
            }
            else
            {
                Logging.Warning(typeof(SteamHelper), "SteamPath is empty.");
            }

            return string.Empty;
        }

        /// <summary>
        /// Gets the games folder path.
        /// </summary>
        public static IEnumerable<string> GetGamesFolderPaths()
        {
            string SteamPath   = SteamHelper.GetSteamAppsPath();
            string LibraryPath = SteamHelper.GetLibraryPath();
            string LibraryFile = File.ReadAllText(LibraryPath);

            if (string.IsNullOrEmpty(LibraryFile) == false)
            {
                VProperty Library = VdfConvert.Deserialize(LibraryFile);
                JProperty Json = Library.ToJson();

                if (Json.Value["1"] != null)
                {
                    string GamesPath = Json.Value["1"].ToObject<string>();

                    if (string.IsNullOrEmpty(GamesPath) == false)
                    {
                        if (string.IsNullOrEmpty(GamesPath) == false)
                        {
                            string CommonPath = Path.Combine(GamesPath, "steamapps\\common\\");

                            if (string.IsNullOrEmpty(CommonPath) == false)
                            {
                                yield return CommonPath;
                            }
                            else
                            {
                                Logging.Warning(typeof(SteamHelper), "CommonPath is empty.");
                            }
                        }
                        else
                        {
                            Logging.Warning(typeof(SteamHelper), "GamesPath is empty.");
                        }
                    }
                    else
                    {
                        Logging.Warning(typeof(SteamHelper), "GamesPath property is empty.");
                    }
                }
                else
                {
                    Logging.Warning(typeof(SteamHelper), "LibraryFolders property is empty.");
                }
            }
            else
            {
                Logging.Error(typeof(SteamHelper), "Empty ?");
            }

            yield return Path.Combine(SteamPath, "common");
        }

        /// <summary>
        /// Gets the installed games.
        /// </summary>
        public static IEnumerable<string> GetInstalledGames()
        {
            var GameFolders = SteamHelper.GetGamesFolderPaths().ToList();

            if (GameFolders.Any())
            {
                foreach (var GameFolder in GameFolders)
                {
                    var Games = Directory.GetDirectories(GameFolder);

                    foreach (var Game in Games)
                    {
                        int LastSlash = Game.LastIndexOf('\\') + 1;

                        if (LastSlash != 0)
                        {
                            yield return Game.Substring(LastSlash, Game.Length - LastSlash);
                        }
                    }
                }
            }
            else
            {
                Logging.Warning(typeof(SteamHelper), "GameFolders is empty.");
            }
        }

        /// <summary>
        /// Determines whether the specified game is installed.0
        /// </summary>
        /// <param name="GameName">Name of the game.</param>
        public static bool IsGameInstalled(string GameName)
        {
            foreach (var Game in SteamHelper.GetInstalledGames())
            {
                Logging.Info(typeof(SteamHelper), " - " + Game);
            }

            return SteamHelper.GetInstalledGames().Contains(GameName);
        }

        /// <summary>
        /// Gets the game path.
        /// </summary>
        /// <param name="GameName">Name of the game.</param>
        public static string GetGamePath(string GameName)
        {
            if (SteamHelper.IsGameInstalled(GameName))
            {
                var GameFolders = SteamHelper.GetGamesFolderPaths().ToList();

                if (GameFolders.Any())
                {
                    foreach (var GameFolder in GameFolders)
                    {
                        var Games = Directory.GetDirectories(GameFolder);

                        if (Games.Contains(GameName, new PathEndingComparer()))
                        {
                            return Path.Combine(GameFolder, GameName);
                        }
                    }
                }
                else
                {
                    Logging.Warning(typeof(SteamHelper), "GameFolders is empty.");
                }
            }
            else
            {
                Logging.Warning(typeof(SteamHelper), "Game is not installed.");
            }

            return string.Empty;
        }
    }
}
