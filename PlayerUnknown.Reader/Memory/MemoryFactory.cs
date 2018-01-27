namespace PlayerUnknown.Reader.Memory
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using PlayerUnknown.Reader.Internals;
    using PlayerUnknown.Reader.Native;

    /// <summary>
    /// Class providing tools for manipulating memory space.
    /// </summary>
    public class MemoryFactory : IFactory
    {
        /// <summary>
        /// The reference of the <see cref="BattleGroundMemory"/> object.
        /// </summary>
        protected readonly BattleGroundMemory BattleGroundMemory;

        /// <summary>
        /// The list containing all allocated memory.
        /// </summary>
        protected readonly List<RemoteAllocation> InternalRemoteAllocations;

        /// <summary>
        /// Initializes a new instance of the <see cref="MemoryFactory"/> class.
        /// </summary>
        /// <param name="BattleGroundMemory">The reference of the <see cref="BattleGroundMemory"/> object.</param>
        internal MemoryFactory(BattleGroundMemory BattleGroundMemory)
        {
            // Save the parameter
            this.BattleGroundMemory = BattleGroundMemory;

            // Create a list containing all allocated memory
            this.InternalRemoteAllocations = new List<RemoteAllocation>();
        }

        /// <summary>
        /// Frees resources and perform other cleanup operations before it is reclaimed by garbage collection.
        /// </summary>
        ~MemoryFactory()
        {
            this.Dispose();
        }

        /// <summary>
        /// A collection containing all allocated memory in the remote process.
        /// </summary>
        public IEnumerable<RemoteAllocation> RemoteAllocations
        {
            get
            {
                return this.InternalRemoteAllocations.AsReadOnly();
            }
        }

        /// <summary>
        /// Gets all blocks of memory allocated in the remote process.
        /// </summary>
        public IEnumerable<RemoteRegion> Regions
        {
            get
            {
#if x64
                var adresseTo = new IntPtr(0x7fffffffffffffff);
#else
                var AdresseTo = new IntPtr(0x7fffffff);
#endif
                return MemoryCore.Query(this.BattleGroundMemory.Handle, IntPtr.Zero, AdresseTo).Select(Page => new RemoteRegion(this.BattleGroundMemory, Page.BaseAddress));
            }
        }

        /// <summary>
        /// Allocates a region of memory within the virtual address space of the remote process.
        /// </summary>
        /// <param name="Size">The size of the memory to allocate.</param>
        /// <param name="Protection">The protection of the memory to allocate.</param>
        /// <param name="MustBeDisposed">The allocated memory will be released when the finalizer collects the object.</param>
        /// <returns>A new instance of the <see cref="RemoteAllocation"/> class.</returns>
        public RemoteAllocation Allocate(int Size, MemoryProtectionFlags Protection = MemoryProtectionFlags.ExecuteReadWrite, bool MustBeDisposed = true)
        {
            // Allocate a memory space
            var memory = new RemoteAllocation(this.BattleGroundMemory, Size, Protection, MustBeDisposed);

            // Add the memory in the list
            this.InternalRemoteAllocations.Add(memory);
            return memory;
        }

        /// <summary>
        /// Deallocates a region of memory previously allocated within the virtual address space of the remote process.
        /// </summary>
        /// <param name="Allocation">The allocated memory to release.</param>
        public void Deallocate(RemoteAllocation Allocation)
        {
            // Dispose the element
            if (!Allocation.IsDisposed)
            {
                Allocation.Dispose();
            }

            // Remove the element from the allocated memory list
            if (this.InternalRemoteAllocations.Contains(Allocation))
            {
                this.InternalRemoteAllocations.Remove(Allocation);
            }
        }

        /// <summary>
        /// Releases all resources used by the <see cref="MemoryFactory"/> object.
        /// </summary>
        public virtual void Dispose()
        {
            // Release all allocated memories which must be disposed
            foreach (var AllocatedMemory in this.InternalRemoteAllocations.Where(M => M.MustBeDisposed).ToArray())
            {
                AllocatedMemory.Dispose();
            }

            // Avoid the finalizer
            GC.SuppressFinalize(this);
        }
    }
}