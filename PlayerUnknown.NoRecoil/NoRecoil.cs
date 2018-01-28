namespace PlayerUnknown.NoRecoil
{
    using System;
    using System.Drawing;
    using System.Threading.Tasks;

    using PlayerUnknown.NoRecoil.Events;
    using PlayerUnknown.NoRecoil.Logic;
    using PlayerUnknown.NoRecoil.Logic.Weapons;

    public static class NoRecoil
    {
        /// <summary>
        /// Gets or sets a value indicating whether the <see cref="NoRecoil"/> is enabled.
        /// </summary>
        public static bool IsEnabled
        {
            get;
            set;
        }

        /// <summary>
        /// Defines the entry point of the application.
        /// </summary>
        public static void Main()
        {
            PUBG.Attach();

            if (PUBG.IsAttached && PUBG.IsRunning)
            {
                Player.SetWeapon(new Ak47());
            }

            EventHandlers.Run();
            NoRecoil.Run();

            Console.ReadKey(false);
        }

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
                while (NoRecoil.IsEnabled && PUBG.IsOnScreen)
                {
                    var Weapon = Player.GetWeapon();

                    if (Weapon != null)
                    {
                        DoRecoil(Weapon.RecoilRate);

                        if (Weapon.IsRecoilEnabled)
                        {
                            await Task.Delay(Weapon.FireRate);
                        }
                    }
                }

                await Task.Delay(1000);
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
