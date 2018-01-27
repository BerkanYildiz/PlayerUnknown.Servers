namespace PlayerUnknown.Reader.Windows
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using PlayerUnknown.Reader.Native;
    using PlayerUnknown.Reader.Threading;
    using PlayerUnknown.Reader.Windows.Keyboard;
    using PlayerUnknown.Reader.Windows.Mouse;

    /// <summary>
    /// Class repesenting a window in the remote process.
    /// </summary>
    public class RemoteWindow : IEquatable<RemoteWindow>
    {
        /// <summary>
        /// The reference of the <see cref="BattleGroundMemory"/> object.
        /// </summary>
        protected readonly BattleGroundMemory BattleGroundMemory;

        /// <summary>
        /// Initializes a new instance of the <see cref="RemoteWindow"/> class.
        /// </summary>
        /// <param name="BattleGroundMemory">The reference of the <see cref="BattleGroundMemory"/> object.</param>
        /// <param name="Handle">The handle of a window.</param>
        internal RemoteWindow(BattleGroundMemory BattleGroundMemory, IntPtr Handle)
        {
            // Save the parameters
            this.BattleGroundMemory = BattleGroundMemory;
            this.Handle = Handle;

            // Create the tools
            this.Keyboard = new MessageKeyboard(this);
            this.Mouse = new SendInputMouse(this);
        }

        /// <summary>
        /// Gets all the child windows of this window.
        /// </summary>
        public IEnumerable<RemoteWindow> Children
        {
            get
            {
                return this.ChildrenHandles.Select(Handle => new RemoteWindow(this.BattleGroundMemory, Handle));
            }
        }

        /// <summary>
        /// Gets the class name of the window.
        /// </summary>
        public string ClassName
        {
            get
            {
                return WindowCore.GetClassName(this.Handle);
            }
        }

        /// <summary>
        /// The handle of the window.
        /// </summary>
        /// <remarks>
        /// The type here is not <see cref="SafeMemoryHandle"/> because a window cannot be closed by calling <see cref="NativeMethods.CloseHandle"/>.
        /// For more information, see: http://stackoverflow.com/questions/8507307/why-cant-i-close-the-window-handle-in-my-code.
        /// </remarks>
        public IntPtr Handle
        {
            get;
        }

        /// <summary>
        /// Gets or sets the height of the element.
        /// </summary>
        public int Height
        {
            get
            {
                return this.Placement.NormalPosition.Height;
            }

            set
            {
                var p = this.Placement;
                p.NormalPosition.Height = value;
                this.Placement = p;
            }
        }

        /// <summary>
        /// Gets if the window is currently activated.
        /// </summary>
        public bool IsActivated
        {
            get
            {
                return WindowCore.GetForegroundWindow() == this.Handle;
            }
        }

        /// <summary>
        /// Gets if this is the main window.
        /// </summary>
        public bool IsMainWindow
        {
            get
            {
                return this.BattleGroundMemory.Windows.MainWindow == this;
            }
        }

        /// <summary>
        /// Tools for managing a virtual keyboard in the window.
        /// </summary>
        public BaseKeyboard Keyboard
        {
            get;
            set;
        }

        /// <summary>
        /// Tools for managing a virtual mouse in the window.
        /// </summary>
        public BaseMouse Mouse
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the placement of the window.
        /// </summary>
        public WindowPlacement Placement
        {
            get
            {
                return WindowCore.GetWindowPlacement(this.Handle);
            }

            set
            {
                WindowCore.SetWindowPlacement(this.Handle, value);
            }
        }

        /// <summary>
        /// Gets or sets the specified window's show state.
        /// </summary>
        public WindowStates State
        {
            get
            {
                return this.Placement.ShowCmd;
            }

            set
            {
                WindowCore.ShowWindow(this.Handle, value);
            }
        }

        /// <summary>
        /// Gets or sets the title of the window.
        /// </summary>
        public string Title
        {
            get
            {
                return WindowCore.GetWindowText(this.Handle);
            }

            set
            {
                WindowCore.SetWindowText(this.Handle, value);
            }
        }

        /// <summary>
        /// Gets the thread of the window.
        /// </summary>
        public RemoteThread Thread
        {
            get
            {
                return this.BattleGroundMemory.Threads.GetThreadById(WindowCore.GetWindowThreadId(this.Handle));
            }
        }

        /// <summary>
        /// Gets or sets the width of the element.
        /// </summary>
        public int Width
        {
            get
            {
                return this.Placement.NormalPosition.Width;
            }

            set
            {
                var p = this.Placement;
                p.NormalPosition.Width = value;
                this.Placement = p;
            }
        }

        /// <summary>
        /// Gets or sets the x-coordinate of the window.
        /// </summary>
        public int X
        {
            get
            {
                return this.Placement.NormalPosition.Left;
            }

            set
            {
                var p = this.Placement;
                p.NormalPosition.Right = value + p.NormalPosition.Width;
                p.NormalPosition.Left = value;
                this.Placement = p;
            }
        }

        /// <summary>
        /// Gets or sets the y-coordinate of the window.
        /// </summary>
        public int Y
        {
            get
            {
                return this.Placement.NormalPosition.Top;
            }

            set
            {
                var p = this.Placement;
                p.NormalPosition.Bottom = value + p.NormalPosition.Height;
                p.NormalPosition.Top = value;
                this.Placement = p;
            }
        }

        /// <summary>
        /// Gets all the child window handles of this window.
        /// </summary>
        protected IEnumerable<IntPtr> ChildrenHandles
        {
            get
            {
                return WindowCore.EnumChildWindows(this.Handle);
            }
        }

        /// <summary>
        /// Activates the window.
        /// </summary>
        public void Activate()
        {
            WindowCore.SetForegroundWindow(this.Handle);
        }

        /// <summary>
        /// Closes the window.
        /// </summary>
        public void Close()
        {
            this.PostMessage(WindowsMessages.Close, UIntPtr.Zero, UIntPtr.Zero);
        }

        /// <summary>
        /// Determines whether the specified object is equal to the current object.
        /// </summary>
        public override bool Equals(object Obj)
        {
            if (object.ReferenceEquals(null, Obj))
            {
                return false;
            }

            if (object.ReferenceEquals(this, Obj))
            {
                return true;
            }

            if (Obj.GetType() != this.GetType())
            {
                return false;
            }

            return this.Equals((RemoteWindow)Obj);
        }

        /// <summary>
        /// Returns a value indicating whether this instance is equal to a specified object.
        /// </summary>
        public bool Equals(RemoteWindow Other)
        {
            if (object.ReferenceEquals(null, Other))
            {
                return false;
            }

            if (object.ReferenceEquals(this, Other))
            {
                return true;
            }

            return object.Equals(this.BattleGroundMemory, Other.BattleGroundMemory) && this.Handle.Equals(Other.Handle);
        }

        /// <summary>
        /// Flashes the window one time. It does not change the active state of the window.
        /// </summary>
        public void Flash()
        {
            WindowCore.FlashWindow(this.Handle);
        }

        /// <summary>
        /// Flashes the window. It does not change the active state of the window.
        /// </summary>
        /// <param name="Count">The number of times to flash the window.</param>
        /// <param name="Timeout">The rate at which the window is to be flashed.</param>
        /// <param name="Flags">The flash status.</param>
        public void Flash(uint Count, TimeSpan Timeout, FlashWindowFlags Flags = FlashWindowFlags.All)
        {
            WindowCore.FlashWindowEx(this.Handle, Flags, Count, Timeout);
        }

        /// <summary>
        /// Serves as a hash function for a particular type. 
        /// </summary>
        public override int GetHashCode()
        {
            unchecked
            {
                int HashCode = this.BattleGroundMemory != null ? this.BattleGroundMemory.GetHashCode() : 0;
                HashCode = (HashCode * 397) ^ this.Handle.GetHashCode();
                return HashCode;
            }
        }

        public static bool operator ==(RemoteWindow Left, RemoteWindow Right)
        {
            return object.Equals(Left, Right);
        }

        public static bool operator !=(RemoteWindow Left, RemoteWindow Right)
        {
            return !object.Equals(Left, Right);
        }

        /// <summary>
        /// Places (posts) a message in the message queue associated with the thread that created the window and returns without waiting for the thread to process the message.
        /// </summary>
        /// <param name="Message">The message to be posted.</param>
        /// <param name="WParam">Additional message-specific information.</param>
        /// <param name="LParam">Additional message-specific information.</param>
        public void PostMessage(WindowsMessages Message, UIntPtr WParam, UIntPtr LParam)
        {
            WindowCore.PostMessage(this.Handle, Message, WParam, LParam);
        }

        /// <summary>
        /// Places (posts) a message in the message queue associated with the thread that created the window and returns without waiting for the thread to process the message.
        /// </summary>
        /// <param name="Message">The message to be posted.</param>
        /// <param name="WParam">Additional message-specific information.</param>
        /// <param name="LParam">Additional message-specific information.</param>
        public void PostMessage(uint Message, UIntPtr WParam, UIntPtr LParam)
        {
            WindowCore.PostMessage(this.Handle, Message, WParam, LParam);
        }

        /// <summary>
        /// Sends the specified message to a window or windows.
        /// The SendMessage function calls the window procedure for the specified window and does not return until the window procedure has processed the message.
        /// </summary>
        /// <param name="Message">The message to be sent.</param>
        /// <param name="WParam">Additional message-specific information.</param>
        /// <param name="LParam">Additional message-specific information.</param>
        /// <returns>The return value specifies the result of the message processing; it depends on the message sent.</returns>
        public IntPtr SendMessage(WindowsMessages Message, UIntPtr WParam, IntPtr LParam)
        {
            return WindowCore.SendMessage(this.Handle, Message, WParam, LParam);
        }

        /// <summary>
        /// Sends the specified message to a window or windows.
        /// The SendMessage function calls the window procedure for the specified window and does not return until the window procedure has processed the message.
        /// </summary>
        /// <param name="Message">The message to be sent.</param>
        /// <param name="WParam">Additional message-specific information.</param>
        /// <param name="LParam">Additional message-specific information.</param>
        /// <returns>The return value specifies the result of the message processing; it depends on the message sent.</returns>
        public IntPtr SendMessage(uint Message, UIntPtr WParam, IntPtr LParam)
        {
            return WindowCore.SendMessage(this.Handle, Message, WParam, LParam);
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        public override string ToString()
        {
            return string.Format("Title = {0} ClassName = {1}", this.Title, this.ClassName);
        }
    }
}