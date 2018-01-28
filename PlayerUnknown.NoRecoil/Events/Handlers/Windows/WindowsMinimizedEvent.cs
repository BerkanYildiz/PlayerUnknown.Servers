namespace PlayerUnknown.NoRecoil.Events.Handlers.Windows
{
    public class WindowsMinimizedEvent : Event
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WindowsMinimizedEvent"/> class.
        /// </summary>
        public WindowsMinimizedEvent()
        {
            Logging.Info(this.GetType().BaseType, "PUBG has been minimized.");
        }
    }
}
