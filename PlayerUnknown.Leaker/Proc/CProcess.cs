namespace PlayerUnknown.Leaker.Proc
{
    using System;
    using System.Diagnostics;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Threading;

    using PlayerUnknown.Leaker.Imports;

    public class CProcess
    {
        private Process[] ProcessList;

        private readonly string ProcessName;

        private IntPtr HProcess;

        /// <summary>
        /// Initializes a new instance of the <see cref="CProcess"/> class.
        /// </summary>
        public CProcess()
        {
            this.ProcessName    = string.Empty;
            this.HProcess       = Kernel32.GetCurrentProcess();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CProcess"/> class.
        /// </summary>
        /// <param name="DwProcessId">The dw process identifier.</param>
        /// <param name="DesiredAccess">The desired access.</param>
        public CProcess(int DwProcessId, uint DesiredAccess = 0x1fffff)
        {
            this.ProcessName    = string.Empty;
            this.HProcess       = Kernel32.OpenProcess(DesiredAccess, false, DwProcessId);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CProcess"/> class.
        /// </summary>
        /// <param name="ProcessName">Name of the process.</param>
        public CProcess(string ProcessName)
        {
            this.ProcessName    = ProcessName;
            this.HProcess       = IntPtr.Zero;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CProcess"/> class.
        /// </summary>
        /// <param name="HProcess">The h process.</param>
        public CProcess(IntPtr HProcess)
        {
            this.ProcessName    = string.Empty;
            this.HProcess       = HProcess;
        }

        /// <summary>
        /// This functions waits for our target process to start.
        /// </summary>
        public bool Wait(int Interval)
        {
            if (this.ProcessName.Length == 0)
            {
                return false;
            }

            this.HProcess = IntPtr.Zero;

            while (true)
            {
                this.ProcessList = Process.GetProcessesByName(this.ProcessName);

                Thread.Sleep(Interval);

                if (this.ProcessList.Length > 0)
                {
                    this.HProcess = Kernel32.OpenProcess(0x1fffff, false, this.ProcessList[0].Id);
                    break;
                }
            }

            return true;
        }

        /// <summary>
        /// This functions sets the privilege of our target process
        /// </summary>
        public bool SetPrivilege(string LpszPrivilege, bool BEnablePrivilege)
        {
            bool Status = true;
            Kernel32.TokenPrivileges priv = new Kernel32.TokenPrivileges();
            IntPtr HToken = IntPtr.Zero;
            Kernel32.Luid luid = new Kernel32.Luid();
            int RetLength = 0;

            if (!Kernel32.OpenProcessToken(this.HProcess, 0x0020, ref HToken))
            {
                Status = false;
                goto EXIT;
            }

            if (!Advapi32.LookupPrivilegeValueA(null, LpszPrivilege, ref luid))
            {
                Status = false;
                goto EXIT;
            }

            priv.PrivilegeCount = 1;
            priv.Privileges = new Kernel32.LuidAndAttributes();
            priv.Privileges.Luid = luid;
            priv.Privileges.Attributes = (int)(BEnablePrivilege ? 0x00000002L : 0x00000004L);

            if (!Kernel32.AdjustTokenPrivileges(HToken, false, ref priv, 0, IntPtr.Zero, ref RetLength))
            {
                Status = false;
            }

            EXIT:
            if (HToken != IntPtr.Zero)
            {
                Kernel32.CloseHandle(HToken);
            }

            return Status;
        }

        /// <summary>
        /// This functions suspends our process
        /// </summary>
        public bool Suspend()
        {
            return Ntdll.NtSuspendProcess(this.HProcess) == 0;
        }

        /// <summary>
        /// This function resumes our process
        /// </summary>
        public bool Resume()
        {
            return Ntdll.NtResumeProcess(this.HProcess) == 0;
        }

        /// <summary>
        /// This functions kills our process
        /// </summary>
        public bool Kill()
        {
            return Kernel32.TerminateProcess(this.HProcess, 0);
        }

        /// <summary>
        /// This functions opens our target process
        /// </summary>
        public bool Open(uint DesiredAccess = 0x1fffff)
        {
            if (this.ProcessName.Length == 0)
            {
                return false;
            }

            this.HProcess       = IntPtr.Zero;
            this.ProcessList    = Process.GetProcessesByName(this.ProcessName);

            if (this.ProcessList.Length > 0)
            {
                this.HProcess   = Kernel32.OpenProcess(DesiredAccess, false, this.ProcessList[0].Id);
            }

            return this.IsValidProcess();
        }

        /// <summary>
        /// This functions closes our target process
        /// </summary>
        public bool Close()
        {
            return Kernel32.CloseHandle(this.HProcess);
        }

        /// <summary>
        /// This functions returns our target process as handle
        /// </summary>
        public IntPtr GetHandle()
        {
            return this.HProcess;
        }

        /// <summary>
        /// This functions returns our target process as process id
        /// </summary>
        public int GetPid()
        {
            return Kernel32.GetProcessId(this.HProcess);
        }

        /// <summary>
        /// This functions returns the parent id of our target process
        /// </summary>
        public int GetParentPid()
        {
            IntPtr[] pbi    = new IntPtr[6];
            int UlSize      = 0;

            if (Ntdll.NtQueryInformationProcess(this.HProcess, 0, pbi, Marshal.SizeOf(pbi), ref UlSize) >= 0)
            {
                return (int)pbi[5];
            }

            return 0;
        }

        /// <summary>
        /// This functions checks if the target process is x64
        /// </summary>
        public int Is64(ref bool Is64)
        {
            int Status          = 1;
            int DwFileSize      = 0, DwReaded = 0, DwSize = 255;

            IntPtr HFile        = (IntPtr)(-1);
            IntPtr LpFile       = (IntPtr) 0;

            Kernel32.ImageDosHeader DosHeader;

            byte[] Path         = new byte[255];
            byte[] FileCopy     = null;
            string LpFileName   = string.Empty;
            int MachineUint     = 0;

            if (!Kernel32.QueryFullProcessImageNameA(this.HProcess, 0, Path, ref DwSize))
            {
                Status = 2;
                goto EXIT;
            }

            LpFileName = Encoding.Default.GetString(Path);
            HFile = Kernel32.CreateFileA(LpFileName, 0x80000000, 0, IntPtr.Zero, 3, 0, IntPtr.Zero);

            if (HFile == (IntPtr)(-1))
            {
                Status = 3;
                goto EXIT;
            }

            DwFileSize = Kernel32.GetFileSize(HFile, IntPtr.Zero);
            LpFile = Kernel32.VirtualAlloc(IntPtr.Zero, DwFileSize, 0x1000, 0x40);

            if (LpFile == IntPtr.Zero)
            {
                Status = 4;
                goto EXIT;
            }

            if (!Kernel32.ReadFile(HFile, LpFile, DwFileSize, ref DwReaded, IntPtr.Zero))
            {
                Status = 5;
                goto EXIT;
            }

            DosHeader = new Kernel32.ImageDosHeader();
            DosHeader = (Kernel32.ImageDosHeader) Marshal.PtrToStructure(LpFile, typeof(Kernel32.ImageDosHeader));

            if (!DosHeader.IsValid)
            {
                Status = 6;
                goto EXIT;
            }

            FileCopy    = new byte[DwFileSize];
            Marshal.Copy(LpFile, FileCopy, 0, DwFileSize);
            MachineUint = BitConverter.ToUInt16(FileCopy, BitConverter.ToInt32(FileCopy, 60) + 4);

            if (MachineUint == 0x8664 || MachineUint == 0x0200)
            {
                Is64 = true;
                goto EXIT;
            }

            if (MachineUint == 0x014c)
            {
                Is64 = false;
            }

            EXIT:

            if (HFile != IntPtr.Zero)
            {
                Kernel32.CloseHandle(HFile);
            }

            if (LpFile != IntPtr.Zero)
            {
                Kernel32.VirtualFree(LpFile, DwFileSize, 0x4000);
            }

            return Status;
        }

        /// <summary>
        /// This functions checks if our target process is valid
        /// </summary>
        public bool IsValidProcess()
        {
            if (this.HProcess == (IntPtr)(-1))
            {
                return false;
            }

            return Kernel32.WaitForSingleObject(this.HProcess, 0) == 258L;
        }
    }
}