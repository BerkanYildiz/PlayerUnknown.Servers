namespace PlayerUnknown.Reader.Memory
{
    using System;

    using PlayerUnknown.Reader.Native;

    /// <summary>
    /// Class providing tools for manipulating memory protection.
    /// </summary>
    public class MemoryProtection : IDisposable
    {
        /// <summary>
        /// The reference of the <see cref="BattleGroundMemory"/> object.
        /// </summary>
        private readonly BattleGroundMemory BattleGroundMemory;

        /// <summary>
        /// Initializes a new instance of the <see cref="MemoryProtection"/> class.
        /// </summary>
        /// <param name="BattleGroundMemory">The reference of the <see cref="BattleGroundMemory"/> object.</param>
        /// <param name="BaseAddress">The base address of the memory to change the protection.</param>
        /// <param name="Size">The size of the memory to change.</param>
        /// <param name="Protection">The new protection to apply.</param>
        /// <param name="MustBeDisposed">The resource will be automatically disposed when the finalizer collects the object.</param>
        public MemoryProtection(BattleGroundMemory BattleGroundMemory, IntPtr BaseAddress, int Size, MemoryProtectionFlags Protection = MemoryProtectionFlags.ExecuteReadWrite, bool MustBeDisposed = true)
        {
            // Save the parameters
            this.BattleGroundMemory = BattleGroundMemory;
            this.BaseAddress = BaseAddress;
            this.NewProtection = Protection;
            this.Size = Size;
            this.MustBeDisposed = MustBeDisposed;

            // Change the memory protection
            this.OldProtection = MemoryCore.ChangeProtection(this.BattleGroundMemory.Handle, BaseAddress, Size, Protection);
        }

        /// <summary>
        /// Frees resources and perform other cleanup operations before it is reclaimed by garbage collection.
        /// </summary>
        ~MemoryProtection()
        {
            if (this.MustBeDisposed)
            {
                this.Dispose();
            }
        }

        /// <summary>
        /// The base address of the altered memory.
        /// </summary>
        public IntPtr BaseAddress
        {
            get;
        }

        /// <summary>
        /// States if the <see cref="MemoryProtection"/> object nust be disposed when it is collected.
        /// </summary>
        public bool MustBeDisposed
        {
            get;
            set;
        }

        /// <summary>
        /// Defines the new protection applied to the memory.
        /// </summary>
        public MemoryProtectionFlags NewProtection
        {
            get;
        }

        /// <summary>
        /// References the inital protection of the memory.
        /// </summary>
        public MemoryProtectionFlags OldProtection
        {
            get;
        }

        /// <summary>
        /// The size of the altered memory.
        /// </summary>
        public int Size
        {
            get;
        }

        /// <summary>
        /// Restores the initial protection of the memory.
        /// </summary>
        public virtual void Dispose()
        {
            // Restore the memory protection
            MemoryCore.ChangeProtection(this.BattleGroundMemory.Handle, this.BaseAddress, this.Size, this.OldProtection);

            // Avoid the finalizer 
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        public override string ToString()
        {
            return string.Format("BaseAddress = 0x{0:X} NewProtection = {1} OldProtection = {2}", this.BaseAddress.ToInt64(), this.NewProtection, this.OldProtection);
        }
    }
}