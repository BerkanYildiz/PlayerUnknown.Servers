namespace PlayerUnknown.Reader.Windows
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using PlayerUnknown.Reader.Internals;

    /// <summary>
    /// Class providing tools for manipulating windows.
    /// </summary>
    public class WindowFactory : IFactory
    {
        /// <summary>
        /// The reference of the <see cref="BattleGroundMemory"/> object.
        /// </summary>
        private readonly BattleGroundMemory BattleGroundMemory;

        /// <summary>
        /// Initializes a new instance of the <see cref="WindowFactory"/> class.
        /// </summary>
        /// <param name="BattleGroundMemory">The reference of the <see cref="BattleGroundMemory"/> object.</param>
        internal WindowFactory(BattleGroundMemory BattleGroundMemory)
        {
            // Save the parameter
            this.BattleGroundMemory = BattleGroundMemory;
        }

        /// <summary>
        /// Gets all the child windows that belong to the application.
        /// </summary>
        public IEnumerable<RemoteWindow> ChildWindows
        {
            get
            {
                return this.ChildWindowHandles.Select(Handle => new RemoteWindow(this.BattleGroundMemory, Handle));
            }
        }

        /// <summary>
        /// Gets the main window of the application.
        /// </summary>
        public RemoteWindow MainWindow
        {
            get
            {
                return new RemoteWindow(this.BattleGroundMemory, this.MainWindowHandle);
            }
        }

        /// <summary>
        /// Gets the main window handle of the application.
        /// </summary>
        public IntPtr MainWindowHandle
        {
            get
            {
                return this.BattleGroundMemory.Native.MainWindowHandle;
            }
        }

        /// <summary>
        /// Gets all the windows that belong to the application.
        /// </summary>
        public IEnumerable<RemoteWindow> RemoteWindows
        {
            get
            {
                return this.WindowHandles.Select(Handle => new RemoteWindow(this.BattleGroundMemory, Handle));
            }
        }

        /// <summary>
        /// Gets all the child window handles that belong to the application.
        /// </summary>
        internal IEnumerable<IntPtr> ChildWindowHandles
        {
            get
            {
                return WindowCore.EnumChildWindows(this.BattleGroundMemory.Native.MainWindowHandle);
            }
        }

        /// <summary>
        /// Gets all the window handles that belong to the application.
        /// </summary>
        internal IEnumerable<IntPtr> WindowHandles
        {
            get
            {
                return new List<IntPtr>(this.ChildWindowHandles)
                           {
                               this.MainWindowHandle
                           };
            }
        }

        /// <summary>
        /// Gets all the windows that have the same specified title.
        /// </summary>
        /// <param name="WindowTitle">The window title string.</param>
        /// <returns>A collection of <see cref="RemoteWindow"/>.</returns>
        public IEnumerable<RemoteWindow> this[string WindowTitle]
        {
            get
            {
                return this.GetWindowsByTitle(WindowTitle);
            }
        }

        /// <summary>
        /// Releases all resources used by the <see cref="WindowFactory"/> object.
        /// </summary>
        public void Dispose()
        {
            // Nothing to dispose... yet
        }

        /// <summary>
        /// Gets all the windows that have the specified class name.
        /// </summary>
        /// <param name="ClassName">The class name string.</param>
        /// <returns>A collection of <see cref="RemoteWindow"/>.</returns>
        public IEnumerable<RemoteWindow> GetWindowsByClassName(string ClassName)
        {
            return this.WindowHandles.Where(Handle => WindowCore.GetClassName(Handle) == ClassName).Select(Handle => new RemoteWindow(this.BattleGroundMemory, Handle));
        }

        /// <summary>
        /// Gets all the windows that have the same specified title.
        /// </summary>
        /// <param name="WindowTitle">The window title string.</param>
        /// <returns>A collection of <see cref="RemoteWindow"/>.</returns>
        public IEnumerable<RemoteWindow> GetWindowsByTitle(string WindowTitle)
        {
            return this.WindowHandles.Where(Handle => WindowCore.GetWindowText(Handle) == WindowTitle).Select(Handle => new RemoteWindow(this.BattleGroundMemory, Handle));
        }

        /// <summary>
        /// Gets all the windows that contain the specified title.
        /// </summary>
        /// <param name="WindowTitle">A part a window title string.</param>
        /// <returns>A collection of <see cref="RemoteWindow"/>.</returns>
        public IEnumerable<RemoteWindow> GetWindowsByTitleContains(string WindowTitle)
        {
            return this.WindowHandles.Where(Handle => WindowCore.GetWindowText(Handle).Contains(WindowTitle)).Select(Handle => new RemoteWindow(this.BattleGroundMemory, Handle));
        }
    }
}