namespace PlayerUnknown.Leaker.Service
{
    using System;

    public partial class Service
    {
        public struct HandleInfo
        {
            public int Pid;
            public IntPtr HProcess;

            /// <summary>
            /// Initializes a new instance of the <see cref="HandleInfo"/> struct.
            /// </summary>
            /// <param name="Pid">The pid.</param>
            /// <param name="HProcess">The h process.</param>
            public HandleInfo(int Pid, IntPtr HProcess)
            {
                this.Pid        = Pid;
                this.HProcess   = HProcess;
            }
        }
    }
}