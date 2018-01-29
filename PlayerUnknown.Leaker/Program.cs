namespace PlayerUnknown.Leaker
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Windows.Forms;

    using PlayerUnknown.Leaker.Imports;
    using PlayerUnknown.Leaker.Proc;

    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main(string[] Args)
        {
            CProcess CurrentProcess    = new CProcess();
            CProcess TargetProcess     = new CProcess(Options.TargetProcess);
            CProcess ServProcess;

            IntPtr HProcess = IntPtr.Zero;

            int counter  = 0;
            int MaxCount = 1;

            List<Service.Service.HandleInfo> HandleList = new List<Service.Service.HandleInfo>();

            if (Args.Length == 0)
            {
                CurrentProcess.SetPrivilege("SeDebugPrivilege", true);
                CurrentProcess.SetPrivilege("SeTcbPrivilege", true);

                TargetProcess.Wait(Options.DelayToWait);

                if (TargetProcess.IsValidProcess())
                {
                    HandleList = Service.Service.ServiceEnumHandles(TargetProcess.GetPid(), Options.DesiredAccess);

                    if (HandleList.Count > 0)
                    {
                        foreach (Service.Service.HandleInfo enumerator in HandleList)
                        {
                            if (counter == MaxCount)
                            {
                                break;
                            }

                            if (enumerator.Pid == Kernel32.GetCurrentProcessId())
                            {
                                continue;
                            }

                            ServProcess = new CProcess(enumerator.Pid);

                            if (Service.Service.ServiceSetHandleStatus(ServProcess, enumerator.HProcess, true, true))
                            {
                                HProcess = Service.Service.ServiceStartProcess(null, Directory.GetCurrentDirectory() + "\\" + Options.YourProcess + " " + enumerator.HProcess, null, true, ServProcess.GetHandle());
                                Service.Service.ServiceSetHandleStatus(ServProcess, enumerator.HProcess, false, false);
                                counter++;
                            }

                            if (HProcess != null)
                            {
                                Kernel32.CloseHandle(HProcess);
                            }

                            ServProcess.Close();
                        }
                    }

                    TargetProcess.Close();
                }

                CurrentProcess.SetPrivilege("SeDebugPrivilege", false);
                CurrentProcess.SetPrivilege("SeTcbPrivilege", false);
            }
            else if (Args.Length == 1)
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new HLeakerGui((IntPtr) Convert.ToInt32(Args[Args.Length - 1])));
            }
        }
    }
}