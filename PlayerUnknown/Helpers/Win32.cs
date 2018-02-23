namespace PlayerUnknown.Helpers
{
    using System;
    using System.Runtime.InteropServices;

    using PlayerUnknown.Native;

    public static class Win32
    {
        [DllImport("user32.dll")]
        public static extern int GetWindowRect(IntPtr Handle, out Rectangle Rectangle);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetWindowPlacement(IntPtr Handle, ref WindowPlacement Placement);

        [DllImport("kernel32.dll")]
        public static extern bool ReadProcessMemory(IntPtr Handle, IntPtr Address, byte[] Buffer, int Size, int BytesRead);

        [DllImport("kernel32.dll")]
        public static extern bool WriteProcessMemory(IntPtr Handle, IntPtr Address, byte[] Buffer, int Size, int BytesWritten);

        [DllImport("user32.dll")]
        public static extern int SetWindowText(IntPtr Handle, string Title);

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
