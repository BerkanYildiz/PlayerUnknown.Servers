namespace PlayerUnknown.Launcher
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;

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
                Logging.Info(typeof(Program), "Game is installed.");
                Logging.Info(typeof(Program), "Path : " + SteamHelper.GetGamePath("PUBG"));

                var ExecutablePath = Path.Combine(SteamHelper.GetGamePath("PUBG"), @"TslGame\Binaries\Win64\TslGame.exe");
                var ExecutableFile = new FileInfo(ExecutablePath);

                Logging.Info(typeof(Program), "Executable at " + ExecutablePath + ".");

                if (ExecutableFile.Exists)
                {
                    var Executable = new ProcessStartInfo(ExecutablePath, string.Join(" ", Parameters))
                    {
                        UseShellExecute = false,
                    };

                    // Steam

                    Executable.EnvironmentVariables.Add("STEAMID", "76561198826798479");
                    Executable.EnvironmentVariables.Add("SteamPath", "C:\\Program Files (x86)\\Steam");
                    Executable.EnvironmentVariables.Add("SteamUser", "sukhrab4");
                    Executable.EnvironmentVariables.Add("SteamGameId", "578080");
                    Executable.EnvironmentVariables.Add("SteamAppId", "578080");
                    Executable.EnvironmentVariables.Add("SteamAppUser", "sukhrab4");
                    Executable.EnvironmentVariables.Add("SteamControllerAppId", "578080");

                    // Extra

                    Executable.EnvironmentVariables.Add("ENABLE_VK_LAYER_VALVE_steam_overlay_1", "1");
                    Executable.EnvironmentVariables.Add("SDL_GAMECONTROLLER_ALLOW_STEAM_VIRTUAL_GAMEPAD", "1");
                    Executable.EnvironmentVariables.Add("EnableConfiguratorSupport", "0");

                    // Streaming

                    Executable.EnvironmentVariables.Add("SteamStreamingHardwareEncodingNVIDIA", "1");
                    Executable.EnvironmentVariables.Add("SteamStreamingHardwareEncodingAMD", "1");
                    Executable.EnvironmentVariables.Add("SteamStreamingHardwareEncodingIntel", "1");

                    var Processus   = Process.Start(Executable);
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
