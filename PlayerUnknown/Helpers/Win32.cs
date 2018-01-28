namespace PlayerUnknown.Helpers
{
    using System;
    using System.Runtime.InteropServices;

    using PlayerUnknown.Reader.Native;

    public static class Win32
    {
        [DllImport("user32.dll")]
        public static extern int GetWindowRect(IntPtr Handle, out Rectangle Rectangle);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool GetWindowPlacement(IntPtr Handle, ref WindowPlacement Placement);

        /// <summary>
        /// Gets the window placement using the specified handle.
        /// </summary>
        /// <param name="Handle">The handle.</param>
        public static WindowPlacement GetWindowPlacement(IntPtr Handle)
        {
            WindowPlacement Placement = new WindowPlacement();

            if (GetWindowPlacement(Handle, ref Placement))
            {
                return Placement;
            }

            return Placement;
        }
    }
}
