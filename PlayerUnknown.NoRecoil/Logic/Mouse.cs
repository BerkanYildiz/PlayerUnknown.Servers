namespace PlayerUnknown.NoRecoil.Logic
{
    using System;
    using System.Drawing;
    using System.Windows.Forms;

    public static class Mouse
    {
        /// <summary>
        /// Gets the position of the mouse cursor.
        /// </summary>
        public static Point GetPosition()
        {
            return Cursor.Position;
        }

        /// <summary>
        /// Sets the position of the mouse cursor,
        /// using the specified <see cref="Point"/>.
        /// </summary>
        /// <param name="NewPosition">The new position.</param>
        public static void SetPosition(Point NewPosition)
        {
            if (NewPosition == null)
            {
                throw new ArgumentNullException(nameof(NewPosition) + " == null at Mouse.SetPosition(NewPosition).");
            }

            Cursor.Position = NewPosition;
        }
    }
}
