namespace PlayerUnknown.Reader.Memory
{
    using System;

    using PlayerUnknown.Reader.Internals;
    using PlayerUnknown.Reader.Native;

    /// <summary>
    /// Class representing an allocated memory in a remote process.
    /// </summary>
    public class RemoteAllocation : RemoteRegion, IDisposableState
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RemoteAllocation"/> class.
        /// </summary>
        /// <param name="BattleGroundMemory">The reference of the <see cref="BattleGroundMemory"/> object.</param>
        /// <param name="Size">The size of the allocated memory.</param>
        /// <param name="Protection">The protection of the allocated memory.</param>
        /// <param name="MustBeDisposed">The allocated memory will be released when the finalizer collects the object.</param>
        internal RemoteAllocation(BattleGroundMemory BattleGroundMemory, int Size, MemoryProtectionFlags Protection = MemoryProtectionFlags.ExecuteReadWrite, bool MustBeDisposed = true)
            : base(BattleGroundMemory, MemoryCore.Allocate(BattleGroundMemory.Handle, Size, Protection))
        {
            // Set local vars
            this.MustBeDisposed = MustBeDisposed;
            this.IsDisposed = false;
        }

        /// <summary>
        /// Frees resources and perform other cleanup operations before it is reclaimed by garbage collection.
        /// </summary>
        ~RemoteAllocation()
        {
            if (this.MustBeDisposed)
            {
                this.Dispose();
            }
        }

        /// <summary>
        /// Gets a value indicating whether the element is disposed.
        /// </summary>
        public bool IsDisposed
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets a value indicating whether the element must be disposed when the Garbage Collector collects the object.
        /// </summary>
        public bool MustBeDisposed
        {
            get;
            set;
        }

        /// <summary>
        /// Releases all resources used by the <see cref="RemoteAllocation"/> object.
        /// </summary>
        /// <remarks>Don't use the IDisposable pattern because the class is sealed.</remarks>
        public virtual void Dispose()
        {
            if (!this.IsDisposed)
            {
                // Set the flag to true
                this.IsDisposed = true;

                // Release the allocated memory
                this.Release();

                // Remove this object from the collection of allocated memory
                this.BattleGroundMemory.Memory.Deallocate(this);

                // Remove the pointer
                this.BaseAddress = IntPtr.Zero;

                // Avoid the finalizer 
                GC.SuppressFinalize(this);
            }
        }
    }
}