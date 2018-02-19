namespace PlayerUnknown.Leaker.Service
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;

    using PlayerUnknown.Leaker.Imports;
    using PlayerUnknown.Leaker.Proc;

    public partial class Service
    {
        private static IntPtr ExitThread             = IntPtr.Zero;
        private static IntPtr GetProcessId           = IntPtr.Zero;
        private static IntPtr NtSetInformationObject = IntPtr.Zero;

        /// <summary>
        /// This function starts our target process in the normal session
        /// </summary>
        public static IntPtr ServiceStartProcess(string LpFile, string LpArguments, string LpDir, bool Inherit, IntPtr HParent)
        {
            Kernel32.Startupinfoex si       = new Kernel32.Startupinfoex();
            Kernel32.ProcessInformation pi  = new Kernel32.ProcessInformation();
            Advapi32.SecurityAttributes sa  = new Advapi32.SecurityAttributes();

            IntPtr ProcessToken             = IntPtr.Zero;
            IntPtr UserToken                = IntPtr.Zero;
            IntPtr PEnvironment             = IntPtr.Zero;
            IntPtr CbAttributeListSize      = IntPtr.Zero;
            IntPtr PAttributeList           = IntPtr.Zero;

            pi.hProcess                     = IntPtr.Zero;

            if (!Kernel32.OpenProcessToken(Kernel32.GetCurrentProcess(), Kernel32.TOKEN_ALL_ACCESS, ref ProcessToken))
            {
                goto EXIT;
            }

            if (!Advapi32.DuplicateTokenEx(ProcessToken, Kernel32.TOKEN_ALL_ACCESS, out sa, Advapi32.SecurityImpersonationLevel.SecurityImpersonation, Advapi32.TokenType.TokenPrimary, out UserToken) || !UserEnv.CreateEnvironmentBlock(ref PEnvironment, UserToken, false))
            {
                goto EXIT;
            }

            Kernel32.InitializeProcThreadAttributeList(IntPtr.Zero, 1, 0, ref CbAttributeListSize);
            PAttributeList = Kernel32.VirtualAlloc(IntPtr.Zero, (int)CbAttributeListSize, 0x1000, 0x40);

            if (!Kernel32.InitializeProcThreadAttributeList(PAttributeList, 1, 0, ref CbAttributeListSize))
            {
                goto EXIT;
            }

            if (!Kernel32.UpdateProcThreadAttribute(PAttributeList, 0, (IntPtr)0x00020000, ref HParent, (IntPtr)Marshal.SizeOf(HParent), IntPtr.Zero, IntPtr.Zero))
            {
                goto EXIT;
            }

            si.LpAttributeList  = PAttributeList;
            si.StartupInfo      = new Kernel32.Startupinfo();
            si.StartupInfo.cb   = Marshal.SizeOf(typeof(Kernel32.Startupinfo));

            if (!Kernel32.CreateProcessAsUserA(UserToken, LpFile, LpArguments, IntPtr.Zero, IntPtr.Zero, Inherit, 0x400 | 0x010 | 0x00080000, PEnvironment, LpDir, ref si, ref pi))
            {
                // TODO.
            }

            EXIT:

            if (ProcessToken != IntPtr.Zero)
            {
                Kernel32.CloseHandle(ProcessToken);
            }

            if (UserToken != IntPtr.Zero)
            {
                Kernel32.CloseHandle(UserToken);
            }

            if (PEnvironment != IntPtr.Zero)
            {
                UserEnv.DestroyEnvironmentBlock(PEnvironment);
            }

            if (PAttributeList != IntPtr.Zero)
            {
                Kernel32.DeleteProcThreadAttributeList(PAttributeList);
                Kernel32.VirtualFree(PAttributeList, (int)CbAttributeListSize, 0x4000);
            }

            if (pi.hThread != IntPtr.Zero)
            {
                Kernel32.CloseHandle(pi.hThread);
            }

            return pi.hProcess;
        }

        /// <summary>
        /// This function executes a shellcode to change the handle characteristics
        /// </summary>
        public static bool ServiceSetHandleStatus(CProcess Process, IntPtr HObject, bool Protect, bool Inherit)
        {
            bool Is64   = false;
            bool Status = false;

            byte[] W64Thread =
            {
                0x48, 0x83, 0xEC, 0x28, 0xF, 0xB6, 0x41, 0x8, 0x4C, 0x8D, 0x44, 0x24, 0x30, 0x41, 0xB9, 0x2, 0x0, 0x0, 0x0, 0x88, 0x44, 0x24, 0x31, 0xF, 0xB6, 0x41, 0xC, 0x4C, 0x8B, 0xD1, 0x48, 0x8B, 0x9, 0x88, 0x44, 0x24, 0x30, 0x41, 0x8D, 0x51, 0x2, 0x41, 0xFF, 0x52, 0x10, 0x33, 0xC9, 0x85, 0xC0, 0xF, 0x94, 0xC1, 0x8B, 0xC1, 0x48, 0x83, 0xC4, 0x28, 0xC3
            };
            byte[] W32Thread =
            {
                0x55, 0x8B, 0xEC, 0x8B, 0x4D, 0x8, 0x6A, 0x2, 0xF, 0xB6, 0x41, 0x4, 0x88, 0x45, 0x9, 0xF, 0xB6, 0x41, 0x8, 0x88, 0x45, 0x8, 0x8D, 0x45, 0x8, 0x50, 0x8B, 0x41, 0xC, 0x6A, 0x4, 0xFF, 0x31, 0xFF, 0xD0, 0xF7, 0xD8, 0x1B, 0xC0, 0x40, 0x5D, 0xC2, 0x4, 0x0
            };

            HandleIn Args;
            IntPtr HThread = IntPtr.Zero, LpArgs = IntPtr.Zero, LpThread = IntPtr.Zero, WThread = IntPtr.Zero, WArgs = IntPtr.Zero;

            if (HObject == IntPtr.Zero || Process.Is64(ref Is64) != 1)
            {
                goto EXIT;
            }

            if (IntPtr.Size == 8 ? !Is64 : Is64)
            {
                goto EXIT;
            }

            if (Service.NtSetInformationObject == IntPtr.Zero)
            {
                Service.NtSetInformationObject = Kernel32.GetProcAddress(Kernel32.GetModuleHandleA("ntdll.dll"), "NtSetInformationObject");

                if (Service.NtSetInformationObject == IntPtr.Zero)
                {
                    goto EXIT;
                }
            }

            Args = new HandleIn(HObject, Protect, Inherit, Service.NtSetInformationObject);

            if (Args.Function == IntPtr.Zero)
            {
                goto EXIT;
            }

            if ((LpThread = Kernel32.VirtualAllocEx(Process.GetHandle(), IntPtr.Zero, Is64 ? W64Thread.Length : W32Thread.Length, 0x1000, 0x40)) == IntPtr.Zero || (LpArgs = Kernel32.VirtualAllocEx(Process.GetHandle(), IntPtr.Zero, Marshal.SizeOf(typeof(HandleIn)), 0x1000, 0x40)) == IntPtr.Zero)
            {
                goto EXIT;
            }

            WArgs = Marshal.AllocHGlobal(Marshal.SizeOf(WArgs));
            WThread = Marshal.AllocHGlobal(Is64 ? W64Thread.Length : W32Thread.Length);
            Marshal.Copy(Is64 ? W64Thread : W32Thread, 0, WThread, Is64 ? W64Thread.Length : W32Thread.Length);
            Marshal.StructureToPtr(Args, WArgs, true);

            if (!Kernel32.WriteProcessMemory(Process.GetHandle(), LpThread, WThread, Is64 ? W64Thread.Length : W32Thread.Length, IntPtr.Zero) || !Kernel32.WriteProcessMemory(Process.GetHandle(), LpArgs, WArgs, Marshal.SizeOf(Args), IntPtr.Zero))
            {
                goto EXIT;
            }

            if (Ntdll.RtlCreateUserThread(Process.GetHandle(), IntPtr.Zero, false, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, LpThread, LpArgs, ref HThread, IntPtr.Zero) != 0)
            {
                goto EXIT;
            }

            Kernel32.WaitForSingleObject(HThread, 0xFFFFFFFF);
            Status = true;

            EXIT:
            if (HThread != IntPtr.Zero)
            {
                Kernel32.CloseHandle(HThread);
            }

            if (LpThread != null)
            {
                Kernel32.VirtualFreeEx(Process.GetHandle(), LpThread, Is64 ? W64Thread.Length : W32Thread.Length, 0x4000);
            }

            if (LpArgs != null)
            {
                Kernel32.VirtualFreeEx(Process.GetHandle(), LpArgs, Marshal.SizeOf(typeof(HandleIn)), 0x4000);
            }

            return Status;
        }

        /// <summary>
        /// This function enumrates the handles to our target process
        /// </summary>
        public static List<HandleInfo> ServiceEnumHandles(int ProcessId, uint DesiredAccess)
        {
            IntPtr buffer           = IntPtr.Zero;
            IntPtr IpHandle         = IntPtr.Zero;
            IntPtr ProcessHandle    = IntPtr.Zero;
            IntPtr ProcessCopy      = IntPtr.Zero;

            int BufferSize          = 0;
            uint status             = 0;
            uint PId                = 0;
            long LHandleCount       = 0;

            List<HandleInfo> handlelist = new List<HandleInfo>();
            Ntdll.SystemHandle WHandles = new Ntdll.SystemHandle();
            HandleInfo hi;
            
            CProcess Process        = null;

            while (true)
            {
                status = (UInt32) Ntdll.NtQuerySystemInformation(0x10, buffer, BufferSize, ref BufferSize);

                if (status != 0)
                {
                    if (status == 0xc0000004)
                    {
                        Kernel32.VirtualFree(buffer, BufferSize, 0x8000);
                        buffer = Kernel32.VirtualAlloc(IntPtr.Zero, BufferSize, 0x1000, 0x40);
                        continue;
                    }

                    break;
                }

                if (IntPtr.Size == 8)
                {
                    LHandleCount = Marshal.ReadInt64(buffer);
                    IpHandle = new IntPtr(buffer.ToInt64() + 8);
                }
                else
                {
                    LHandleCount = Marshal.ReadInt32(buffer);
                    IpHandle = new IntPtr(buffer.ToInt32() + 4);
                }

                for (long i = 0; i < LHandleCount; i++)
                {
                    WHandles = new Ntdll.SystemHandle();

                    if (IntPtr.Size == 8)
                    {
                        WHandles = (Ntdll.SystemHandle) Marshal.PtrToStructure(IpHandle, WHandles.GetType());
                        IpHandle = new IntPtr(IpHandle.ToInt64() + Marshal.SizeOf(WHandles));
                    }
                    else
                    {
                        IpHandle = new IntPtr(IpHandle.ToInt64() + Marshal.SizeOf(WHandles));
                        WHandles = (Ntdll.SystemHandle) Marshal.PtrToStructure(IpHandle, WHandles.GetType());
                    }

                    if (WHandles.ObjectTypeNumber == 7 && WHandles.ProcessID != Kernel32.GetCurrentProcessId() && ((uint) WHandles.GrantedAccess & DesiredAccess) == DesiredAccess)
                    {
                        if (Options.UseDuplicateHandle)
                        {
                            ProcessHandle = Kernel32.OpenProcess(0x0040, false, WHandles.ProcessID);

                            if (Kernel32.DuplicateHandle(ProcessHandle, (IntPtr) WHandles.Handle, Kernel32.GetCurrentProcess(), ref ProcessCopy, 0x0400, false, 0))
                            {
                                if (Kernel32.GetProcessId(ProcessCopy) == ProcessId && WHandles.ProcessID != ProcessId)
                                {
                                    hi = new HandleInfo(WHandles.ProcessID, (IntPtr) WHandles.Handle);
                                    handlelist.Add(hi);
                                }
                            }

                            if (ProcessHandle != IntPtr.Zero)
                            {
                                Kernel32.CloseHandle(ProcessHandle);
                            }

                            if (ProcessCopy != IntPtr.Zero)
                            {
                                Kernel32.CloseHandle(ProcessCopy);
                            }
                        }
                        else
                        {
                            Process = new CProcess(WHandles.ProcessID, 0x1fffff);

                            if (Process.IsValidProcess() && Service.ServiceGetProcessId(Process, (IntPtr) WHandles.Handle, ref PId))
                            {
                                if (PId == ProcessId)
                                {
                                    hi = new HandleInfo(WHandles.ProcessID, (IntPtr) WHandles.Handle);
                                    handlelist.Add(hi);
                                }
                            }

                            if (Process.IsValidProcess())
                            {
                                Process.Close();
                            }
                        }
                    }
                }

                break;
            }

            if (buffer != null)
            {
                Kernel32.VirtualFree(buffer, BufferSize, 0x8000);
            }

            return handlelist;
        }

        /// <summary>
        /// This function executes a shellcode to get the process id of an existing handle
        /// </summary>
        private static bool ServiceGetProcessId(CProcess Process, IntPtr Handle, ref uint ProcessId)
        {
            ThreadIn Args;

            byte[] W32Thread =
            {
                0x55, 0x8B, 0xEC, 0x56, 0x8B, 0x75, 0x08, 0xFF, 0x36, 0x8B, 0x46, 0x04, 0xFF, 0xD0, 0x50, 0x8B, 0x46, 0x08, 0xFF, 0xD0, 0x5E, 0x5D, 0xC3
            };

            byte[] W64Thread =
            {
                0x40, 0x53, 0x48, 0x83, 0xEC, 0x20, 0x48, 0x8B, 0xD9, 0x48, 0x8B, 0x09, 0xFF, 0x53, 0x08, 0x8B, 0xC8, 0xFF, 0x53, 0x10, 0xB8, 0x01, 0x00, 0x00, 0x00, 0x48, 0x83, 0xC4, 0x20, 0x5B, 0xC3
            };

            bool Is64       = false;
            bool Status     = false;

            IntPtr HThread  = IntPtr.Zero;
            IntPtr LpArgs   = IntPtr.Zero;
            IntPtr LpThread = IntPtr.Zero;
            IntPtr WThread  = IntPtr.Zero;
            IntPtr WArgs    = IntPtr.Zero;
            IntPtr k32      = IntPtr.Zero;

            if (Handle == IntPtr.Zero || Process.Is64(ref Is64) != 1)
            {
                goto EXIT;
            }

            if (IntPtr.Size == 8 ? !Is64 : Is64)
            {
                goto EXIT;
            }

            if (Service.ExitThread == IntPtr.Zero && Service.GetProcessId == IntPtr.Zero)
            {
                k32                 = Kernel32.GetModuleHandleA("kernel32.dll");
                Service.ExitThread  = Kernel32.GetProcAddress(k32, "ExitThread");

                if (Service.ExitThread == IntPtr.Zero)
                {
                    goto EXIT;
                }

                Service.GetProcessId = Kernel32.GetProcAddress(k32, "GetProcessId");

                if (Service.GetProcessId == IntPtr.Zero)
                {
                    goto EXIT;
                }
            }

            Args = new ThreadIn(Handle, Service.GetProcessId, Service.ExitThread);

            if ((LpThread = Kernel32.VirtualAllocEx(Process.GetHandle(), IntPtr.Zero, Is64 ? W64Thread.Length : W32Thread.Length, 0x1000, 0x40)) == IntPtr.Zero || (LpArgs = Kernel32.VirtualAllocEx(Process.GetHandle(), IntPtr.Zero, Marshal.SizeOf(typeof(ThreadIn)), 0x1000, 0x40)) == IntPtr.Zero)
            {
                goto EXIT;
            }

            WArgs   = Marshal.AllocHGlobal(Marshal.SizeOf(Args));
            WThread = Marshal.AllocHGlobal(Is64 ? W64Thread.Length : W32Thread.Length);

            Marshal.Copy(Is64 ? W64Thread : W32Thread, 0, WThread, Is64 ? W64Thread.Length : W32Thread.Length);
            Marshal.StructureToPtr(Args, WArgs, true);

            if (!Kernel32.WriteProcessMemory(Process.GetHandle(), LpThread, WThread, Is64 ? W64Thread.Length : W32Thread.Length, IntPtr.Zero) || !Kernel32.WriteProcessMemory(Process.GetHandle(), LpArgs, WArgs, Marshal.SizeOf(Args), IntPtr.Zero))
            {
                goto EXIT;
            }

            if (Ntdll.RtlCreateUserThread(Process.GetHandle(), IntPtr.Zero, false, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, LpThread, LpArgs, ref HThread, IntPtr.Zero) != 0)
            {
                goto EXIT;
            }

            if (Kernel32.WaitForSingleObject(HThread, Options.ObjectTimeout) == 0x00000102)
            {
                Kernel32.TerminateThread(HThread, 0x102);
                goto EXIT;
            }

            Status = Kernel32.GetExitCodeThread(HThread, out ProcessId);

            EXIT:

            if (HThread != IntPtr.Zero)
            {
                Kernel32.CloseHandle(HThread);
            }

            Kernel32.VirtualFreeEx(Process.GetHandle(), LpThread, Is64 ? W64Thread.Length : W32Thread.Length, 0x4000);
            Kernel32.VirtualFreeEx(Process.GetHandle(), LpArgs, Marshal.SizeOf(typeof(ThreadIn)), 0x4000);

            return Status;
        }
    }
}