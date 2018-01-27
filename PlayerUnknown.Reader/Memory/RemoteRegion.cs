namespace PlayerUnknown.Reader.Memory
{
    using System;

    using PlayerUnknown.Reader.Native;

    /// <summary>
    /// Represents a contiguous block of memory in the remote process.
    /// </summary>
    public class RemoteRegion : RemotePointer, IEquatable<RemoteRegion>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RemoteRegion"/> class.
        /// </summary>
        /// <param name="BattleGroundMemory">The reference of the <see cref="BattleGroundMemory"/> object.</param>
        /// <param name="BaseAddress">The base address of the memory region.</param>
        internal RemoteRegion(BattleGroundMemory BattleGroundMemory, IntPtr BaseAddress)
            : base(BattleGroundMemory, BaseAddress)
        {
        }

        /// <summary>
        /// Contains information about the memory.
        /// </summary>
        public MemoryBasicInformation Information
        {
            get
            {
                return MemoryCore.Query(this.BattleGroundMemory.Handle, this.BaseAddress);
            }
        }

        /// <summary>
        /// Gets if the <see cref="RemoteRegion"/> is valid.
        /// </summary>
        public override bool IsValid
        {
            get
            {
                return base.IsValid && this.Information.State != MemoryStateFlags.Free;
            }
        }

        /// <summary>
        /// Changes the protection of the n next bytes in remote process.
        /// </summary>
        /// <param name="Protection">The new protection to apply.</param>
        /// <param name="MustBeDisposed">The resource will be automatically disposed when the finalizer collects the object.</param>
        /// <returns>A new instance of the <see cref="MemoryProtection"/> class.</returns>
        public MemoryProtection ChangeProtection(MemoryProtectionFlags Protection = MemoryProtectionFlags.ExecuteReadWrite, bool MustBeDisposed = true)
        {
            return new MemoryProtection(this.BattleGroundMemory, this.BaseAddress, this.Information.RegionSize, Protection, MustBeDisposed);
        }

        /// <summary>
        /// Determines whether the specified object is equal to the current object.
        /// </summary>
        public override bool Equals(object Obj)
        {
            if (object.ReferenceEquals(null, Obj))
            {
                return false;
            }

            if (object.ReferenceEquals(this, Obj))
            {
                return true;
            }

            return Obj.GetType() == this.GetType() && this.Equals((RemoteRegion)Obj);
        }

        /// <summary>
        /// Returns a value indicating whether this instance is equal to a specified object.
        /// </summary>
        public bool Equals(RemoteRegion Other)
        {
            if (object.ReferenceEquals(null, Other))
            {
                return false;
            }

            return object.ReferenceEquals(this, Other) || this.BaseAddress.Equals(Other.BaseAddress) && this.BattleGroundMemory.Equals(Other.BattleGroundMemory) && this.Information.RegionSize.Equals(Other.Information.RegionSize);
        }

        /// <summary>
        /// Serves as a hash function for a particular type. 
        /// </summary>
        public override int GetHashCode()
        {
            return this.BaseAddress.GetHashCode() ^ this.BattleGroundMemory.GetHashCode() ^ this.Information.RegionSize.GetHashCode();
        }

        public static bool operator ==(RemoteRegion Left, RemoteRegion Right)
        {
            return object.Equals(Left, Right);
        }

        public static bool operator !=(RemoteRegion Left, RemoteRegion Right)
        {
            return !object.Equals(Left, Right);
        }

        /// <summary>
        /// Releases the memory used by the region.
        /// </summary>
        public void Release()
        {
            // Release the memory
            MemoryCore.Free(this.BattleGroundMemory.Handle, this.BaseAddress);

            // Remove the pointer
            this.BaseAddress = IntPtr.Zero;
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        public override string ToString()
        {
            return string.Format("BaseAddress = 0x{0:X} Size = 0x{1:X} Protection = {2}", this.BaseAddress.ToInt64(), this.Information.RegionSize, this.Information.Protect);
        }
    }
}