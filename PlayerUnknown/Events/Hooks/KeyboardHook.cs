namespace PlayerUnknown.Events.Hooks
{
    using System;
    using System.Diagnostics;
    using System.Runtime.InteropServices;

    /// <summary>
    /// Class for intercepting low level keyboard hooks
    /// </summary>
    public class KeyboardHook
    {
        private KeyboardHookHandler HookHandler;

        /// <summary>
        /// Hook ID
        /// </summary>
        private IntPtr HookId = IntPtr.Zero;

        /// <summary>
        /// Internal callback processing function
        /// </summary>
        private delegate IntPtr KeyboardHookHandler(int NCode, IntPtr WParam, IntPtr LParam);

        /// <summary>
        /// Function that will be called when defined events occur
        /// </summary>
        /// <param name="Key">VKeys</param>
        public delegate void KeyboardHookCallback(VKeys Key);

        public event KeyboardHookCallback KeyDown;
        public event KeyboardHookCallback KeyUp;

        /// <summary>
        /// Destructor. Unhook current hook
        /// </summary>
        ~KeyboardHook()
        {
            this.Uninstall();
        }

        /// <summary>
        /// Install low level keyboard hook
        /// </summary>
        public void Install()
        {
            this.HookHandler = this.HookFunc;
            this.HookId = this.SetHook(this.HookHandler);
        }

        /// <summary>
        /// Remove low level keyboard hook
        /// </summary>
        public void Uninstall()
        {
            KeyboardHook.UnhookWindowsHookEx(this.HookId);
        }

        /// <summary>
        /// Registers hook with Windows API
        /// </summary>
        /// <param name="Proc">Callback function</param>
        /// <returns>Hook ID</returns>
        private IntPtr SetHook(KeyboardHookHandler Proc)
        {
            using (ProcessModule module = Process.GetCurrentProcess().MainModule)
            {
                return KeyboardHook.SetWindowsHookEx(13, Proc, KeyboardHook.GetModuleHandle(module.ModuleName), 0);
            }
        }

        /// <summary>
        /// Default hook call, which analyses pressed keys
        /// </summary>
        private IntPtr HookFunc(int NCode, IntPtr WParam, IntPtr LParam)
        {
            if (NCode >= 0)
            {
                int IwParam = WParam.ToInt32();

                if (IwParam == KeyboardHook.WM_KEYDOWN || IwParam == KeyboardHook.WM_SYSKEYDOWN)
                {
                    if (this.KeyDown != null)
                    {
                        this.KeyDown((VKeys)Marshal.ReadInt32(LParam));
                    }
                }

                if (IwParam == KeyboardHook.WM_KEYUP || IwParam == KeyboardHook.WM_SYSKEYUP)
                {
                    if (this.KeyUp != null)
                    {
                        this.KeyUp((VKeys)Marshal.ReadInt32(LParam));
                    }
                }
            }

            return KeyboardHook.CallNextHookEx(this.HookId, NCode, WParam, LParam);
        }

        private const int WM_KEYDOWN = 0x100;

        private const int WM_SYSKEYDOWN = 0x104;

        private const int WM_KEYUP = 0x101;

        private const int WM_SYSKEYUP = 0x105;

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int IdHook, KeyboardHookHandler Lpfn, IntPtr HMod, uint DwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool UnhookWindowsHookEx(IntPtr Hhk);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr Hhk, int NCode, IntPtr WParam, IntPtr LParam);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string LpModuleName);
    }
}