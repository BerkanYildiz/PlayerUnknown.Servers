namespace PlayerUnknown.NoRecoil.Logic
{
    public class Weapon
    {
        /// <summary>
        /// Gets at which speed we're gonna decrease the cursor X position.
        /// </summary>
        public virtual int FireRate
        {
            get;
        }

        /// <summary>
        /// Gets how much pixels we're gonna remove for the cursor X position.
        /// </summary>
        public virtual int RecoilRate
        {
            get;
        }

        /// <summary>
        /// Gets a value indicating whether the no recoil is enabled.
        /// </summary>
        public virtual bool IsRecoilEnabled
        {
            get
            {
                return true;
            }
        }
    }
}
