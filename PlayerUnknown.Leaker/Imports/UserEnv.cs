namespace PlayerUnknown.Leaker.Imports
{
    using System;
    using System.Runtime.InteropServices;

    public class UserEnv
    {
        [DllImport("userenv.dll")]
        public static extern bool CreateEnvironmentBlock(ref IntPtr LpEnvironment, IntPtr HToken, bool BInherit);

        [DllImport("userenv.dll")]
        public static extern bool DestroyEnvironmentBlock(IntPtr LpEnvironment);
    }
}