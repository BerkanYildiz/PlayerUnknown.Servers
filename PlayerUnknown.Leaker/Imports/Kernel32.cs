namespace PlayerUnknown.Leaker.Imports
{
    using System;
    using System.Runtime.InteropServices;

    public class Kernel32
    {
        public const uint STANDARD_RIGHTS_REQUIRED = 0x000F0000;

        public const uint STANDARD_RIGHTS_READ = 0x00020000;

        public const uint TOKEN_ASSIGN_PRIMARY = 0x0001;

        public const uint TOKEN_DUPLICATE = 0x0002;

        public const uint TOKEN_IMPERSONATE = 0x0004;

        public const uint TOKEN_QUERY = 0x0008;

        public const uint TOKEN_QUERY_SOURCE = 0x0010;

        public const uint TOKEN_ADJUST_PRIVILEGES = 0x0020;

        public const uint TOKEN_ADJUST_GROUPS = 0x0040;

        public const uint TOKEN_ADJUST_DEFAULT = 0x0080;

        public const uint TOKEN_ADJUST_SESSIONID = 0x0100;

        public const uint TOKEN_READ = Kernel32.STANDARD_RIGHTS_READ | Kernel32.TOKEN_QUERY;

        public const uint TOKEN_ALL_ACCESS = Kernel32.STANDARD_RIGHTS_REQUIRED | Kernel32.TOKEN_ASSIGN_PRIMARY | Kernel32.TOKEN_DUPLICATE | Kernel32.TOKEN_IMPERSONATE | Kernel32.TOKEN_QUERY | Kernel32.TOKEN_QUERY_SOURCE | Kernel32.TOKEN_ADJUST_PRIVILEGES | Kernel32.TOKEN_ADJUST_GROUPS | Kernel32.TOKEN_ADJUST_DEFAULT | Kernel32.TOKEN_ADJUST_SESSIONID;

        [DllImport("kernel32.dll")]
        public static extern IntPtr CreateFileA(string LpFleName, uint DwDesiredAccess, int DwShareMode, IntPtr LpSecurityAttributes, int DwCreationDisposition, int DwFlagsAndAttributes, IntPtr HTemplateFile);

        [DllImport("kernel32.dll")]
        public static extern bool ReadFile(IntPtr HFile, IntPtr Buffer, int NNumberOfBytesToRead, ref int LpNumberOfBytesRead, IntPtr LpOverlapped);

        [DllImport("kernel32.dll")]
        public static extern bool CloseHandle(IntPtr HObject);

        [DllImport("kernel32.dll")]
        public static extern int GetFileSize(IntPtr HFile, IntPtr LpFileSizeHigh);

        [DllImport("kernel32.dll")]
        public static extern IntPtr VirtualAlloc(IntPtr LpAddress, int DwSize, int FlAllocationType, int FlProtect);

        [DllImport("kernel32.dll")]
        public static extern IntPtr VirtualAllocEx(IntPtr HProcess, IntPtr LpAddress, int DwSize, int FlAllocationType, int FlProtect);

        [DllImport("kernel32.dll")]
        public static extern bool VirtualFree(IntPtr LpAddress, int DwSize, int DwFreeType);

        [DllImport("kernel32.dll")]
        public static extern bool VirtualFreeEx(IntPtr HProcess, IntPtr LpAddress, int DwSize, int DwFreeType);

        [DllImport("kernel32.dll")]
        public static extern int WaitForSingleObject(IntPtr HObject, uint DwMilliseconds);

        [DllImport("kernel32.dll")]
        public static extern IntPtr OpenProcess(uint DwDesiredAccess, bool BInheritHandle, int DwProcessId);

        [DllImport("kernel32.dll")]
        public static extern bool TerminateProcess(IntPtr HProcess, int ExitStatus);

        [DllImport("kernel32.dll")]
        public static extern bool TerminateThread(IntPtr HThread, int ExitStatus);

        [DllImport("kernel32.dll")]
        public static extern bool OpenProcessToken(IntPtr HProcess, uint DesiredAccess, ref IntPtr TokenHandle);

        [DllImport("KERNELBASE.dll")]
        public static extern bool AdjustTokenPrivileges(IntPtr TokenHandle, bool DisableAllPrivileges, ref TokenPrivileges NewState, int BufferLength, IntPtr PreviousState, ref int ReturnLength);

        [DllImport("kernel32.dll")]
        public static extern bool QueryFullProcessImageNameA(IntPtr HProcess, int DwFlags, byte[] LpExeName, ref int LpdwSize);

        [DllImport("kernel32.dll")]
        public static extern IntPtr GetCurrentProcess();

        [DllImport("kernel32.dll")]
        public static extern int GetCurrentProcessId();

        [DllImport("kernel32.dll")]
        public static extern int GetProcessId(IntPtr HProcess);

        [DllImport("kernel32.dll")]
        public static extern bool WriteProcessMemory(IntPtr HProcess, IntPtr LpBaseAddress, IntPtr LpBuffer, int NSize, IntPtr LpNumberOfBytesWritten);

        [DllImport("kernel32.dll")]
        public static extern IntPtr GetProcAddress(IntPtr HModule, string LpProcName);

        [DllImport("kernel32.dll")]
        public static extern IntPtr GetModuleHandleA(string LpModuleName);

        [DllImport("kernel32.dll")]
        public static extern bool CreateProcessAsUserA(IntPtr HToken, string LpApplicationName, string LpCommandLine, IntPtr LpProcessAttributes, IntPtr LpThreadAttributes, bool BInheritHandles, int DwCreationFlags, IntPtr LpEnvironment, string LpCurrentDirectory, ref Startupinfoex Si, ref ProcessInformation Pi);

        [DllImport("kernel32.dll")]
        public static extern void DeleteProcThreadAttributeList(IntPtr LpAttributeList);

        [DllImport("kernel32.dll")]
        public static extern bool InitializeProcThreadAttributeList(IntPtr LpAttributeList, int DwAttributeCount, int DwFlags, ref IntPtr LpSize);

        [DllImport("kernel32.dll")]
        public static extern bool UpdateProcThreadAttribute(IntPtr LpAttributeList, uint DwFlags, IntPtr Attribute, ref IntPtr LpValue, IntPtr CbSize, IntPtr LpPreviousValue, IntPtr LpReturnSize);

        [DllImport("kernel32.dll")]
        public static extern bool GetExitCodeThread(IntPtr HThread, out uint LpExitCode);

        [DllImport("kernel32.dll")]
        public static extern bool DuplicateHandle(IntPtr HSourceProcessHandle, IntPtr HSourceHandle, IntPtr TargetProcessHandle, ref IntPtr LpTargetHandle, int DwDesiredAccess, bool BInherithandle, int DwOptions);

        [StructLayout(LayoutKind.Sequential)]
        public struct ImageDosHeader
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
            public char[] e_magic;

            public ushort e_cblp;

            public ushort e_cp;

            public ushort e_crlc;

            public ushort e_cparhdr;

            public ushort e_minalloc;

            public ushort e_maxalloc;

            public ushort e_ss;

            public ushort e_sp;

            public ushort e_csum;

            public ushort e_ip;

            public ushort e_cs;

            public ushort e_lfarlc;

            public ushort e_ovno;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public ushort[] e_res1;

            public ushort e_oemid;

            public ushort e_oeminfo;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
            public ushort[] e_res2;

            public uint e_lfanew;

            private string EMagic
            {
                get
                {
                    return new string(this.e_magic);
                }
            }

            public bool IsValid
            {
                get
                {
                    return this.EMagic == "MZ";
                }
            }
        }

        public struct Luid
        {
            public int LowPart;

            public int HighPart;
        }

        public struct LuidAndAttributes
        {
            public Luid Luid;

            public int Attributes;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct ProcessInformation
        {
            public IntPtr hProcess;

            public IntPtr hThread;

            public int dwProcessId;

            public int dwThreadId;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct Startupinfo
        {
            public int cb;

            public string lpReserved;

            public string lpDesktop;

            public string lpTitle;

            public int dwX;

            public int dwY;

            public int dwXSize;

            public int dwYSize;

            public int dwXCountChars;

            public int dwYCountChars;

            public int dwFillAttribute;

            public int dwFlags;

            public short wShowWindow;

            public short cbReserved2;

            public IntPtr lpReserved2;

            public IntPtr hStdInput;

            public IntPtr hStdOutput;

            public IntPtr hStdError;
        }

        public struct Startupinfoex
        {
            public Startupinfo StartupInfo;

            public IntPtr LpAttributeList;
        }

        public struct TokenPrivileges
        {
            public int PrivilegeCount;

            public LuidAndAttributes Privileges;
        }
    }
}