namespace PlayerUnknown.Events
{
    using PlayerUnknown.Events.Hooks;

    public static class EventHandlersKeyboard
    {
        public static KeyboardHook Keyboard;

        /// <summary>
        /// Runs this instance.
        /// </summary>
        public static void Run()
        {
            EventHandlersKeyboard.Keyboard = new KeyboardHook();
        }

        /// <summary>
        /// Hooks this instance.
        /// </summary>
        public static void Hook()
        {
            EventHandlersKeyboard.Keyboard.Install();
        }
    }
}
