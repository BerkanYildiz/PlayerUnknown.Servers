namespace PlayerUnknown.Reader.Assembly.CallingConvention
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    /// <summary>
    /// Define the Fast Calling Convention (aka __msfastcall).
    /// </summary>
    public class FastcallCallingConvention : ICallingConvention
    {
        /// <summary>
        /// The name of the calling convention.
        /// </summary>
        public string Name
        {
            get
            {
                return "Fastcall";
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
            var ParamList = new List<IntPtr>(Parameters);

            // Store the first parameter in the ECX register
            if (ParamList.Count > 0)
            {
                ret.AppendLine("mov ecx, " + ParamList[0]);
                ParamList.RemoveAt(0);
            }

            // Store the second parameter in the EDX register
            if (ParamList.Count > 0)
            {
                ret.AppendLine("mov edx, " + ParamList[0]);
                ParamList.RemoveAt(0);
            }

            // For each parameters (in reverse order)
            ParamList.Reverse();
            foreach (var parameter in ParamList)
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