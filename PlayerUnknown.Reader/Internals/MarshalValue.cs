namespace PlayerUnknown.Reader.Internals
{
    using System;

    using PlayerUnknown.Reader.Memory;

    /// <summary>
    /// The factory to create instance of the <see cref="MarshalledValue{T}"/> class.
    /// </summary>
    /// <remarks>
    /// A factory pattern is used because C# 5.0 constructor doesn't support type inference.
    /// More info from Eric Lippert here : http://stackoverflow.com/questions/3570167/why-cant-the-c-sharp-constructor-infer-type
    /// </remarks>
    public static class MarshalValue
    {
        /// <summary>
        /// Marshals a given value into the remote process.
        /// </summary>
        /// <typeparam name="T">The type of the value. It can be a primitive or reference data type.</typeparam>
        /// <param name="BattleGroundMemory">The concerned process.</param>
        /// <param name="Value">The value to marshal.</param>
        /// <returns>The return value is an new instance of the <see cref="MarshalledValue{T}"/> class.</returns>
        public static MarshalledValue<T> Marshal<T>(BattleGroundMemory BattleGroundMemory, T Value)
        {
            return new MarshalledValue<T>(BattleGroundMemory, Value);
        }
    }

    /// <summary>
    /// Class marshalling a value into the remote process.
    /// </summary>
    /// <typeparam name="T">The type of the value. It can be a primitive or reference data type.</typeparam>
    public class MarshalledValue<T> : IMarshalledValue
    {
        /// <summary>
        /// The reference of the <see cref="BattleGroundMemory"/> object.
        /// </summary>
        protected readonly BattleGroundMemory BattleGroundMemory;

        /// <summary>
        /// Initializes a new instance of the <see cref="MarshalledValue{T}"/> class.
        /// </summary>
        /// <param name="BattleGroundMemory">The reference of the <see cref="BattleGroundMemory"/> object.</param>
        /// <param name="Value">The value to marshal.</param>
        public MarshalledValue(BattleGroundMemory BattleGroundMemory, T Value)
        {
            // Save the parameters
            this.BattleGroundMemory = BattleGroundMemory;
            this.Value = Value;

            // Marshal the value
            this.Marshal();
        }

        /// <summary>
        /// Frees resources and perform other cleanup operations before it is reclaimed by garbage collection.
        /// </summary>
        ~MarshalledValue()
        {
            this.Dispose();
        }

        /// <summary>
        /// The memory allocated where the value is fully written if needed. It can be unused.
        /// </summary>
        public RemoteAllocation Allocated
        {
            get;
            private set;
        }

        /// <summary>
        /// The reference of the value. It can be directly the value or a pointer.
        /// </summary>
        public IntPtr Reference
        {
            get;
            private set;
        }

        /// <summary>
        /// The initial value.
        /// </summary>
        public T Value
        {
            get;
        }

        /// <summary>
        /// Releases all resources used by the <see cref="RemoteAllocation"/> object.
        /// </summary>
        public void Dispose()
        {
            // Free the allocated memory
            if (this.Allocated != null)
            {
                this.Allocated.Dispose();
            }

            // Set the pointer to zero
            this.Reference = IntPtr.Zero;

            // Avoid the finalizer
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Marshals the value into the remote process.
        /// </summary>
        private void Marshal()
        {
            // If the type is string, it's a special case
            if (typeof(T) == typeof(string))
            {
                var text = this.Value.ToString();

                // Allocate memory in the remote process (string + '\0')
                this.Allocated = this.BattleGroundMemory.Memory.Allocate(text.Length + 1);

                // Write the value
                this.Allocated.WriteString(0, text);

                // Get the pointer
                this.Reference = this.Allocated.BaseAddress;
            }
            else
            {
                // For all other types
                // Convert the value into a byte array
                var ByteArray = MarshalType<T>.ObjectToByteArray(this.Value);

                // If the value can be stored directly in registers
                if (MarshalType<T>.CanBeStoredInRegisters)
                {
                    // Convert the byte array into a pointer
                    this.Reference = MarshalType<IntPtr>.ByteArrayToObject(ByteArray);
                }
                else
                {
                    // It's a bit more complicated, we must allocate some space into
                    // the remote process to store the value and get its pointer

                    // Allocate memory in the remote process
                    this.Allocated = this.BattleGroundMemory.Memory.Allocate(MarshalType<T>.Size);

                    // Write the value
                    this.Allocated.Write(0, this.Value);

                    // Get the pointer
                    this.Reference = this.Allocated.BaseAddress;
                }
            }
        }
    }
}