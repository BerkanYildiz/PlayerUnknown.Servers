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
                    "-LobbyUrl=http://prod-live-front.playprivategrounds.com/index.html"
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
                        UseShellExecute = false
                    };

                    var Processus   = Process.Start(Executable);
                    var Handle      = Processus.Handle;
                    var SafeHandle  = Processus.SafeHandle;
                    var ProcId      = Processus.Id;

                    Processus.WaitForInputIdle();

                    /*
                    
                    var Memory      = new Memory(Handle, Processus.MainModule.BaseAddress);

                    Logging.Warning(typeof(Program), "Process has been loaded as #" + Handle + ".");
                    Logging.Warning(typeof(Program), "Process base address detected at " + Memory.Base + ".");

                    int Result      = Win32.SetWindowText(Processus.MainWindowHandle, "Rekt niggas");

                    if (Result != 1)
                    {
                        Logging.Error(typeof(Program), "Oh, Damn! Couldn't change the title of PUBG process.");
                    }

                    */

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
