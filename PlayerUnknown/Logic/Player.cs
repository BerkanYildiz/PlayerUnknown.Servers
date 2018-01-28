namespace PlayerUnknown.Logic
{
    using System;

    public static class Player
    {
        private static Weapon Weapon;

        /// <summary>
        /// Gets the <see cref="Weapon"/> the <see cref="Player"/> is currently using.
        /// </summary>
        public static Weapon GetWeapon()
        {
            if (Player.Weapon != null)
            {
                return Player.Weapon;
            }

            return null;
        }

        /// <summary>
        /// Sets the weapon.
        /// </summary>
        /// <param name="Weapon">The weapon.</param>
        /// <exception cref="ArgumentNullException">Weapon is null.</exception>
        public static void SetWeapon(Weapon NewWeapon)
        {
            if (NewWeapon == null)
            {
                throw new ArgumentNullException(nameof(NewWeapon) + " == null at Player.SetWeapon(NewWeapon).");
            }

            Player.Weapon = NewWeapon;
        }
    }
}
