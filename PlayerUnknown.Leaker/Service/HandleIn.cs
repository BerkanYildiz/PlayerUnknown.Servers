namespace PlayerUnknown.Leaker.Service
{
    using System;

    public struct HandleIn
    {
        public IntPtr HObject;
        public IntPtr Function;

        public bool PStatus;
        public bool Status;

        /// <summary>
        /// Initializes a new instance of the <see cref="HandleIn"/> struct.
        /// </summary>
        /// <param name="HObject">The h object.</param>
        /// <param name="PStatus">if set to <c>true</c> [p status].</param>
        /// <param name="Status">if set to <c>true</c> [status].</param>
        /// <param name="Function">The function.</param>
        public HandleIn(IntPtr HObject, bool PStatus, bool Status, IntPtr Function)
        {
            this.HObject    = HObject;
            this.PStatus    = PStatus;
            this.Status     = Status;
            this.Function   = Function;
        }
    }
}