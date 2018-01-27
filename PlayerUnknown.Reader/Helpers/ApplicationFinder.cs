namespace PlayerUnknown.Reader.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;

    using PlayerUnknown.Reader.Windows;

    /// <summary>
    /// Static helper class providing tools for finding applications.
    /// </summary>
    public static class ApplicationFinder
    {
        /// <summary>
        /// Gets all top-level windows on the screen.
        /// </summary>
        public static IEnumerable<IntPtr> TopLevelWindows
        {
            get
            {
                return WindowCore.EnumTopLevelWindows();
            }
        }

        /// <summary>
        /// Gets all the windows on the screen.
        /// </summary>
        public static IEnumerable<IntPtr> Windows
        {
            get
            {
                return WindowCore.EnumAllWindows();
            }
        }

        /// <summary>
        /// Returns a new <see cref="Process"/> component, given the identifier of a process.
        /// </summary>
        /// <param name="ProcessId">The system-unique identifier of a process resource.</param>
        /// <returns>A <see cref="Process"/> component that is associated with the local process resource identified by the processId parameter.</returns>
        public static Process FromProcessId(int ProcessId)
        {
            return Process.GetProcessById(ProcessId);
        }

        /// <summary>
        /// Creates an collection of new <see cref="Process"/> components and associates them with all the process resources that share the specified process name.
        /// </summary>
        /// <param name="ProcessName">The friendly name of the process.</param>
        /// <returns>A collection of type <see cref="Process"/> that represents the process resources running the specified application or file.</returns>
        public static IEnumerable<Process> FromProcessName(string ProcessName)
        {
            return Process.GetProcessesByName(ProcessName);
        }

        /// <summary>
        /// Creates a collection of new <see cref="Process"/> components and associates them with all the process resources that share the specified class name.
        /// </summary>
        /// <param name="ClassName">The class name string.</param>
        /// <returns>A collection of type <see cref="Process"/> that represents the process resources running the specified application or file.</returns>
        public static IEnumerable<Process> FromWindowClassName(string ClassName)
        {
            return ApplicationFinder.Windows.Where(Window => WindowCore.GetClassName(Window) == ClassName).Select(ApplicationFinder.FromWindowHandle);
        }

        /// <summary>
        /// Retrieves a new <see cref="Process"/> component that created the window. 
        /// </summary>
        /// <param name="WindowHandle">A handle to the window.</param>
        /// <returns>A <see cref="Process"/>A <see cref="Process"/> component that is associated with the specified window handle.</returns>
        public static Process FromWindowHandle(IntPtr WindowHandle)
        {
            return ApplicationFinder.FromProcessId(WindowCore.GetWindowProcessId(WindowHandle));
        }

        /// <summary>
        /// Creates a collection of new <see cref="Process"/> components and associates them with all the process resources that share the specified window title.
        /// </summary>
        /// <param name="WindowTitle">The window title string.</param>
        /// <returns>A collection of type <see cref="Process"/> that represents the process resources running the specified application or file.</returns>
        public static IEnumerable<Process> FromWindowTitle(string WindowTitle)
        {
            return ApplicationFinder.Windows.Where(Window => WindowCore.GetWindowText(Window) == WindowTitle).Select(ApplicationFinder.FromWindowHandle);
        }

        /// <summary>
        /// Creates a collection of new <see cref="Process"/> components and associates them with all the process resources that contain the specified window title.
        /// </summary>
        /// <param name="WindowTitle">A part a window title string.</param>
        /// <returns>A collection of type <see cref="Process"/> that represents the process resources running the specified application or file.</returns>
        public static IEnumerable<Process> FromWindowTitleContains(string WindowTitle)
        {
            return ApplicationFinder.Windows.Where(Window => WindowCore.GetWindowText(Window).Contains(WindowTitle)).Select(ApplicationFinder.FromWindowHandle);
        }
    }
}