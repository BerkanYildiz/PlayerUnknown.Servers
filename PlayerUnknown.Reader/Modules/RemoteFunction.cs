namespace PlayerUnknown.Reader.Modules
{
    using System;

    using PlayerUnknown.Reader.Memory;

    /// <summary>
    /// Class representing a function in the remote process.
    /// </summary>
    public class RemoteFunction : RemotePointer
    {
        public RemoteFunction(BattleGroundMemory BattleGroundMemory, IntPtr Address, string FunctionName)
            : base(BattleGroundMemory, Address)
        {
            // Save the parameter
            this.Name = FunctionName;
        }

        /// <summary>
        /// The name of the function.
        /// </summary>
        public string Name
        {
            get;
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        public override string ToString()
        {
            return string.Format("BaseAddress = 0x{0:X} Name = {1}", this.BaseAddress.ToInt64(), this.Name);
        }
    }
}