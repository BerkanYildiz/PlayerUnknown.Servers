namespace PlayerUnknown.Reader.Memory
{
    using System;
    using System.Text;
    using System.Threading.Tasks;

    using PlayerUnknown.Reader.Assembly.CallingConvention;
    using PlayerUnknown.Reader.Native;

    /// <summary>
    /// Class representing a pointer in the memory of the remote process.
    /// </summary>
    public class RemotePointer : IEquatable<RemotePointer>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RemotePointer"/> class.
        /// </summary>
        /// <param name="BattleGroundMemory">The reference of the <see cref="BattleGroundMemory"/> object.</param>
        /// <param name="Address">The location where the pointer points in the remote process.</param>
        public RemotePointer(BattleGroundMemory BattleGroundMemory, IntPtr Address)
        {
            // Save the parameters
            this.BattleGroundMemory = BattleGroundMemory;
            this.BaseAddress = Address;
        }

        /// <summary>
        /// The address of the pointer in the remote process.
        /// </summary>
        public IntPtr BaseAddress
        {
            get;
            protected set;
        }

        /// <summary>
        /// Gets if the <see cref="RemotePointer"/> is valid.
        /// </summary>
        public virtual bool IsValid
        {
            get
            {
                return this.BattleGroundMemory.IsRunning && this.BaseAddress != IntPtr.Zero;
            }
        }

        /// <summary>
        /// The reference of the <see cref="BattleGroundMemory"/> object.
        /// </summary>
        public BattleGroundMemory BattleGroundMemory
        {
            get;
            protected set;
        }

        /// <summary>
        /// Changes the protection of the n next bytes in remote process.
        /// </summary>
        /// <param name="Size">The size of the memory to change.</param>
        /// <param name="Protection">The new protection to apply.</param>
        /// <param name="MustBeDisposed">The resource will be automatically disposed when the finalizer collects the object.</param>
        /// <returns>A new instance of the <see cref="MemoryProtection"/> class.</returns>
        public MemoryProtection ChangeProtection(int Size, MemoryProtectionFlags Protection = MemoryProtectionFlags.ExecuteReadWrite, bool MustBeDisposed = true)
        {
            return new MemoryProtection(this.BattleGroundMemory, this.BaseAddress, Size, Protection, MustBeDisposed);
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

            return Obj.GetType() == this.GetType() && this.Equals((RemotePointer)Obj);
        }

        /// <summary>
        /// Returns a value indicating whether this instance is equal to a specified object.
        /// </summary>
        public bool Equals(RemotePointer Other)
        {
            if (object.ReferenceEquals(null, Other))
            {
                return false;
            }

            return object.ReferenceEquals(this, Other) || this.BaseAddress.Equals(Other.BaseAddress) && this.BattleGroundMemory.Equals(Other.BattleGroundMemory);
        }

        /// <summary>
        /// Executes the assembly code in the remote process.
        /// </summary>
        /// <returns>The return value is the exit code of the thread created to execute the assembly code.</returns>
        public T Execute<T>()
        {
            return this.BattleGroundMemory.Assembly.Execute<T>(this.BaseAddress);
        }

        /// <summary>
        /// Executes the assembly code in the remote process.
        /// </summary>
        /// <returns>The return value is the exit code of the thread created to execute the assembly code.</returns>
        public IntPtr Execute()
        {
            return this.Execute<IntPtr>();
        }

        /// <summary>
        /// Executes the assembly code in the remote process.
        /// </summary>
        /// <param name="Parameter">The parameter used to execute the assembly code.</param>
        /// <returns>The return value is the exit code of the thread created to execute the assembly code.</returns>
        public T Execute<T>(dynamic Parameter)
        {
            return this.BattleGroundMemory.Assembly.Execute<T>(this.BaseAddress, Parameter);
        }

        /// <summary>
        /// Executes the assembly code in the remote process.
        /// </summary>
        /// <param name="Parameter">The parameter used to execute the assembly code.</param>
        /// <returns>The return value is the exit code of the thread created to execute the assembly code.</returns>
        public IntPtr Execute(dynamic Parameter)
        {
            return Execute<IntPtr>(Parameter);
        }

        /// <summary>
        /// Executes the assembly code in the remote process.
        /// </summary>
        /// <param name="CallingConvention">The calling convention used to execute the assembly code with the parameters.</param>
        /// <param name="Parameters">An array of parameters used to execute the assembly code.</param>
        /// <returns>The return value is the exit code of the thread created to execute the assembly code.</returns>
        public T Execute<T>(CallingConventions CallingConvention, params dynamic[] Parameters)
        {
            return this.BattleGroundMemory.Assembly.Execute<T>(this.BaseAddress, CallingConvention, Parameters);
        }

        /// <summary>
        /// Executes the assembly code in the remote process.
        /// </summary>
        /// <param name="CallingConvention">The calling convention used to execute the assembly code with the parameters.</param>
        /// <param name="Parameters">An array of parameters used to execute the assembly code.</param>
        /// <returns>The return value is the exit code of the thread created to execute the assembly code.</returns>
        public IntPtr Execute(CallingConventions CallingConvention, params dynamic[] Parameters)
        {
            return this.Execute<IntPtr>(CallingConvention, Parameters);
        }

        /// <summary>
        /// Executes asynchronously the assembly code in the remote process.
        /// </summary>
        /// <returns>The return value is an asynchronous operation that return the exit code of the thread created to execute the assembly code.</returns>
        public Task<T> ExecuteAsync<T>()
        {
            return this.BattleGroundMemory.Assembly.ExecuteAsync<T>(this.BaseAddress);
        }

        /// <summary>
        /// Executes asynchronously the assembly code in the remote process.
        /// </summary>
        /// <returns>The return value is an asynchronous operation that return the exit code of the thread created to execute the assembly code.</returns>
        public Task<IntPtr> ExecuteAsync()
        {
            return this.ExecuteAsync<IntPtr>();
        }

        /// <summary>
        /// Executes asynchronously the assembly code located in the remote process at the specified address.
        /// </summary>
        /// <param name="Parameter">The parameter used to execute the assembly code.</param>
        /// <returns>The return value is an asynchronous operation that return the exit code of the thread created to execute the assembly code.</returns>
        public Task<T> ExecuteAsync<T>(dynamic Parameter)
        {
            return this.BattleGroundMemory.Assembly.ExecuteAsync<T>(this.BaseAddress, Parameter);
        }

        /// <summary>
        /// Executes asynchronously the assembly code located in the remote process at the specified address.
        /// </summary>
        /// <param name="Parameter">The parameter used to execute the assembly code.</param>
        /// <returns>The return value is an asynchronous operation that return the exit code of the thread created to execute the assembly code.</returns>
        public Task<IntPtr> ExecuteAsync(dynamic Parameter)
        {
            return ExecuteAsync<IntPtr>(Parameter);
        }

        /// <summary>
        /// Executes asynchronously the assembly code located in the remote process at the specified address.
        /// </summary>
        /// <param name="CallingConvention">The calling convention used to execute the assembly code with the parameters.</param>
        /// <param name="Parameters">An array of parameters used to execute the assembly code.</param>
        /// <returns>The return value is an asynchronous operation that return the exit code of the thread created to execute the assembly code.</returns>
        public Task<T> ExecuteAsync<T>(CallingConventions CallingConvention, params dynamic[] Parameters)
        {
            return this.BattleGroundMemory.Assembly.ExecuteAsync<T>(this.BaseAddress, CallingConvention, Parameters);
        }

        /// <summary>
        /// Executes asynchronously the assembly code located in the remote process at the specified address.
        /// </summary>
        /// <param name="CallingConvention">The calling convention used to execute the assembly code with the parameters.</param>
        /// <param name="Parameters">An array of parameters used to execute the assembly code.</param>
        /// <returns>The return value is an asynchronous operation that return the exit code of the thread created to execute the assembly code.</returns>
        public Task<IntPtr> ExecuteAsync(CallingConventions CallingConvention, params dynamic[] Parameters)
        {
            return this.ExecuteAsync<IntPtr>(CallingConvention, Parameters);
        }

        /// <summary>
        /// Serves as a hash function for a particular type. 
        /// </summary>
        public override int GetHashCode()
        {
            return this.BaseAddress.GetHashCode() ^ this.BattleGroundMemory.GetHashCode();
        }

        public static bool operator ==(RemotePointer Left, RemotePointer Right)
        {
            return object.Equals(Left, Right);
        }

        public static bool operator !=(RemotePointer Left, RemotePointer Right)
        {
            return !object.Equals(Left, Right);
        }

        /// <summary>
        /// Reads the value of a specified type in the remote process.
        /// </summary>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <param name="Offset">The offset where the value is read from the pointer.</param>
        /// <returns>A value.</returns>
        public T Read<T>(int Offset)
        {
            return this.BattleGroundMemory.Read<T>(this.BaseAddress + Offset, false);
        }

        /// <summary>
        /// Reads the value of a specified type in the remote process.
        /// </summary>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <param name="Offset">The offset where the value is read from the pointer.</param>
        /// <returns>A value.</returns>
        public T Read<T>(Enum Offset)
        {
            return this.Read<T>(Convert.ToInt32(Offset));
        }

        /// <summary>
        /// Reads the value of a specified type in the remote process.
        /// </summary>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <returns>A value.</returns>
        public T Read<T>()
        {
            return this.Read<T>(0);
        }

        /// <summary>
        /// Reads an array of a specified type in the remote process.
        /// </summary>
        /// <typeparam name="T">The type of the values.</typeparam>
        /// <param name="Offset">The offset where the values is read from the pointer.</param>
        /// <param name="Count">The number of cells in the array.</param>
        /// <returns>An array.</returns>
        public T[] Read<T>(int Offset, int Count)
        {
            return this.BattleGroundMemory.Read<T>(this.BaseAddress + Offset, Count, false);
        }

        /// <summary>
        /// Reads an array of a specified type in the remote process.
        /// </summary>
        /// <typeparam name="T">The type of the values.</typeparam>
        /// <param name="Offset">The offset where the values is read from the pointer.</param>
        /// <param name="Count">The number of cells in the array.</param>
        /// <returns>An array.</returns>
        public T[] Read<T>(Enum Offset, int Count)
        {
            return this.Read<T>(Convert.ToInt32(Offset), Count);
        }

        /// <summary>
        /// Reads a string with a specified encoding in the remote process.
        /// </summary>
        /// <param name="Offset">The offset where the string is read from the pointer.</param>
        /// <param name="Encoding">The encoding used.</param>
        /// <param name="MaxLength">[Optional] The number of maximum bytes to read. The string is automatically cropped at this end ('\0' char).</param>
        /// <returns>The string.</returns>
        public string ReadString(int Offset, Encoding Encoding, int MaxLength = 512)
        {
            return this.BattleGroundMemory.ReadString(this.BaseAddress + Offset, Encoding, false, MaxLength);
        }

        /// <summary>
        /// Reads a string with a specified encoding in the remote process.
        /// </summary>
        /// <param name="Offset">The offset where the string is read from the pointer.</param>
        /// <param name="Encoding">The encoding used.</param>
        /// <param name="MaxLength">[Optional] The number of maximum bytes to read. The string is automatically cropped at this end ('\0' char).</param>
        /// <returns>The string.</returns>
        public string ReadString(Enum Offset, Encoding Encoding, int MaxLength = 512)
        {
            return this.ReadString(Convert.ToInt32(Offset), Encoding, MaxLength);
        }

        /// <summary>
        /// Reads a string with a specified encoding in the remote process.
        /// </summary>
        /// <param name="Encoding">The encoding used.</param>
        /// <param name="MaxLength">[Optional] The number of maximum bytes to read. The string is automatically cropped at this end ('\0' char).</param>
        /// <returns>The string.</returns>
        public string ReadString(Encoding Encoding, int MaxLength = 512)
        {
            return this.ReadString(0, Encoding, MaxLength);
        }

        /// <summary>
        /// Reads a string using the encoding UTF8 in the remote process.
        /// </summary>
        /// <param name="Offset">The offset where the string is read from the pointer.</param>
        /// <param name="MaxLength">[Optional] The number of maximum bytes to read. The string is automatically cropped at this end ('\0' char).</param>
        /// <returns>The string.</returns>
        public string ReadString(int Offset, int MaxLength = 512)
        {
            return this.BattleGroundMemory.ReadString(this.BaseAddress + Offset, false, MaxLength);
        }

        /// <summary>
        /// Reads a string using the encoding UTF8 in the remote process.
        /// </summary>
        /// <param name="Offset">The offset where the string is read from the pointer.</param>
        /// <param name="MaxLength">[Optional] The number of maximum bytes to read. The string is automatically cropped at this end ('\0' char).</param>
        /// <returns>The string.</returns>
        public string ReadString(Enum Offset, int MaxLength = 512)
        {
            return this.ReadString(Convert.ToInt32(Offset), MaxLength);
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        public override string ToString()
        {
            return string.Format("BaseAddress = 0x{0:X}", this.BaseAddress.ToInt64());
        }

        /// <summary>
        /// Writes the values of a specified type in the remote process.
        /// </summary>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <param name="Offset">The offset where the value is written from the pointer.</param>
        /// <param name="Value">The value to write.</param>
        public void Write<T>(int Offset, T Value)
        {
            this.BattleGroundMemory.Write(this.BaseAddress + Offset, Value, false);
        }

        /// <summary>
        /// Writes the values of a specified type in the remote process.
        /// </summary>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <param name="Offset">The offset where the value is written from the pointer.</param>
        /// <param name="Value">The value to write.</param>
        public void Write<T>(Enum Offset, T Value)
        {
            this.Write(Convert.ToInt32(Offset), Value);
        }

        /// <summary>
        /// Writes the values of a specified type in the remote process.
        /// </summary>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <param name="Value">The value to write.</param>
        public void Write<T>(T Value)
        {
            this.Write(0, Value);
        }

        /// <summary>
        /// Writes an array of a specified type in the remote process.
        /// </summary>
        /// <typeparam name="T">The type of the values.</typeparam>
        /// <param name="Offset">The offset where the values is written from the pointer.</param>
        /// <param name="Array">The array to write.</param>
        public void Write<T>(int Offset, T[] Array)
        {
            this.BattleGroundMemory.Write(this.BaseAddress + Offset, Array, false);
        }

        /// <summary>
        /// Writes an array of a specified type in the remote process.
        /// </summary>
        /// <typeparam name="T">The type of the values.</typeparam>
        /// <param name="Offset">The offset where the values is written from the pointer.</param>
        /// <param name="Array">The array to write.</param>
        public void Write<T>(Enum Offset, T[] Array)
        {
            this.Write(Convert.ToInt32(Offset), Array);
        }

        /// <summary>
        /// Writes an array of a specified type in the remote process.
        /// </summary>
        /// <typeparam name="T">The type of the values.</typeparam>
        /// <param name="Array">The array to write.</param>
        public void Write<T>(T[] Array)
        {
            this.Write(0, Array);
        }

        /// <summary>
        /// Writes a string with a specified encoding in the remote process.
        /// </summary>
        /// <param name="Offset">The offset where the string is written from the pointer.</param>
        /// <param name="Text">The text to write.</param>
        /// <param name="Encoding">The encoding used.</param>
        public void WriteString(int Offset, string Text, Encoding Encoding)
        {
            this.BattleGroundMemory.WriteString(this.BaseAddress + Offset, Text, Encoding, false);
        }

        /// <summary>
        /// Writes a string with a specified encoding in the remote process.
        /// </summary>
        /// <param name="Offset">The offset where the string is written from the pointer.</param>
        /// <param name="Text">The text to write.</param>
        /// <param name="Encoding">The encoding used.</param>
        public void WriteString(Enum Offset, string Text, Encoding Encoding)
        {
            this.WriteString(Convert.ToInt32(Offset), Text, Encoding);
        }

        /// <summary>
        /// Writes a string with a specified encoding in the remote process.
        /// </summary>
        /// <param name="Text">The text to write.</param>
        /// <param name="Encoding">The encoding used.</param>
        public void WriteString(string Text, Encoding Encoding)
        {
            this.WriteString(0, Text, Encoding);
        }

        /// <summary>
        /// Writes a string using the encoding UTF8 in the remote process.
        /// </summary>
        /// <param name="Offset">The offset where the string is written from the pointer.</param>
        /// <param name="Text">The text to write.</param>
        public void WriteString(int Offset, string Text)
        {
            this.BattleGroundMemory.WriteString(this.BaseAddress + Offset, Text, false);
        }

        /// <summary>
        /// Writes a string using the encoding UTF8 in the remote process.
        /// </summary>
        /// <param name="Offset">The offset where the string is written from the pointer.</param>
        /// <param name="Text">The text to write.</param>
        public void WriteString(Enum Offset, string Text)
        {
            this.WriteString(Convert.ToInt32(Offset), Text);
        }

        /// <summary>
        /// Writes a string using the encoding UTF8 in the remote process.
        /// </summary>
        /// <param name="Text">The text to write.</param>
        public void WriteString(string Text)
        {
            this.WriteString(0, Text);
        }
    }
}