namespace PlayerUnknown.Reader.Assembly.CallingConvention
{
    using System;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Define the Standard Calling Convention.
    /// </summary>
    public class StdcallCallingConvention : ICallingConvention
    {
        /// <summary>
        /// The name of the calling convention.
        /// </summary>
        public string Name
        {
            get
            {
                return "Stdcall";
            }
        }

        /// <summary>
        /// Defines which function performs the clean-up task.
        /// </summary>
        public CleanupTypes Cleanup
        {
            get
            {
                return CleanupTypes.Callee;
            }
        }

        /// <summary>
        /// Formats the given parameters to call a function.
        /// </summary>
        /// <param name="Parameters">An array of parameters.</param>
        /// <returns>The mnemonics to pass the parameters.</returns>
        public string FormatParameters(IntPtr[] Parameters)
        {
            // Declare a var to store the mnemonics
            var ret = new StringBuilder();

            // For each parameters (in reverse order)
            foreach (var parameter in Parameters.Reverse())
            {
                ret.AppendLine("push " + parameter);
            }

            // Return the mnemonics
            return ret.ToString();
        }

        /// <summary>
        /// Formats the call of a given function.
        /// </summary>
        /// <param name="Function">The function to call.</param>
        /// <returns>The mnemonics to call the function.</returns>
        public string FormatCalling(IntPtr Function)
        {
            return "call " + Function;
        }

        /// <summary>
        /// Formats the cleaning of a given number of parameters.
        /// </summary>
        /// <param name="NbParameters">The number of parameters to clean.</param>
        /// <returns>The mnemonics to clean a given number of parameters.</returns>
        public string FormatCleaning(int NbParameters)
        {
            return string.Empty;
        }
    }
}