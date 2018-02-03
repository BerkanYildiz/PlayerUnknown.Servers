namespace PlayerUnknown
{
    using System;
    using System.Diagnostics;

    using PlayerUnknown.Enums;
    using PlayerUnknown.Events;
    using PlayerUnknown.Exceptions;
    using PlayerUnknown.Helpers;

    public static class PUBG
    {
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="PUBG"/> is initiliazed.
        /// </summary>
        public static bool Initialized
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the attached <see cref="PUBG"/> process.
        /// </summary>
        public static Process AttachedProcess
        {
            get
            {
                if (PUBG._AttachedProcess != null)
                {
                    if (PUBG._AttachedProcess.HasExited == false)
                    {
                        return PUBG._AttachedProcess;
                    }
                }

                return null;
            }
        }

        /// <summary>
        /// Gets a value indicating whether <see cref="PUBG"/> is attached.
        /// </summary>
        public static bool IsAttached
        {
            get
            {
                return PUBG.AttachedProcess != null;
            }
        }

        /// <summary>
        /// Gets a value indicating whether <see cref="PUBG"/> is detached.
        /// </summary>
        public static bool IsDetached
        {
            get
            {
                return PUBG.AttachedProcess == null;
            }
        }

        /// <summary>
        /// Gets a value indicating whether <see cref="PUBG"/> is running.
        /// </summary>
        public static bool IsRunning
        {
            get
            {
                if (PUBG.AttachedProcess != null)
                {
                    if (PUBG._AttachedProcess.HasExited == false)
                    {
                        return true;
                    }
                }

                return false;
            }
        }

        /// <summary>
        /// Gets a value indicating whether <see cref="PUBG"/> is responding.
        /// </summary>
        public static bool IsResponding
        {
            get
            {
                if (PUBG.AttachedProcess != null)
                {
                    return PUBG._AttachedProcess.Responding;
                }

                return false;
            }
        }

        /// <summary>
        /// Gets a value indicating whether <see cref="PUBG"/> is minimized.
        /// </summary>
        public static bool IsMinimized
        {
            get
            {
                if (PUBG.AttachedProcess != null)
                {
                    var Placement = Win32.GetWindowPlacement(PUBG._AttachedProcess.MainWindowHandle);

                    if (Placement.ShowCmd == WindowStates.Hide || Placement.ShowCmd == WindowStates.Minimize)
                    {
                        return true;
                    }
                }

                return false;
            }
        }

        /// <summary>
        /// Gets a value indicating whether <see cref="PUBG"/> is maximized.
        /// </summary>
        public static bool IsMaximized
        {
            get
            {
                if (PUBG.AttachedProcess != null)
                {
                    var Placement = Win32.GetWindowPlacement(PUBG._AttachedProcess.MainWindowHandle);

                    if (Placement.ShowCmd == WindowStates.Maximize || Placement.ShowCmd == WindowStates.ShowMaximized)
                    {
                        return true;
                    }
                }

                return false;
            }
        }

        /// <summary>
        /// Gets a value indicating whether <see cref="PUBG"/> is displayed on the screen.
        /// </summary>
        public static bool IsOnScreen
        {
            get
            {
                if (PUBG.AttachedProcess != null)
                {
                    var Placement = Win32.GetWindowPlacement(PUBG._AttachedProcess.MainWindowHandle);
                    var Flag      = Placement.ShowCmd;

                    if (PUBG.IsMaximized)
                    {
                        return true;
                    }

                    if (Flag == WindowStates.Restore || Flag == WindowStates.Show || Flag == WindowStates.ShowNormal || Flag == WindowStates.ShowDefault)
                    {
                        return true;
                    }
                }

                return false;
            }
        }

        private static Process _AttachedProcess;

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        public static void Initialize()
        {
            if (PUBG.Initialized)
            {
                return;
            }

            EventHandlers.Run();

            PUBG.Initialized = true;
        }

        /// <summary>
        /// Attaches this instance to <see cref="PUBG"/>.
        /// </summary>
        public static void Attach()
        {
            var Processes = Process.GetProcessesByName("TslGame");
            var Processus = (Process) null;

            if (Processes.Length == 0)
            {
                // throw new ProcessNotFoundException("Processes.Length == 0 at PUBG.Attach().");
            }
            else
            {
                if (Processes.Length > 1)
                {
                    Logging.Info(typeof(PUBG), "Processes.Length > 1 at PUBG.Attach().");

                    foreach (var Match in Processes)
                    {
                        // Get the correct instance.
                    }
                }
                else
                {
                    Processus = Processes[0];
                }
            }

            if (Processus == null)
            {
                // throw new ProcessNotFoundException("Processus == null at PUBG.Attach().");
            }
            else
            {
                PUBG._AttachedProcess = Processus;
            }
        }

        /// <summary>
        /// Detaches this instance to <see cref="PUBG"/>.
        /// </summary>
        public static void Detach()
        {
            if (PUBG._AttachedProcess == null)
            {
                Logging.Info(typeof(PUBG), "_AttachedProcess == null at PUBG.Detach().");
            }

            PUBG._AttachedProcess = null;
        }
    }
}
