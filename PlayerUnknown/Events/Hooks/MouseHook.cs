namespace PlayerUnknown.Events.Hooks
{
    using System;
    using System.Diagnostics;
    using System.Runtime.InteropServices;

    /// <summary>
    /// Class for intercepting low level Windows mouse hooks.
    /// </summary>
    public class MouseHook
    {
        private const int WH_MOUSE_LL = 14;

        private MouseHookHandler HookHandler;

        /// <summary>
        /// Low level mouse hook's ID
        /// </summary>
        private IntPtr HookId = IntPtr.Zero;

        /// <summary>
        /// Internal callback processing function
        /// </summary>
        private delegate IntPtr MouseHookHandler(int NCode, IntPtr WParam, IntPtr LParam);

        /// <summary>
        /// Function to be called when defined even occurs
        /// </summary>
        /// <param name="MouseStruct">MSLLHOOKSTRUCT mouse structure</param>
        public delegate void MouseHookCallback(Msllhookstruct MouseStruct);

        public event MouseHookCallback LeftButtonDown;

        public event MouseHookCallback LeftButtonUp;

        public event MouseHookCallback RightButtonDown;

        public event MouseHookCallback RightButtonUp;

        public event MouseHookCallback MouseMove;

        public event MouseHookCallback MouseWheel;

        public event MouseHookCallback DoubleClick;

        public event MouseHookCallback MiddleButtonDown;

        public event MouseHookCallback MiddleButtonUp;

        /// <summary>
        /// Destructor. Unhook current hook
        /// </summary>
        ~MouseHook()
        {
            this.Uninstall();
        }

        /// <summary>
        /// Install low level mouse hook
        /// </summary>
        /// <param name="mouseHookCallbackFunc">Callback function</param>
        public void Install()
        {
            this.HookHandler = this.HookFunc;
            this.HookId      = this.SetHook(this.HookHandler);
        }

        /// <summary>
        /// Remove low level mouse hook
        /// </summary>
        public void Uninstall()
        {
            if (this.HookId == IntPtr.Zero)
            {
                return;
            }

            MouseHook.UnhookWindowsHookEx(this.HookId);
            this.HookId = IntPtr.Zero;
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool UnhookWindowsHookEx(IntPtr Hhk);

        /// <summary>
        /// Sets hook and assigns its ID for tracking
        /// </summary>
        /// <param name="Proc">Internal callback function</param>
        /// <returns>Hook ID</returns>
        private IntPtr SetHook(MouseHookHandler Proc)
        {
            using (ProcessModule module = Process.GetCurrentProcess().MainModule)
            {
                return MouseHook.SetWindowsHookEx(MouseHook.WH_MOUSE_LL, Proc, MouseHook.GetModuleHandle(module.ModuleName), 0);
            }
        }

        /// <summary>
        /// Callback function
        /// </summary>
        private IntPtr HookFunc(int NCode, IntPtr WParam, IntPtr LParam)
        {
            // parse system messages
            if (NCode >= 0)
            {
                if (MouseMessages.WmLbuttondown == (MouseMessages)WParam)
                {
                    if (this.LeftButtonDown != null)
                    {
                        this.LeftButtonDown((Msllhookstruct)Marshal.PtrToStructure(LParam, typeof(Msllhookstruct)));
                    }
                }

                if (MouseMessages.WmLbuttonup == (MouseMessages)WParam)
                {
                    if (this.LeftButtonUp != null)
                    {
                        this.LeftButtonUp((Msllhookstruct)Marshal.PtrToStructure(LParam, typeof(Msllhookstruct)));
                    }
                }

                if (MouseMessages.WmRbuttondown == (MouseMessages)WParam)
                {
                    if (this.RightButtonDown != null)
                    {
                        this.RightButtonDown((Msllhookstruct)Marshal.PtrToStructure(LParam, typeof(Msllhookstruct)));
                    }
                }

                if (MouseMessages.WmRbuttonup == (MouseMessages)WParam)
                {
                    if (this.RightButtonUp != null)
                    {
                        this.RightButtonUp((Msllhookstruct)Marshal.PtrToStructure(LParam, typeof(Msllhookstruct)));
                    }
                }

                if (MouseMessages.WmMousemove == (MouseMessages)WParam)
                {
                    if (this.MouseMove != null)
                    {
                        this.MouseMove((Msllhookstruct)Marshal.PtrToStructure(LParam, typeof(Msllhookstruct)));
                    }
                }

                if (MouseMessages.WmMousewheel == (MouseMessages)WParam)
                {
                    if (this.MouseWheel != null)
                    {
                        this.MouseWheel((Msllhookstruct)Marshal.PtrToStructure(LParam, typeof(Msllhookstruct)));
                    }
                }

                if (MouseMessages.WmLbuttondblclk == (MouseMessages)WParam)
                {
                    if (this.DoubleClick != null)
                    {
                        this.DoubleClick((Msllhookstruct)Marshal.PtrToStructure(LParam, typeof(Msllhookstruct)));
                    }
                }

                if (MouseMessages.WmMbuttondown == (MouseMessages)WParam)
                {
                    if (this.MiddleButtonDown != null)
                    {
                        this.MiddleButtonDown((Msllhookstruct)Marshal.PtrToStructure(LParam, typeof(Msllhookstruct)));
                    }
                }

                if (MouseMessages.WmMbuttonup == (MouseMessages)WParam)
                {
                    if (this.MiddleButtonUp != null)
                    {
                        this.MiddleButtonUp((Msllhookstruct)Marshal.PtrToStructure(LParam, typeof(Msllhookstruct)));
                    }
                }
            }

            return MouseHook.CallNextHookEx(this.HookId, NCode, WParam, LParam);
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int IdHook, MouseHookHandler Lpfn, IntPtr HMod, uint DwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr Hhk, int NCode, IntPtr WParam, IntPtr LParam);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string LpModuleName);

        [StructLayout(LayoutKind.Sequential)]
        public struct Msllhookstruct
        {
            public Point pt;

            public uint mouseData;

            public uint flags;

            public uint time;

            public IntPtr dwExtraInfo;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct Point
        {
            public int x;

            public int y;
        }
    }
}