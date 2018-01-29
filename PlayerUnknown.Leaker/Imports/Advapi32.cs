namespace PlayerUnknown.Leaker.Imports
{
    using System;
    using System.Runtime.InteropServices;

    public class Advapi32
    {
        public enum SecurityImpersonationLevel
        {
            SecurityAnonymous,
            SecurityIdentification,
            SecurityImpersonation,
            SecurityDelegation
        }

        public enum TokenType
        {
            TokenPrimary = 1,

            TokenImpersonation
        }

        [DllImport("Advapi32.dll")]
        public static extern bool LookupPrivilegeValueA(string LpSystemName, string LpName, ref Kernel32.Luid LpLuid);

        [DllImport("advapi32.dll")]
        public static extern bool DuplicateTokenEx(IntPtr HExistingToken, uint DwDesiredAccess, out SecurityAttributes LpTokenAttributes, SecurityImpersonationLevel ImpersonationLevel, TokenType TokenType, out IntPtr PhNewToken);

        [StructLayout(LayoutKind.Sequential)]
        public struct SecurityAttributes
        {
            public int nLength;
            public byte lpSecurityDescriptor;
            public int bInheritHandle;
        }
    }
}