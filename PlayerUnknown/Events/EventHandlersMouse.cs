namespace PlayerUnknown.Events
{
    using PlayerUnknown.Events.Hooks;

    public static class EventHandlersMouse
    {
        public static MouseHook Mouse;

        /// <summary>
        /// Runs this instance.
        /// </summary>
        public static void Run()
        {
            EventHandlersMouse.Mouse = new MouseHook();
        }

        /// <summary>
        /// Hooks this instance.
        /// </summary>
        public static void Hook()
        {
            EventHandlersMouse.Mouse.Install();
        }
    }
}
