namespace PlayerUnknown.Events
{
    using System;
    using System.Threading.Tasks;

    using PlayerUnknown.Events.Handlers;
    using PlayerUnknown.Events.Handlers.Weapon;
    using PlayerUnknown.Events.Handlers.Windows;

    public static class EventHandlers
    {
        public static EventHandler<PubgAttachedEvent> OnPubgAttached;
        public static EventHandler<PubgDetachedEvent> OnPubgDetached;

        public static EventHandler<WindowsMaximizedEvent> OnWindowsMaximized;
        public static EventHandler<WindowsMinimizedEvent> OnWindowsMinimized;

        public static EventHandler<WindowsOnScreenEvent> OnWindowsShowed;
        public static EventHandler<WindowsNotOnScreenEvent> OnWindowsNotShowed;

        public static EventHandler<WeaponChangedEvent> OnWeaponChanged;

        // Change Logs

        private static bool IsAttached;
        private static bool IsDetached = true;
        private static bool IsRunning;
        private static bool IsResponding;

        private static bool IsMaximized;
        private static bool IsMinimized;

        private static bool IsOnScreen;

        /// <summary>
        /// Runs this instance.
        /// </summary>
        public static async Task Run()
        {
            while (true)
            {
                if (PUBG.IsAttached)
                {
                    if (EventHandlers.IsAttached == false)
                    {
                        EventHandlers.IsAttached = true;
                        EventHandlers.IsDetached = false;

                        var Event = new PubgAttachedEvent();

                        if (EventHandlers.OnPubgAttached != null)
                        {
                            EventHandlers.OnPubgAttached.Invoke(null, Event);
                        }
                    }
                }
                else if (PUBG.IsDetached)
                {
                    if (EventHandlers.IsDetached == false)
                    {
                        EventHandlers.IsAttached = false;
                        EventHandlers.IsDetached = true;

                        var Event = new PubgDetachedEvent();

                        if (EventHandlers.OnPubgDetached != null)
                        {
                            EventHandlers.OnPubgDetached.Invoke(null, Event);
                        }
                    }
                    else
                    {
                        PUBG.Attach();
                    }
                }

                if (PUBG.IsAttached)
                {
                    if (PUBG.IsMaximized)
                    {
                        if (EventHandlers.IsMaximized == false)
                        {
                            EventHandlers.IsMaximized = true;
                            EventHandlers.IsMinimized = false;

                            var Event = new WindowsMaximizedEvent();

                            if (EventHandlers.OnWindowsMaximized != null)
                            {
                                EventHandlers.OnWindowsMaximized.Invoke(null, Event);
                            }
                        }
                    }

                    if (PUBG.IsMinimized)
                    {
                        if (EventHandlers.IsMinimized == false)
                        {
                            EventHandlers.IsMaximized = false;
                            EventHandlers.IsMinimized = true;

                            var Event = new WindowsMinimizedEvent();

                            if (EventHandlers.OnWindowsMinimized != null)
                            {
                                EventHandlers.OnWindowsMinimized.Invoke(null, Event);
                            }
                        }
                    }

                    if (PUBG.IsOnScreen)
                    {
                        if (EventHandlers.IsOnScreen == false)
                        {
                            EventHandlers.IsOnScreen = true;

                            var Event = new WindowsOnScreenEvent();

                            if (EventHandlers.OnWindowsShowed != null)
                            {
                                EventHandlers.OnWindowsShowed.Invoke(null, Event);
                            }
                        }
                    }
                    else
                    {
                        if (EventHandlers.IsOnScreen)
                        {
                            EventHandlers.IsOnScreen = false;

                            var Event = new WindowsNotOnScreenEvent();

                            if (EventHandlers.OnWindowsNotShowed != null)
                            {
                                EventHandlers.OnWindowsNotShowed.Invoke(null, Event);
                            }
                        }
                    }
                }

                await Task.Delay(100);
            }
        }
    }
}
