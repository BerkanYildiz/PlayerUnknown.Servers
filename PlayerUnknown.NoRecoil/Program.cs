namespace PlayerUnknown.NoRecoil
{
    using PlayerUnknown.Events;
    using PlayerUnknown.Events.Hooks;
    using PlayerUnknown.Logic;
    using PlayerUnknown.Logic.Weapons;

    public static class Program
    {
        /// <summary>
        /// Defines the entry point of the application.
        /// </summary>
        public static void Main()
        {
            PUBG.Initialize();
            PUBG.Attach();

            if (PUBG.IsAttached && PUBG.IsRunning)
            {
                Player.SetWeapon(new Ak47());

                EventHandlersMouse.Mouse.LeftButtonDown += OnMouseDown;
                EventHandlersMouse.Mouse.LeftButtonUp   += OnMouseUp;

                EventHandlersMouse.Hook();
            }

            NoRecoil.Run().Wait();
        }

        /// <summary>
        /// Called when a left click is detected.
        /// </summary>
        /// <param name="Mouse">The mouse.</param>
        private static void OnMouseDown(MouseHook.Msllhookstruct Mouse)
        {
            NoRecoil.IsEnabled = true;
        }

        /// <summary>
        /// Called when a left click is not detected anymore.
        /// </summary>
        /// <param name="Mouse">The mouse.</param>
        private static void OnMouseUp(MouseHook.Msllhookstruct Mouse)
        {
            NoRecoil.IsEnabled = false;
        }
    }
}
