namespace PlayerUnknown.Reader.Windows.Keyboard
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    using PlayerUnknown.Reader.Native;

    /// <summary>
    /// Abstract class defining a virtual keyboard.
    /// </summary>
    public abstract class BaseKeyboard
    {
        /// <summary>
        /// The collection storing the current pressed keys.
        /// </summary>
        protected static readonly List<Tuple<IntPtr, Keys>> PressedKeys = new List<Tuple<IntPtr, Keys>>();

        /// <summary>
        /// The reference of the <see cref="RemoteWindow"/> object.
        /// </summary>
        protected readonly RemoteWindow Window;

        /// <summary>
        /// Initializes a new instance of a child of the <see cref="BaseKeyboard"/> class.
        /// </summary>
        /// <param name="Window">The reference of the <see cref="RemoteWindow"/> object.</param>
        protected BaseKeyboard(RemoteWindow Window)
        {
            // Save the parameter
            this.Window = Window;
        }

        /// <summary>
        /// Presses the specified virtual key to the window.
        /// </summary>
        /// <param name="Key">The virtual key to press.</param>
        public abstract void Press(Keys Key);

        /// <summary>
        /// Writes the specified character to the window.
        /// </summary>
        /// <param name="Character">The character to write.</param>
        public abstract void Write(char Character);

        /// <summary>
        /// Releases the specified virtual key to the window.
        /// </summary>
        /// <param name="Key">The virtual key to release.</param>
        public virtual void Release(Keys Key)
        {
            // Create the tuple
            var tuple = Tuple.Create(this.Window.Handle, Key);

            // If the key is pressed with an interval
            if (BaseKeyboard.PressedKeys.Contains(tuple))
            {
                BaseKeyboard.PressedKeys.Remove(tuple);
            }
        }

        /// <summary>
        /// Presses the specified virtual key to the window at a specified interval.
        /// </summary>
        /// <param name="Key">The virtual key to press.</param>
        /// <param name="Interval">The interval between the key activations.</param>
        public void Press(Keys Key, TimeSpan Interval)
        {
            // Create the tuple
            var tuple = Tuple.Create(this.Window.Handle, Key);

            // If the key is already pressed
            if (BaseKeyboard.PressedKeys.Contains(tuple))
            {
                return;
            }

            // Add the key to the collection
            BaseKeyboard.PressedKeys.Add(tuple);

            // Start a new task to press the key at the specified interval
            Task.Run(
                async () =>
                    {
                        // While the key must be pressed
                        while (BaseKeyboard.PressedKeys.Contains(tuple))
                        {
                            // Press the key
                            this.Press(Key);

                            // Wait the interval
                            await Task.Delay(Interval);
                        }
                    });
        }

        /// <summary>
        /// Presses and releaes the specified virtual key to the window.
        /// </summary>
        /// <param name="Key">The virtual key to press and release.</param>
        public void PressRelease(Keys Key)
        {
            this.Press(Key);
            Thread.Sleep(10);
            this.Release(Key);
        }

        /// <summary>
        /// Writes the text representation of the specified array of objects to the window using the specified format information.
        /// </summary>
        /// <param name="Text">A composite format string.</param>
        /// <param name="Args">An array of objects to write using format.</param>
        public void Write(string Text, params object[] Args)
        {
            foreach (var character in string.Format(Text, Args))
            {
                this.Write(character);
            }
        }
    }
}