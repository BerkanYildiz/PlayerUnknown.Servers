namespace PlayerUnknown.NoRecoil
{
    using System;
    using System.Threading.Tasks;

    using PlayerUnknown.Logic;

    public static class NoRecoil
    {
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="NoRecoil"/> is initialized.
        /// </summary>
        public static bool Initialized
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether the <see cref="NoRecoil"/> is enabled.
        /// </summary>
        public static bool IsEnabled
        {
            get
            {
                return NoRecoil.HasLeftClick && NoRecoil.HasRightClick;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance has left click.
        /// </summary>
        public static bool HasLeftClick;

        /// <summary>
        /// Gets or sets a value indicating whether this instance has right click.
        /// </summary>
        public static bool HasRightClick;

        /// <summary>
        /// Gets or sets the <see cref="Player"/>, currently playing the game.
        /// </summary>
        public static Weapon Weapon;

        /// <summary>
        /// Gets or sets the <see cref="Random"/> used to make the <see cref="NoRecoil"/> a bit random.
        /// </summary>
        public static Random Random;

        /// <summary>
        /// Enables the no recoil.
        /// </summary>
        public static async Task Run()
        {
            if (NoRecoil.Initialized)
            {
                return;
            }

            NoRecoil.Initialized = true;
            NoRecoil.Random      = new Random();

            while (true)
            {
                if (NoRecoil.IsEnabled && PUBG.IsOnScreen)
                {
                    if (Weapon != null)
                    {
                        if (Weapon.IsRecoilEnabled)
                        {
                            await DoRecoil();

                            if (Weapon.FireRate > 0)
                            {
                                await Task.Delay(Weapon.FireRate);
                            }
                        }
                    }
                }
                else
                {
                    await Task.Delay(100);
                }
            }
        }

        /// <summary>
        /// Does the recoil hack and move the mouse with some randomness.
        /// </summary>
        /// <param name="RecoilRate">The recoil rate.</param>
        /// <param name="Smooth">If set to true, moves the mouse pixel per pixel.</param>
        private static async Task DoRecoil(bool Smooth = false)
        {
            var Randomness      = (1 * Weapon.RandomnessMultiplier);

            var DiffX           = Random.Next(-Randomness, Randomness + 1);
            var DiffY           = Random.Next(Weapon.RecoilRate, (Weapon.RecoilRate * 2) + 1);

            var TargetX         = DiffX;
            var TargetY         = DiffY;

            if (Smooth)
            {
                while (true)
                {
                    Mouse.MovePosition((DiffX > 0 ? +1 : -1), (DiffY > 0 ? +1 : -1));

                    if (TargetX > 0)
                    {
                        TargetX = TargetX - 1;
                    }
                    else if (TargetX < 0)
                    {
                        TargetX = TargetX + 1;
                    }

                    if (TargetY > 0)
                    {
                        TargetY = TargetY - 1;
                    }
                    else if (TargetY < 0)
                    {
                        TargetY = TargetY + 1;
                    }

                    if (TargetX == 0 && TargetY == 0)
                    {
                        break;
                    }

                    int Delay = (Weapon.FireRate / 5);

                    if (Delay >= 10)
                    {
                        await Task.Delay(Delay);
                    }
                }
            }
            else
            {
                Mouse.MovePosition(DiffX, DiffY);
            }
        }
    }
}
