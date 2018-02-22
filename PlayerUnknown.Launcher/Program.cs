namespace PlayerUnknown.Launcher
{
    using System;
    using System.Diagnostics;
    using System.IO;

    using PlayerUnknown.Helpers;

    public class Program
    {
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
                    var Executable = new ProcessStartInfo(ExecutablePath, "-LobbyUrl=http://prod-live-front.playprivategrounds.com/index.html")
                    {
                        UseShellExecute = false
                    };

                    var Processus   = Process.Start(Executable);
                    var Handle      = Processus.Handle;
                    var SafeHandle  = Processus.SafeHandle;
                    var ProcId      = Processus.Id;

                    Processus.WaitForInputIdle();
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
