namespace PlayerUnknown.NoRecoil.Events.Handlers.Weapon
{
    using PlayerUnknown.NoRecoil.Logic;

    public class WeaponChangedEvent : Event
    {
        public Weapon OldWeapon;
        public Weapon NewWeapon;

        /// <summary>
        /// Initializes a new instance of the <see cref="WeaponChangedEvent"/> class.
        /// </summary>
        public WeaponChangedEvent()
        {
            if (this.OldWeapon != null)
            {
                Logging.Info(this.GetType().BaseType, "Player has changed weapon.");
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WeaponChangedEvent"/> class.
        /// </summary>
        /// <param name="OldWeapon">The old weapon.</param>
        /// <param name="NewWeapon">The new weapon.</param>
        public WeaponChangedEvent(Weapon OldWeapon, Weapon NewWeapon) : this()
        {
            this.OldWeapon = OldWeapon;
            this.NewWeapon = NewWeapon;
        }
    }
}
