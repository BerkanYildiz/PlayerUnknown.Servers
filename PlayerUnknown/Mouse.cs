namespace PlayerUnknown
{
    using System;
    using System.Drawing;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;

    public static class Mouse
    {
        [DllImport("user32.dll")]
        private static extern bool GetCursorPos(out Point Point);

        [DllImport("user32.dll")]
        private static extern bool SetCursorPos(int X, int Y);

        [DllImport("user32.dll")]
        public static extern void mouse_event(int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo);

        [Flags]
        public enum MouseEventFlags
        {
            LeftDown    = 0x00000002,
            LeftUp      = 0x00000004,
            MiddleDown  = 0x00000020,
            MiddleUp    = 0x00000040,
            Move        = 0x00000001,
            Absolute    = 0x00008000,
            RightDown   = 0x00000008,
            RightUp     = 0x00000010
        }

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
        /// <param name="SimulateEvent">If set to true, simulates a mouse_event.</param>
        public static void SetPosition(Point NewPosition, bool SimulateEvent = true)
        {
            if (NewPosition == null)
            {
                throw new ArgumentNullException(nameof(NewPosition) + " == null at Mouse.SetPosition(NewPosition).");
            }

            // Cursor.Position = NewPosition;

            if (SimulateEvent)
            {
                mouse_event((int) MouseEventFlags.Move, 3, 3, 0, 0);
            }
        }
        
        /// <summary>
        /// Moves the position of the mouse cursor,
        /// using the specified <see cref="Point"/>.
        /// </summary>
        /// <param name="NewPosition">The new position.</param>
        /// <param name="SimulateEvent">If set to true, simulates a mouse_event.</param>
        public static void MovePosition(int DiffX, int DiffY)
        {
            mouse_event((int) MouseEventFlags.Move, DiffX, DiffY, 0, 0);
        }
    }
}
