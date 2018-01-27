namespace PlayerUnknown.Reader.Windows.Mouse
{
    using System.Threading;

    /// <summary>
    /// Abstract class defining a virtual mouse.
    /// </summary>
    public abstract class BaseMouse
    {
        /// <summary>
        /// The reference of the <see cref="RemoteWindow"/> object.
        /// </summary>
        protected readonly RemoteWindow Window;

        /// <summary>
        /// Initializes a new instance of a child of the <see cref="BaseMouse"/> class.
        /// </summary>
        /// <param name="Window">The reference of the <see cref="RemoteWindow"/> object.</param>
        protected BaseMouse(RemoteWindow Window)
        {
            // Save the parameter
            this.Window = Window;
        }

        /// <summary>
        /// Presses the left button of the mouse at the current cursor position.
        /// </summary>
        public abstract void PressLeft();

        /// <summary>
        /// Presses the middle button of the mouse at the current cursor position.
        /// </summary>
        public abstract void PressMiddle();

        /// <summary>
        /// Presses the right button of the mouse at the current cursor position.
        /// </summary>
        public abstract void PressRight();

        /// <summary>
        /// Releases the left button of the mouse at the current cursor position.
        /// </summary>
        public abstract void ReleaseLeft();

        /// <summary>
        /// Releases the middle button of the mouse at the current cursor position.
        /// </summary>
        public abstract void ReleaseMiddle();

        /// <summary>
        /// Releases the right button of the mouse at the current cursor position.
        /// </summary>
        public abstract void ReleaseRight();

        /// <summary>
        /// Scrolls horizontally using the wheel of the mouse at the current cursor position.
        /// </summary>
        /// <param name="Delta">The amount of wheel movement.</param>
        public abstract void ScrollHorizontally(int Delta = 120);

        /// <summary>
        /// Scrolls vertically using the wheel of the mouse at the current cursor position.
        /// </summary>
        /// <param name="Delta">The amount of wheel movement.</param>
        public abstract void ScrollVertically(int Delta = 120);

        /// <summary>
        /// Clicks the left button of the mouse at the current cursor position.
        /// </summary>
        public void ClickLeft()
        {
            this.PressLeft();
            this.ReleaseLeft();
        }

        /// <summary>
        /// Clicks the middle button of the mouse at the current cursor position.
        /// </summary>
        public void ClickMiddle()
        {
            this.PressMiddle();
            this.ReleaseMiddle();
        }

        /// <summary>
        /// Clicks the right button of the mouse at the current cursor position.
        /// </summary>
        public void ClickRight()
        {
            this.PressRight();
            this.ReleaseRight();
        }

        /// <summary>
        /// Double clicks the left button of the mouse at the current cursor position.
        /// </summary>
        public void DoubleClickLeft()
        {
            this.ClickLeft();
            Thread.Sleep(10);
            this.ClickLeft();
        }

        /// <summary>
        /// Moves the cursor at the specified coordinate from the position of the window.
        /// </summary>
        /// <param name="X">The x-coordinate.</param>
        /// <param name="Y">The y-coordinate.</param>
        public void MoveTo(int X, int Y)
        {
            this.MoveToAbsolute(this.Window.X + X, this.Window.Y + Y);
        }

        /// <summary>
        /// Moves the cursor at the specified coordinate.
        /// </summary>
        /// <param name="X">The x-coordinate.</param>
        /// <param name="Y">The y-coordinate.</param>
        protected abstract void MoveToAbsolute(int X, int Y);
    }
}