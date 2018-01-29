namespace PlayerUnknown.Leaker.Service
{
    using System;

    public partial class Service
    {
        public struct ThreadIn
        {
            public IntPtr HProcess;
            public IntPtr GetProcessId;
            public IntPtr ExitThread;

            /// <summary>
            /// Initializes a new instance of the <see cref="ThreadIn"/> struct.
            /// </summary>
            /// <param name="HProcess">The h process.</param>
            /// <param name="GetProcessId">The get process identifier.</param>
            /// <param name="ExitThread">The exit thread.</param>
            public ThreadIn(IntPtr HProcess, IntPtr GetProcessId, IntPtr ExitThread)
            {
                this.HProcess       = HProcess;
                this.GetProcessId   = GetProcessId;
                this.ExitThread     = ExitThread;
            }
        }
    }
}