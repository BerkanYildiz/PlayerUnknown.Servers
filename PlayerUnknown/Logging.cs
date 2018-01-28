namespace PlayerUnknown
{
    using System;
    using System.Diagnostics;

    using PlayerUnknown.Helpers;

    public static class Logging
    {
        /// <summary>
        /// Logs the specified informative message.
        /// </summary>
        /// <param name="Type">The type.</param>
        /// <param name="Message">The message.</param>
        [Conditional("DEBUG")]
        public static void Info(Type Type, string Message)
        {
            Debug.WriteLine("[ INFO  ] " + Type.Name.Pad() + " : " + Message);
        }

        /// <summary>
        /// Logs the specified warning message.
        /// </summary>
        /// <param name="Type">The type.</param>
        /// <param name="Message">The message.</param>
        public static void Warning(Type Type, string Message)
        {
            Debug.WriteLine("[WARNING] " + Type.Name.Pad() + " : " + Message);
        }

        /// <summary>
        /// Logs the specified error message.
        /// </summary>
        /// <param name="Type">The type.</param>
        /// <param name="Message">The message.</param>
        public static void Error(Type Type, string Message)
        {
            Debug.WriteLine("[ ERROR ] " + Type.Name.Pad() + " : " + Message);
        }

        /// <summary>
        /// Logs the specified fatal error message.
        /// </summary>
        /// <param name="Type">The type.</param>
        /// <param name="Message">The message.</param>
        public static void Fatal(Type Type, string Message)
        {
            Debug.WriteLine("[ FATAL ] " + Type.Name.Pad() + " : " + Message);
        }
    }
}