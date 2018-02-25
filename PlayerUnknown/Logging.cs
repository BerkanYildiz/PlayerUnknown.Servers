namespace PlayerUnknown
{
    using System;
    using System.Diagnostics;

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
            Debug.WriteLine("[ INFO  ] " + Type.Name + " : " + Message);
        }

        /// <summary>
        /// Logs the specified warning message.
        /// </summary>
        /// <param name="Type">The type.</param>
        /// <param name="Message">The message.</param>
        public static void Warning(Type Type, string Message)
        {
            Debug.WriteLine("[WARNING] " + Type.Name + " : " + Message);
        }

        /// <summary>
        /// Logs the specified error message.
        /// </summary>
        /// <param name="Type">The type.</param>
        /// <param name="Message">The message.</param>
        public static void Error(Type Type, string Message)
        {
            Debug.WriteLine("[ ERROR ] " + Type.Name + " : " + Message);
        }

        /// <summary>
        /// Logs the specified fatal error message.
        /// </summary>
        /// <param name="Type">The type.</param>
        /// <param name="Message">The message.</param>
        public static void Fatal(Type Type, string Message)
        {
            Debug.WriteLine("[ FATAL ] " + Type.Name + " : " + Message);
        }
    }
}