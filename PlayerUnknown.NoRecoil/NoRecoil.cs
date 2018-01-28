namespace PlayerUnknown.NoRecoil
{
    using System;
    using System.Drawing;
    using System.Threading.Tasks;

    using PlayerUnknown.Logic;

    public static class NoRecoil
    {
        /// <summary>
        /// Gets or sets a value indicating whether the <see cref="NoRecoil"/> is enabled.
        /// </summary>
        public static bool IsEnabled;

        /// <summary>
        /// Enables the no recoil.
        /// </summary>
        public static async Task Run()
        {
            if (NoRecoil.IsEnabled)
            {
                return;
            }
            else
            {
                NoRecoil.IsEnabled = true;
            }

            while (true)
            {
                if (NoRecoil.IsEnabled && PUBG.IsOnScreen)
                {
                    var Weapon = Player.GetWeapon();

                    if (Weapon != null)
                    {
                        if (Weapon.IsRecoilEnabled)
                        {
                            DoRecoil(Weapon.RecoilRate);

                            if (Weapon.FireRate > 0)
                            {
                                await Task.Delay(Weapon.FireRate);
                            }
                        }
                    }
                }
                else
                {
                    await Task.Delay(50);
                }
            }
        }

        /// <summary>
        /// Does the recoil hack and move the mouse with some randomness.
        /// </summary>
        /// <param name="RecoilRate">The recoil rate.</param>
        private static void DoRecoil(int RecoilRate)
        {
            var CurrentPosition = Mouse.GetPosition();

            var CurrentX        = CurrentPosition.X;
            var CurrentY        = CurrentPosition.Y;

            Random Random       = new Random();

            var NextX           = Random.Next(CurrentX - 3, CurrentX + 3);
            var NextY           = Random.Next(CurrentY + RecoilRate, CurrentY + (RecoilRate * 2));

            var NextPosition    = new Point(NextX, NextY);

            Mouse.SetPosition(NextPosition);
        }
    }
}
