namespace PlayerUnknown.NoRecoil.Events.Handlers
{
    public class PubgAttachedEvent : Event
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PubgAttachedEvent"/> class.
        /// </summary>
        public PubgAttachedEvent()
        {
            Logging.Info(this.GetType().BaseType, "PUBG has been attached.");
        }
    }
}
