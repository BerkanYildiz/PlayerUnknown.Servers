namespace PlayerUnknown.Native
{
    using System.Runtime.InteropServices;

    /// <summary>
    /// The <see cref="Rectangle"/> structure defines the coordinates of the upper-left and lower-right corners of a rectangle.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct Rectangle
    {
        /// <summary>
        /// The x-coordinate of the upper-left corner of the rectangle.
        /// </summary>
        public int Left;

        /// <summary>
        /// The y-coordinate of the upper-left corner of the rectangle.
        /// </summary>
        public int Top;

        /// <summary>
        /// The x-coordinate of the lower-right corner of the rectangle.
        /// </summary>
        public int Right;

        /// <summary>
        /// The y-coordinate of the lower-right corner of the rectangle.
        /// </summary>
        public int Bottom;

        /// <summary>
        /// Gets or sets the height of the element.
        /// </summary>
        public int Height
        {
            get
            {
                return this.Bottom - this.Top;
            }

            set
            {
                this.Bottom = this.Top + value;
            }
        }

        /// <summary>
        /// Gets or sets the width of the element.
        /// </summary>
        public int Width
        {
            get
            {
                return this.Right - this.Left;
            }

            set
            {
                this.Right = this.Left + value;
            }
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        public override string ToString()
        {
            return string.Format("Left = {0} Top = {1} Height = {2} Width = {3}", this.Left, this.Top, this.Height, this.Width);
        }
    }
}