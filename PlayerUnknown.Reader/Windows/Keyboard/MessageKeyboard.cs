namespace PlayerUnknown.Reader.Windows.Keyboard
{
    using System;

    using PlayerUnknown.Reader.Native;

    /// <summary>
    /// Class defining a virtual keyboard using the API Message.
    /// </summary>
    public class MessageKeyboard : BaseKeyboard
    {
        public MessageKeyboard(RemoteWindow Window)
            : base(Window)
        {
        }

        /// <summary>
        /// Presses the specified virtual key to the window.
        /// </summary>
        /// <param name="Key">The virtual key to press.</param>
        public override void Press(Keys Key)
        {
            this.Window.PostMessage(WindowsMessages.KeyDown, new UIntPtr((uint)Key), this.MakeKeyParameter(Key, false));
        }

        /// <summary>
        /// Releases the specified virtual key to the window.
        /// </summary>
        /// <param name="Key">The virtual key to release.</param>
        public override void Release(Keys Key)
        {
            // Call the base function
            base.Release(Key);
            this.Window.PostMessage(WindowsMessages.KeyUp, new UIntPtr((uint)Key), this.MakeKeyParameter(Key, true));
        }

        /// <summary>
        /// Writes the specified character to the window.
        /// </summary>
        /// <param name="Character">The character to write.</param>
        public override void Write(char Character)
        {
            this.Window.PostMessage(WindowsMessages.Char, new UIntPtr(Character), UIntPtr.Zero);
        }

        /// <summary>
        /// Makes the lParam for a key depending on several settings.
        /// </summary>
        /// <param name="Key">
        /// [16-23 bits] The virtual key.
        /// </param>
        /// <param name="KeyUp">
        /// [31 bit] The transition state.
        /// The value is always 0 for a <see cref="WindowsMessages.KeyDown"/> message.
        /// The value is always 1 for a <see cref="WindowsMessages.KeyUp"/> message.
        /// </param>
        /// <param name="FRepeat">
        /// [30 bit] The previous key state.
        /// The value is 1 if the key is down before the message is sent, or it is zero if the key is up.
        /// The value is always 1 for a <see cref="WindowsMessages.KeyUp"/> message.
        /// </param>
        /// <param name="CRepeat">
        /// [0-15 bits] The repeat count for the current message. 
        /// The value is the number of times the keystroke is autorepeated as a result of the user holding down the key.
        /// If the keystroke is held long enough, multiple messages are sent. However, the repeat count is not cumulative.
        /// The repeat count is always 1 for a <see cref="WindowsMessages.KeyUp"/> message.
        /// </param>
        /// <param name="AltDown">
        /// [29 bit] The context code.
        /// The value is always 0 for a <see cref="WindowsMessages.KeyDown"/> message.
        /// The value is always 0 for a <see cref="WindowsMessages.KeyUp"/> message.</param>
        /// <param name="FExtended">
        /// [24 bit] Indicates whether the key is an extended key, such as the right-hand ALT and CTRL keys that appear on 
        /// an enhanced 101- or 102-key keyboard. The value is 1 if it is an extended key; otherwise, it is 0.
        /// </param>
        /// <returns>The return value is the lParam when posting or sending a message regarding key press.</returns>
        /// <remarks>
        /// KeyDown resources: http://msdn.microsoft.com/en-us/library/windows/desktop/ms646280%28v=vs.85%29.aspx
        /// KeyUp resources:  http://msdn.microsoft.com/en-us/library/windows/desktop/ms646281%28v=vs.85%29.aspx
        /// </remarks>
        private UIntPtr MakeKeyParameter(Keys Key, bool KeyUp, bool FRepeat, uint CRepeat, bool AltDown, bool FExtended)
        {
            // Create the result and assign it with the repeat count
            var result = CRepeat;

            // Add the scan code with a left shift operation
            result |= WindowCore.MapVirtualKey(Key, TranslationTypes.VirtualKeyToScanCode) << 16;

            // Does we need to set the extended flag ?
            if (FExtended)
            {
                result |= 0x1000000;
            }

            // Does we need to set the alt flag ?
            if (AltDown)
            {
                result |= 0x20000000;
            }

            // Does we need to set the repeat flag ?
            if (FRepeat)
            {
                result |= 0x40000000;
            }

            // Does we need to set the keyUp flag ?
            if (KeyUp)
            {
                result |= 0x80000000;
            }

            return new UIntPtr(result);
        }

        /// <summary>
        /// Makes the lParam for a key depending on several settings.
        /// </summary>
        /// <param name="Key">The virtual key.</param>
        /// <param name="KeyUp">
        /// The transition state.
        /// The value is always 0 for a <see cref="WindowsMessages.KeyDown"/> message.
        /// The value is always 1 for a <see cref="WindowsMessages.KeyUp"/> message.
        /// </param>
        /// <returns>The return value is the lParam when posting or sending a message regarding key press.</returns>
        private UIntPtr MakeKeyParameter(Keys Key, bool KeyUp)
        {
            return this.MakeKeyParameter(Key, KeyUp, KeyUp, 1, false, false);
        }
    }
}