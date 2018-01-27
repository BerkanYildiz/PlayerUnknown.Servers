namespace PlayerUnknown
{
    using System;
    using System.Diagnostics;

    using PlayerUnknown.Exceptions;
    using PlayerUnknown.Reader;

    public class PUBG
    {
        /// <summary>
        /// Gets a value indicating whether this instance is attached.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is attached; otherwise, <c>false</c>.
        /// </value>
        public bool IsAttached
        {
            get
            {
                return this.AttachedProcess != null;
            }
        }

        /// <summary>
        /// Gets the attached <see cref="PUBG"/> process.
        /// </summary>
        public Process AttachedProcess
        {
            get
            {
                if (this._AttachedProcess != null)
                {
                    if (this._AttachedProcess.HasExited == false)
                    {
                        return this._AttachedProcess;
                    }
                }

                return null;
            }
        }

        private Process _AttachedProcess;

        /// <summary>
        /// Initializes a new instance of the <see cref="PUBG"/> class.
        /// </summary>
        public PUBG()
        {
            // PUBG.
        }

        /// <summary>
        /// Attaches this instance to <see cref="PUBG"/>.
        /// </summary>
        public void Attach()
        {
            var Processes = Process.GetProcessesByName("notepad++");
            var Processus = (Process) null;

            if (Processes.Length == 0)
            {
                throw new ProcessNotFoundException("Processes.Length == 0 at PUBG.Attach().");
            }

            if (Processes.Length > 1)
            {
                Logging.Info(this.GetType(), "Processes.Length > 1 at PUBG.Attach().");

                foreach (var Match in Processes)
                {
                    // Get the correct instance.
                }
            }
            else
            {
                Processus = Processes[0];
            }

            if (Processus == null)
            {
                throw new ProcessNotFoundException("Processes.Length == 0 at PUBG.Attach().");
            }
            else
            {
                this._AttachedProcess = Processus;
            }

            Logging.Info(this.GetType(), "Process has been attached.");
        }

        /// <summary>
        /// Detaches this instance to <see cref="PUBG"/>.
        /// </summary>
        public void Detach()
        {
            if (this._AttachedProcess == null)
            {
                Logging.Info(this.GetType(), "_AttachedProcess == null at PUBG.Detach().");
            }

            this._AttachedProcess = null;
        }

        /// <summary>
        /// Writes the specified message to the attached <see cref="Process"/>.
        /// </summary>
        public void Write(string Message)
        {
            if (this.IsAttached == false)
            {
                return;
            }

            if (string.IsNullOrEmpty(Message))
            {
                return;
            }

            using (var Hooker = new BattleGroundMemory(Process.GetCurrentProcess()))
            {
                foreach (var Window in Hooker.Windows.RemoteWindows)
                {
                    if (Window.IsActivated)
                    {
                        Window.Keyboard.Write(Message);
                    }
                }
            }

            Logging.Info(this.GetType(), "Message written to the attached process.");
        }
    }
}
