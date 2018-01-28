namespace PlayerUnknown.Test
{
    using System;

    internal class Program
    {
        /// <summary>
        /// Defines the entry point of the application.
        /// </summary>
        internal static void Main()
        {
            PUBG.Attach();

            if (PUBG.IsAttached)
            {
                // Test
            }

            Console.ReadKey(false);
        }
    }
}
