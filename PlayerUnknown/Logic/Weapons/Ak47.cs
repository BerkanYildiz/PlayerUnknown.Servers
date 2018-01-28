namespace PlayerUnknown.Logic.Weapons
{
    public sealed class Ak47 : Weapon
    {
        /// <summary>
        /// Gets at which speed (miliseconds) we're gonna decrease the cursor X position.
        /// </summary>
        public override int FireRate
        {
            get
            {
                return 100;
            }
        }

        /// <summary>
        /// Gets how much pixels we're gonna remove for the cursor X position.
        /// </summary>
        public override int RecoilRate
        {
            get
            {
                return 3;
            }
        }
    }
}
