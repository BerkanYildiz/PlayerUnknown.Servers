namespace PlayerUnknown.Launcher
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;

    using Gameloop.Vdf;
    using Gameloop.Vdf.JsonConverter;

    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    using PlayerUnknown.Launcher.Helpers;

    public class Program
    {
        /// <summary>
        /// Gets the parameters.
        /// </summary>
        public static string[] Parameters
        {
            get
            {
                return new[]
                {
                    "-KoreanRating",
                    // "-LobbyUrl=http://prod-live-front.playprivategrounds.com/index.html",
                    "-LobbyUrl=https://prod-live-front.playbattlegrounds.com/index.html"
                };
            }
        }

        /// <summary>
        /// Defines the entry point of the application.
        /// </summary>
        public static void Main()
        {
            if (SteamHelper.IsGameInstalled("PUBG"))
            {
                var ConfigPath    = Path.Combine(SteamHelper.GetConfigPath(), "loginusers.vdf");
                var ConfigFile    = new FileInfo(ConfigPath);

                if (ConfigFile.Exists == false)
                {
                    throw new FileNotFoundException("loginusers.vdf don't exist.");
                }

                var ConfigValue   = File.ReadAllText(ConfigFile.FullName);
                var ConfigUsers   = VdfConvert.Deserialize(ConfigValue).ToJson();
                var Users         = ConfigUsers.Value;

                if (Users.HasValues == false)
                {
                    Logging.Warning(typeof(Program), "Users is empty.");
                    return;
                }

                var MostRecent    = Users.Values().FirstOrDefault(User => User.Value<int>("mostrecent") == 1);

                if (MostRecent == null)
                {
                    Logging.Warning(typeof(Program), "MostRecent is empty.");
                    return;
                }

                var CurrentUser   = new Dictionary<string, object>
                {
                    {
                      "STEAMID", MostRecent.Parent.ToObject<JProperty>().Name
                    },
                    {
                      "SteamUser", MostRecent.Value<string>("AccountName")
                    },
                    {
                      "SteamAppUser", MostRecent.Value<string>("AccountName")
                    },
                    {
                      "SteamAppId", "578080"
                    },
                    {
                      "SteamPath", "C:\\Program Files (x86)\\Steam"
                    }
                };

                Logging.Info(typeof(Program), "Game is installed.");
                Logging.Info(typeof(Program), "Path : " + SteamHelper.GetGamePath("PUBG"));

                var ExecutablePath = Path.Combine(SteamHelper.GetGamePath("PUBG"), @"TslGame\Binaries\Win64\TslGame_EAC.exe");
                var ExecutableFile = new FileInfo(ExecutablePath);

                Logging.Info(typeof(Program), "Executable at " + ExecutablePath + ".");

                if (ExecutableFile.Exists)
                {
                    var Executable = new ProcessStartInfo(ExecutablePath, arguments: null)
                    {
                        UseShellExecute = false
                    };

                    // Steam

                    Executable.EnvironmentVariables.Add("STEAMID",              (string) CurrentUser["STEAMID"]);
                    Executable.EnvironmentVariables.Add("SteamPath",            (string) CurrentUser["SteamPath"]);
                    Executable.EnvironmentVariables.Add("SteamUser",            (string) CurrentUser["SteamUser"]);
                    Executable.EnvironmentVariables.Add("SteamAppUser",         (string) CurrentUser["SteamAppUser"]);
                    Executable.EnvironmentVariables.Add("SteamGameId",          (string) CurrentUser["SteamAppId"]);
                    Executable.EnvironmentVariables.Add("SteamAppId",           (string) CurrentUser["SteamAppId"]);
                    Executable.EnvironmentVariables.Add("SteamControllerAppId", (string) CurrentUser["SteamAppId"]);

                    // Extras

                    Executable.EnvironmentVariables.Add("ENABLE_VK_LAYER_VALVE_steam_overlay_1", "1");
                    Executable.EnvironmentVariables.Add("SDL_GAMECONTROLLER_ALLOW_STEAM_VIRTUAL_GAMEPAD", "1");
                    Executable.EnvironmentVariables.Add("EnableConfiguratorSupport", "0");

                    // Streaming

                    Executable.EnvironmentVariables.Add("SteamStreamingHardwareEncodingNVIDIA", "1");
                    Executable.EnvironmentVariables.Add("SteamStreamingHardwareEncodingAMD", "1");
                    Executable.EnvironmentVariables.Add("SteamStreamingHardwareEncodingIntel", "1");

                    var Processus   = Process.Start(Executable);

                    // Started !

                    var Handle      = Processus.Handle;
                    var SafeHandle  = Processus.SafeHandle;
                    var ProcId      = Processus.Id;

                    Processus.WaitForInputIdle();

                    bool BEnabled   = Processus.Modules.Cast<ProcessModule>().Any(Module => Module.ModuleName.StartsWith("BattlEye"));

                    if (BEnabled)
                    {
                        Logging.Warning(typeof(Program), "BattlEye has been detected, don't move!");
                    }
                }
                else
                {
                    Logging.Warning(typeof(Program), "Executable not found.");
                }
            }
            else
            {
                Logging.Warning(typeof(Program), "Game not found.");
            }

            Console.ReadKey(false);
        }
    }
}
