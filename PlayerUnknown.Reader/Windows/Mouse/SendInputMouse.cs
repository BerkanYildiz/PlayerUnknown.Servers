namespace PlayerUnknown.Reader.Windows.Mouse
{
    using PlayerUnknown.Reader.Native;

    /// <summary>
    /// Class defining a virtual mouse using the API SendInput.
    /// </summary>
    public class SendInputMouse : BaseMouse
    {
        /// <summary>
        /// Initializes a new instance of a child of the <see cref="SendInputMouse"/> class.
        /// </summary>
        /// <param name="Window">The reference of the <see cref="RemoteWindow"/> object.</param>
        public SendInputMouse(RemoteWindow Window)
            : base(Window)
        {
        }

        /// <summary>
        /// Presses the left button of the mouse at the current cursor position.
        /// </summary>
        public override void PressLeft()
        {
            var input = this.CreateInput();
            input.Mouse.Flags = MouseFlags.LeftDown;
            WindowCore.SendInput(input);
        }

        /// <summary>
        /// Presses the middle button of the mouse at the current cursor position.
        /// </summary>
        public override void PressMiddle()
        {
            var input = this.CreateInput();
            input.Mouse.Flags = MouseFlags.MiddleDown;
            WindowCore.SendInput(input);
        }

        /// <summary>
        /// Presses the right button of the mouse at the current cursor position.
        /// </summary>
        public override void PressRight()
        {
            var input = this.CreateInput();
            input.Mouse.Flags = MouseFlags.RightDown;
            WindowCore.SendInput(input);
        }

        /// <summary>
        /// Releases the left button of the mouse at the current cursor position.
        /// </summary>
        public override void ReleaseLeft()
        {
            var input = this.CreateInput();
            input.Mouse.Flags = MouseFlags.LeftUp;
            WindowCore.SendInput(input);
        }

        /// <summary>
        /// Releases the middle button of the mouse at the current cursor position.
        /// </summary>
        public override void ReleaseMiddle()
        {
            var input = this.CreateInput();
            input.Mouse.Flags = MouseFlags.MiddleUp;
            WindowCore.SendInput(input);
        }

        /// <summary>
        /// Releases the right button of the mouse at the current cursor position.
        /// </summary>
        public override void ReleaseRight()
        {
            var input = this.CreateInput();
            input.Mouse.Flags = MouseFlags.RightUp;
            WindowCore.SendInput(input);
        }

        /// <summary>
        /// Scrolls horizontally using the wheel of the mouse at the current cursor position.
        /// </summary>
        /// <param name="Delta">The amount of wheel movement.</param>
        public override void ScrollHorizontally(int Delta = 120)
        {
            var input = this.CreateInput();
            input.Mouse.Flags = MouseFlags.HWheel;
            input.Mouse.MouseData = Delta;
            WindowCore.SendInput(input);
        }

        /// <summary>
        /// Scrolls vertically using the wheel of the mouse at the current cursor position.
        /// </summary>
        /// <param name="Delta">The amount of wheel movement.</param>
        public override void ScrollVertically(int Delta = 120)
        {
            var input = this.CreateInput();
            input.Mouse.Flags = MouseFlags.Wheel;
            input.Mouse.MouseData = Delta;
            WindowCore.SendInput(input);
        }

        /// <summary>
        /// Moves the cursor at the specified coordinate.
        /// </summary>
        /// <param name="X">The x-coordinate.</param>
        /// <param name="Y">The y-coordinate.</param>
        protected override void MoveToAbsolute(int X, int Y)
        {
            var input = this.CreateInput();
            input.Mouse.DeltaX = this.CalculateAbsoluteCoordinateX(X);
            input.Mouse.DeltaY = this.CalculateAbsoluteCoordinateY(Y);
            input.Mouse.Flags = MouseFlags.Move | MouseFlags.Absolute;
            input.Mouse.MouseData = 0;
            WindowCore.SendInput(input);
        }

        /// <summary>
        /// Calculates the x-coordinate with the system metric.
        /// </summary>
        private int CalculateAbsoluteCoordinateX(int X)
        {
            return X * 65536 / NativeMethods.GetSystemMetrics(SystemMetrics.CxScreen);
        }

        /// <summary>
        /// Calculates the y-coordinate with the system metric.
        /// </summary>
        private int CalculateAbsoluteCoordinateY(int Y)
        {
            return Y * 65536 / NativeMethods.GetSystemMetrics(SystemMetrics.CyScreen);
        }

        /// <summary>
        /// Create an <see cref="Input"/> structure for mouse event.
        /// </summary>
        private Input CreateInput()
        {
            return new Input(InputTypes.Mouse);
        }
    }
}