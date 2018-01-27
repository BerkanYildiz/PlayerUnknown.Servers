namespace PlayerUnknown.Reader.Windows
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Runtime.InteropServices;
    using System.Text;

    using PlayerUnknown.Reader.Helpers;
    using PlayerUnknown.Reader.Internals;
    using PlayerUnknown.Reader.Native;

    /// <summary>
    /// Static core class providing tools for managing windows.
    /// </summary>
    public static class WindowCore
    {
        /// <summary>
        /// Retrieves the name of the class to which the specified window belongs.
        /// </summary>
        /// <param name="WindowHandle">A handle to the window and, indirectly, the class to which the window belongs.</param>
        /// <returns>The return values is the class name string.</returns>
        public static string GetClassName(IntPtr WindowHandle)
        {
            // Check if the handle is valid
            HandleManipulator.ValidateAsArgument(WindowHandle, "windowHandle");

            // Get the window class name
            var StringBuilder = new StringBuilder(char.MaxValue);
            if (NativeMethods.GetClassName(WindowHandle, StringBuilder, StringBuilder.Capacity) == 0)
            {
                throw new Win32Exception("Couldn't get the class name of the window or the window has no class name.");
            }

            return StringBuilder.ToString();
        }

        /// <summary>
        /// Retrieves a handle to the foreground window (the window with which the user is currently working).
        /// </summary>
        /// <returns>A handle to the foreground window. The foreground window can be <c>IntPtr.Zero</c> in certain circumstances, such as when a window is losing activation.</returns>
        public static IntPtr GetForegroundWindow()
        {
            return NativeMethods.GetForegroundWindow();
        }

        /// <summary>
        /// Retrieves the specified system metric or system configuration setting.
        /// </summary>
        /// <param name="Metric">The system metric or configuration setting to be retrieved.</param>
        /// <returns>The return value is the requested system metric or configuration setting.</returns>
        public static int GetSystemMetrics(SystemMetrics Metric)
        {
            var ret = NativeMethods.GetSystemMetrics(Metric);
            if (ret != 0)
            {
                return ret;
            }

            throw new Win32Exception("The call of GetSystemMetrics failed. Unfortunately, GetLastError code doesn't provide more information.");
        }

        /// <summary>
        /// Gets the text of the specified window's title bar.
        /// </summary>
        /// <param name="WindowHandle">A handle to the window containing the text.</param>
        /// <returns>The return value is the window's title bar.</returns>
        public static string GetWindowText(IntPtr WindowHandle)
        {
            // Check if the handle is valid
            HandleManipulator.ValidateAsArgument(WindowHandle, "windowHandle");

            // Get the size of the window's title
            var capacity = NativeMethods.GetWindowTextLength(WindowHandle);

            // If the window doesn't contain any title
            if (capacity == 0)
            {
                return string.Empty;
            }

            // Get the text of the window's title bar text
            var StringBuilder = new StringBuilder(capacity + 1);
            if (NativeMethods.GetWindowText(WindowHandle, StringBuilder, StringBuilder.Capacity) == 0)
            {
                throw new Win32Exception("Couldn't get the text of the window's title bar or the window has no title.");
            }

            return StringBuilder.ToString();
        }

        /// <summary>
        /// Retrieves the show state and the restored, minimized, and maximized positions of the specified window.
        /// </summary>
        /// <param name="WindowHandle">A handle to the window.</param>
        /// <returns>The return value is a <see cref="WindowPlacement"/> structure that receives the show state and position information.</returns>
        public static WindowPlacement GetWindowPlacement(IntPtr WindowHandle)
        {
            // Check if the handle is valid
            HandleManipulator.ValidateAsArgument(WindowHandle, "windowHandle");

            // Allocate a WindowPlacement structure
            WindowPlacement placement;
            placement.Length = Marshal.SizeOf(typeof(WindowPlacement));

            // Get the window placement
            if (!NativeMethods.GetWindowPlacement(WindowHandle, out placement))
            {
                throw new Win32Exception("Couldn't get the window placement.");
            }

            return placement;
        }

        /// <summary>
        /// Retrieves the identifier of the process that created the window. 
        /// </summary>
        /// <param name="WindowHandle">A handle to the window.</param>
        /// <returns>The return value is the identifier of the process that created the window.</returns>
        public static int GetWindowProcessId(IntPtr WindowHandle)
        {
            // Check if the handle is valid
            HandleManipulator.ValidateAsArgument(WindowHandle, "windowHandle");

            // Get the process id
            int ProcessId;
            NativeMethods.GetWindowThreadProcessId(WindowHandle, out ProcessId);
            return ProcessId;
        }

        /// <summary>
        /// Retrieves the identifier of the thread that created the specified window.
        /// </summary>
        /// <param name="WindowHandle">A handle to the window.</param>
        /// <returns>The return value is the identifier of the thread that created the window.</returns>
        public static int GetWindowThreadId(IntPtr WindowHandle)
        {
            // Check if the handle is valid
            HandleManipulator.ValidateAsArgument(WindowHandle, "windowHandle");

            // Get the thread id
            int trash;
            return NativeMethods.GetWindowThreadProcessId(WindowHandle, out trash);
        }

        /// <summary>
        /// Enumerates all the windows on the screen.
        /// </summary>
        /// <returns>A collection of handles of all the windows.</returns>
        public static IEnumerable<IntPtr> EnumAllWindows()
        {
            // Create the list of windows
            var list = new List<IntPtr>();

            // For each top-level windows
            foreach (var TopWindow in WindowCore.EnumTopLevelWindows())
            {
                // Add this window to the list
                list.Add(TopWindow);

                // Enumerate and add the children of this window
                list.AddRange(WindowCore.EnumChildWindows(TopWindow));
            }

            // Return the list of windows
            return list;
        }

        /// <summary>
        /// Enumerates recursively all the child windows that belong to the specified parent window.
        /// </summary>
        /// <param name="ParentHandle">The parent window handle.</param>
        /// <returns>A collection of handles of the child windows.</returns>
        public static IEnumerable<IntPtr> EnumChildWindows(IntPtr ParentHandle)
        {
            // Create the list of windows
            var list = new List<IntPtr>();

            // Create the callback
            EnumWindowsProc callback = delegate(IntPtr WindowHandle, IntPtr LParam)
                {
                    list.Add(WindowHandle);
                    return true;
                };

            // Enumerate all windows
            NativeMethods.EnumChildWindows(ParentHandle, callback, IntPtr.Zero);

            // Returns the list of the windows
            return list.ToArray();
        }

        /// <summary>
        /// Enumerates all top-level windows on the screen. This function does not search child windows. 
        /// </summary>
        /// <returns>A collection of handles of top-level windows.</returns>
        public static IEnumerable<IntPtr> EnumTopLevelWindows()
        {
            // When passing a null pointer, this function is equivalent to EnumWindows
            return WindowCore.EnumChildWindows(IntPtr.Zero);
        }

        /// <summary>
        /// Flashes the specified window one time. It does not change the active state of the window.
        /// To flash the window a specified number of times, use the <see cref="FlashWindowEx(IntPtr, FlashWindowFlags, uint, TimeSpan)"/> function.
        /// </summary>
        /// <param name="WindowHandle">A handle to the window to be flashed. The window can be either open or minimized.</param>
        /// <returns>
        /// The return value specifies the window's state before the call to the <see cref="FlashWindow"/> function. 
        /// If the window caption was drawn as active before the call, the return value is nonzero. Otherwise, the return value is zero.
        /// </returns>
        public static bool FlashWindow(IntPtr WindowHandle)
        {
            // Check if the handle is valid
            HandleManipulator.ValidateAsArgument(WindowHandle, "windowHandle");

            // Flash the window
            return NativeMethods.FlashWindow(WindowHandle, true);
        }

        /// <summary>
        /// Flashes the specified window. It does not change the active state of the window.
        /// </summary>
        /// <param name="WindowHandle">A handle to the window to be flashed. The window can be either opened or minimized.</param>
        /// <param name="Flags">The flash status.</param>
        /// <param name="Count">The number of times to flash the window.</param>
        /// <param name="Timeout">The rate at which the window is to be flashed.</param>
        public static void FlashWindowEx(IntPtr WindowHandle, FlashWindowFlags Flags, uint Count, TimeSpan Timeout)
        {
            // Check if the handle is valid
            HandleManipulator.ValidateAsArgument(WindowHandle, "windowHandle");

            // Create the data structure
            var FlashInfo = new FlashInfo
                                {
                                    Size = Marshal.SizeOf(typeof(FlashInfo)),
                                    Hwnd = WindowHandle,
                                    Flags = Flags,
                                    Count = Count,
                                    Timeout = Convert.ToInt32(Timeout.TotalMilliseconds)
                                };

            // Flash the window
            NativeMethods.FlashWindowEx(ref FlashInfo);
        }

        /// <summary>
        /// Flashes the specified window. It does not change the active state of the window. The function uses the default cursor blink rate.
        /// </summary>
        /// <param name="WindowHandle">A handle to the window to be flashed. The window can be either opened or minimized.</param>
        /// <param name="Flags">The flash status.</param>
        /// <param name="Count">The number of times to flash the window.</param>
        public static void FlashWindowEx(IntPtr WindowHandle, FlashWindowFlags Flags, uint Count)
        {
            WindowCore.FlashWindowEx(WindowHandle, Flags, Count, TimeSpan.FromMilliseconds(0));
        }

        /// <summary>
        /// Flashes the specified window. It does not change the active state of the window. The function uses the default cursor blink rate.
        /// </summary>
        /// <param name="WindowHandle">A handle to the window to be flashed. The window can be either opened or minimized.</param>
        /// <param name="Flags">The flash status.</param>
        public static void FlashWindowEx(IntPtr WindowHandle, FlashWindowFlags Flags)
        {
            WindowCore.FlashWindowEx(WindowHandle, Flags, 0);
        }

        /// <summary>
        /// Translates (maps) a virtual-key code into a scan code or character value, or translates a scan code into a virtual-key code.
        /// To specify a handle to the keyboard layout to use for translating the specified code, use the MapVirtualKeyEx function.
        /// </summary>
        /// <param name="Key">
        /// The virtual key code or scan code for a key. How this value is interpreted depends on the value of the uMapType parameter.
        /// </param>
        /// <param name="Translation">
        /// The translation to be performed. The value of this parameter depends on the value of the uCode parameter.
        /// </param>
        /// <returns>
        /// The return value is either a scan code, a virtual-key code, or a character value, depending on the value of uCode and uMapType. 
        /// If there is no translation, the return value is zero.
        /// </returns>
        public static uint MapVirtualKey(uint Key, TranslationTypes Translation)
        {
            return NativeMethods.MapVirtualKey(Key, Translation);
        }

        /// <summary>
        /// Translates (maps) a virtual-key code into a scan code or character value, or translates a scan code into a virtual-key code.
        /// To specify a handle to the keyboard layout to use for translating the specified code, use the MapVirtualKeyEx function.
        /// </summary>
        /// <param name="Key">
        /// The virtual key code for a key. How this value is interpreted depends on the value of the uMapType parameter.
        /// </param>
        /// <param name="Translation">
        /// The translation to be performed. The value of this parameter depends on the value of the uCode parameter.
        /// </param>
        /// <returns>
        /// The return value is either a scan code, a virtual-key code, or a character value, depending on the value of uCode and uMapType. 
        /// If there is no translation, the return value is zero.
        /// </returns>
        public static uint MapVirtualKey(Keys Key, TranslationTypes Translation)
        {
            return WindowCore.MapVirtualKey((uint)Key, Translation);
        }

        /// <summary>
        /// Places (posts) a message in the message queue associated with the thread that created the specified window and returns without waiting for the thread to process the message.
        /// </summary>
        /// <param name="WindowHandle">A handle to the window whose window procedure is to receive the message. The following values have special meanings.</param>
        /// <param name="Message">The message to be posted.</param>
        /// <param name="WParam">Additional message-specific information.</param>
        /// <param name="LParam">Additional message-specific information.</param>
        public static void PostMessage(IntPtr WindowHandle, uint Message, UIntPtr WParam, UIntPtr LParam)
        {
            // Check if the handle is valid
            HandleManipulator.ValidateAsArgument(WindowHandle, "windowHandle");

            // Post the message
            if (!NativeMethods.PostMessage(WindowHandle, Message, WParam, LParam))
            {
                throw new Win32Exception(string.Format("Couldn't post the message '{0}'.", Message));
            }
        }

        /// <summary>
        /// Places (posts) a message in the message queue associated with the thread that created the specified window and returns without waiting for the thread to process the message.
        /// </summary>
        /// <param name="WindowHandle">A handle to the window whose window procedure is to receive the message. The following values have special meanings.</param>
        /// <param name="Message">The message to be posted.</param>
        /// <param name="WParam">Additional message-specific information.</param>
        /// <param name="LParam">Additional message-specific information.</param>
        public static void PostMessage(IntPtr WindowHandle, WindowsMessages Message, UIntPtr WParam, UIntPtr LParam)
        {
            WindowCore.PostMessage(WindowHandle, (uint)Message, WParam, LParam);
        }

        /// <summary>
        /// Synthesizes keystrokes, mouse motions, and button clicks.
        /// </summary>
        /// <param name="Inputs">An array of <see cref="Input"/> structures. Each structure represents an event to be inserted into the keyboard or mouse input stream.</param>
        public static void SendInput(Input[] Inputs)
        {
            // Check if the array passed in parameter is not empty
            if (Inputs != null && Inputs.Length != 0)
            {
                if (NativeMethods.SendInput(Inputs.Length, Inputs, MarshalType<Input>.Size) == 0)
                {
                    throw new Win32Exception("Couldn't send the inputs.");
                }
            }
            else
            {
                throw new ArgumentException("The parameter cannot be null or empty.", "Inputs");
            }
        }

        /// <summary>
        /// Synthesizes keystrokes, mouse motions, and button clicks.
        /// </summary>
        /// <param name="Input">A structure represents an event to be inserted into the keyboard or mouse input stream.</param>
        public static void SendInput(Input Input)
        {
            WindowCore.SendInput(
                new[]
                    {
                        Input
                    });
        }

        /// <summary>
        /// Sends the specified message to a window or windows.
        /// The SendMessage function calls the window procedure for the specified window and does not return until the window procedure has processed the message.
        /// </summary>
        /// <param name="WindowHandle">A handle to the window whose window procedure will receive the message.</param>
        /// <param name="Message">The message to be sent.</param>
        /// <param name="WParam">Additional message-specific information.</param>
        /// <param name="LParam">Additional message-specific information.</param>
        /// <returns>The return value specifies the result of the message processing; it depends on the message sent.</returns>
        public static IntPtr SendMessage(IntPtr WindowHandle, uint Message, UIntPtr WParam, IntPtr LParam)
        {
            // Check if the handle is valid
            HandleManipulator.ValidateAsArgument(WindowHandle, "windowHandle");

            // Send the message
            return NativeMethods.SendMessage(WindowHandle, Message, WParam, LParam);
        }

        /// <summary>
        /// Sends the specified message to a window or windows.
        /// The SendMessage function calls the window procedure for the specified window and does not return until the window procedure has processed the message.
        /// </summary>
        /// <param name="WindowHandle">A handle to the window whose window procedure will receive the message.</param>
        /// <param name="Message">The message to be sent.</param>
        /// <param name="WParam">Additional message-specific information.</param>
        /// <param name="LParam">Additional message-specific information.</param>
        /// <returns>The return value specifies the result of the message processing; it depends on the message sent.</returns>
        public static IntPtr SendMessage(IntPtr WindowHandle, WindowsMessages Message, UIntPtr WParam, IntPtr LParam)
        {
            return WindowCore.SendMessage(WindowHandle, (uint)Message, WParam, LParam);
        }

        /// <summary>
        /// Brings the thread that created the specified window into the foreground and activates the window. 
        /// The window is restored if minimized. Performs no action if the window is already activated.
        /// </summary>
        /// <param name="WindowHandle">A handle to the window that should be activated and brought to the foreground.</param>
        /// <returns>
        /// If the window was brought to the foreground, the return value is <c>true</c>, otherwise the return value is <c>false</c>.
        /// </returns>
        public static void SetForegroundWindow(IntPtr WindowHandle)
        {
            // Check if the handle is valid
            HandleManipulator.ValidateAsArgument(WindowHandle, "windowHandle");

            // If the window is already activated, do nothing
            if (WindowCore.GetForegroundWindow() == WindowHandle)
            {
                return;
            }

            // Restore the window if minimized
            WindowCore.ShowWindow(WindowHandle, WindowStates.Restore);

            // Activate the window
            if (!NativeMethods.SetForegroundWindow(WindowHandle))
            {
                throw new ApplicationException("Couldn't set the window to foreground.");
            }
        }

        /// <summary>
        /// Sets the current position and size of the specified window.
        /// </summary>
        /// <param name="WindowHandle">A handle to the window.</param>
        /// <param name="Left">The x-coordinate of the upper-left corner of the window.</param>
        /// <param name="Top">The y-coordinate of the upper-left corner of the window.</param>
        /// <param name="Height">The height of the window.</param>
        /// <param name="Width">The width of the window.</param>
        public static void SetWindowPlacement(IntPtr WindowHandle, int Left, int Top, int Height, int Width)
        {
            // Check if the handle is valid
            HandleManipulator.ValidateAsArgument(WindowHandle, "windowHandle");

            // Get a WindowPlacement structure of the current window
            var placement = WindowCore.GetWindowPlacement(WindowHandle);

            // Set the values
            placement.NormalPosition.Left = Left;
            placement.NormalPosition.Top = Top;
            placement.NormalPosition.Height = Height;
            placement.NormalPosition.Width = Width;

            // Set the window placement
            WindowCore.SetWindowPlacement(WindowHandle, placement);
        }

        /// <summary>
        /// Sets the show state and the restored, minimized, and maximized positions of the specified window.
        /// </summary>
        /// <param name="WindowHandle">A handle to the window.</param>
        /// <param name="Placement">A pointer to the <see cref="WindowPlacement"/> structure that specifies the new show state and window positions.</param>
        public static void SetWindowPlacement(IntPtr WindowHandle, WindowPlacement Placement)
        {
            // Check if the handle is valid
            HandleManipulator.ValidateAsArgument(WindowHandle, "windowHandle");

            // If the debugger is attached and the state of the window is ShowDefault, there's an issue where the window disappears
            if (Debugger.IsAttached && Placement.ShowCmd == WindowStates.ShowNormal)
            {
                Placement.ShowCmd = WindowStates.Restore;
            }

            // Set the window placement
            if (!NativeMethods.SetWindowPlacement(WindowHandle, ref Placement))
            {
                throw new Win32Exception("Couldn't set the window placement.");
            }
        }

        /// <summary>
        /// Sets the text of the specified window's title bar.
        /// </summary>
        /// <param name="WindowHandle">A handle to the window whose text is to be changed.</param>
        /// <param name="Title">The new title text.</param>
        public static void SetWindowText(IntPtr WindowHandle, string Title)
        {
            // Check if the handle is valid
            HandleManipulator.ValidateAsArgument(WindowHandle, "windowHandle");

            // Set the text of the window's title bar
            if (!NativeMethods.SetWindowText(WindowHandle, Title))
            {
                throw new Win32Exception("Couldn't set the text of the window's title bar.");
            }
        }

        /// <summary>
        /// Sets the specified window's show state.
        /// </summary>
        /// <param name="WindowHandle">A handle to the window.</param>
        /// <param name="State">Controls how the window is to be shown.</param>
        /// <returns>If the window was previously visible, the return value is <c>true</c>, otherwise the return value is <c>false</c>.</returns>
        public static bool ShowWindow(IntPtr WindowHandle, WindowStates State)
        {
            // Check if the handle is valid
            HandleManipulator.ValidateAsArgument(WindowHandle, "windowHandle");

            // Change the state of the window
            return NativeMethods.ShowWindow(WindowHandle, State);
        }
    }
}