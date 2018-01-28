namespace PlayerUnknown.Events.Handlers
{
    public class PubgDetachedEvent : Event
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PubgDetachedEvent"/> class.
        /// </summary>
        public PubgDetachedEvent()
        {
            Logging.Info(this.GetType().BaseType, "PUBG has been detached.");
        }
    }
}
