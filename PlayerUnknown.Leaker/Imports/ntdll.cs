namespace PlayerUnknown.Leaker.Imports
{
    using System;
    using System.Runtime.InteropServices;

    public class Ntdll
    {
        [DllImport("ntdll.dll")]
        public static extern int NtSuspendProcess(IntPtr HProcess);

        [DllImport("ntdll.dll")]
        public static extern int NtResumeProcess(IntPtr HProcess);

        [DllImport("ntdll.dll")]
        public static extern int NtQueryInformationProcess(IntPtr ProcessHandle, int ProcessInformationClass, IntPtr[] ProcessInformation, int ProcessInformationLength, ref int ReturnLength);

        [DllImport("ntdll.dll")]
        public static extern int NtQuerySystemInformation(int SystemInformationClass, IntPtr SystemInformation, int SystemInformationLength, ref int ReturnLength);

        [DllImport("ntdll.dll")]
        public static extern int RtlCreateUserThread(IntPtr Process, IntPtr ThreadSecurityDescriptor, bool CreateSuspended, IntPtr ZeroBits, IntPtr MaximumStackSize, IntPtr CommittedStackSize, IntPtr StartAddress, IntPtr Parameter, ref IntPtr Thread, IntPtr ClientId);

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct SystemHandle
        {
            public int ProcessID;

            public byte ObjectTypeNumber;

            public char Flags;

            public ushort Handle;

            public IntPtr Object_Pointer;

            public IntPtr GrantedAccess;
        }
    }
}