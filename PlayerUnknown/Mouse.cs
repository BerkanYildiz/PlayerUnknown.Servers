namespace PlayerUnknown
{
    using System;
    using System.Drawing;
    using System.Windows.Forms;

    public static class Mouse
    {
        /* [DllImport("user32.dll")]
        private static extern bool GetCursorPos(out Point Point);

        [DllImport("user32.dll")]
        private static extern bool SetCursorPos(int X, int Y);

        [DllImport("user32.dll")]
        public static extern void mouse_event(int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo);

        public const int MouseLeftDown  = 0x02;
        public const int MouseLeftUp    = 0x04; */

        /// <summary>
        /// Gets the position of the mouse cursor.
        /// </summary>
        public static Point GetPosition()
        {
            /* Point Position = new Point();

            if (GetCursorPos(out Position))
            {
                return Position;
            }

            return Position; */

            return Cursor.Position;
        }

        /// <summary>
        /// Sets the position of the mouse cursor,
        /// using the specified <see cref="Point"/>.
        /// </summary>
        /// <param name="NewPosition">The new position.</param>
        public static void SetPosition(Point NewPosition, bool SimulateClick = true)
        {
            if (NewPosition == null)
            {
                throw new ArgumentNullException(nameof(NewPosition) + " == null at Mouse.SetPosition(NewPosition).");
            }

            Cursor.Position = NewPosition;
        }
    }
}
