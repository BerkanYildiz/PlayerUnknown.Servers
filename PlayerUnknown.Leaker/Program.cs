namespace PlayerUnknown.Leaker
{
    using System;

    using PlayerUnknown.Leaker.Imports;
    using PlayerUnknown.Leaker.Proc;
    using PlayerUnknown.Leaker.Service;

    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            CProcess CurrentProcess    = new CProcess();
            CProcess TargetProcess     = new CProcess(Options.TargetProcess);

            Logging.Info(typeof(Program), "Leaker has been configured.");

            CurrentProcess.SetPrivilege("SeDebugPrivilege", true);
            CurrentProcess.SetPrivilege("SeTcbPrivilege", true);

            Logging.Info(typeof(Program), "Waiting for a few seconds..");

            TargetProcess.Wait(Options.DelayToWait);

            Logging.Info(typeof(Program), "Resuming..");

            if (TargetProcess.IsValidProcess())
            {
                Logging.Info(typeof(Program), "The target process is valid.");

                var Handles = Service.Service.ServiceEnumHandles(TargetProcess.GetPid(), Options.DesiredAccess);

                Logging.Info(typeof(Program), "We've detected " + Handles.Count + " processes : ");
                
                foreach (HandleInfo HandleInfo in Handles)
                {
                    Logging.Info(typeof(Program), " - " + HandleInfo.Pid + ".");
                    
                    if (HandleInfo.Pid == Kernel32.GetCurrentProcessId())
                    {
                        Logging.Info(typeof(Program), "The detected process was PlayerUnknown.Leaker");
                        continue;
                    }

                    var ServProcess = new CProcess(HandleInfo.Pid);

                    IntPtr HProcess;

                    if (Service.Service.ServiceSetHandleStatus(ServProcess, HandleInfo.HProcess, true, true))
                    {
                        // HProcess = Service.Service.ServiceStartProcess(null, Directory.GetCurrentDirectory() + "\\" + Options.YourProcess + " " + HandleInfo.HProcess, null, true, ServProcess.GetHandle());
                        Service.Service.ServiceSetHandleStatus(ServProcess, HandleInfo.HProcess, false, false);
                        // Kernel32.CloseHandle(HProcess);
                    }

                    // ServProcess.Close();
                }

                // TargetProcess.Close();
            }
            else
            {
                Logging.Warning(typeof(Program), "The target process is not valid.");
            }

            CurrentProcess.SetPrivilege("SeDebugPrivilege", false);
            CurrentProcess.SetPrivilege("SeTcbPrivilege", false);

            Console.ReadKey(false);
        }
    }
}