namespace PlayerUnknown.Reader
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Text;

    using PlayerUnknown.Reader.Assembly;
    using PlayerUnknown.Reader.Helpers;
    using PlayerUnknown.Reader.Internals;
    using PlayerUnknown.Reader.Memory;
    using PlayerUnknown.Reader.Modules;
    using PlayerUnknown.Reader.Native;
    using PlayerUnknown.Reader.Threading;
    using PlayerUnknown.Reader.Windows;

    /// <summary>
    /// Class for memory editing a remote process.
    /// </summary>
    public class BattleGroundMemory : IDisposable, IEquatable<BattleGroundMemory>
    {
        /// <summary>
        /// The factories embedded inside the library.
        /// </summary>
        protected List<IFactory> Factories;

        /// <summary>
        /// Raises when the <see cref="BattleGroundMemory"/> object is disposed.
        /// </summary>
        public event EventHandler OnDispose;

        /// <summary>
        /// Initializes a new instance of the <see cref="BattleGroundMemory"/> class.
        /// </summary>
        /// <param name="Process">Process to open.</param>
        public BattleGroundMemory(Process Process)
        {
            this.Native = Process;
            this.Handle = MemoryCore.OpenProcess(ProcessAccessFlags.AllAccess, Process.Id);
            this.Peb = new ManagedPeb(this, ManagedPeb.FindPeb(this.Handle));
            this.Factories = new List<IFactory>();
            this.Factories.AddRange(
                new IFactory[]
                    {
                        this.Assembly = new AssemblyFactory(this), this.Memory = new MemoryFactory(this), this.Modules = new ModuleFactory(this), this.Threads = new ThreadFactory(this), this.Windows = new WindowFactory(this)
                    });
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BattleGroundMemory"/> class.
        /// </summary>
        /// <param name="ProcessId">Process id of the process to open.</param>
        public BattleGroundMemory(int ProcessId)
            : this(ApplicationFinder.FromProcessId(ProcessId))
        {
        }

        /// <summary>
        /// Frees resources and perform other cleanup operations before it is reclaimed by garbage collection. 
        /// </summary>
        ~BattleGroundMemory()
        {
            this.Dispose();
        }

        /// <summary>
        /// Factory for generating assembly code.
        /// </summary>
        public AssemblyFactory Assembly
        {
            get;
            protected set;
        }

        /// <summary>
        /// Gets whether the process is being debugged.
        /// </summary>
        public bool IsDebugged
        {
            get
            {
                return this.Peb.BeingDebugged;
            }

            set
            {
                this.Peb.BeingDebugged = value;
            }
        }

        /// <summary>
        /// State if the process is running.
        /// </summary>
        public bool IsRunning
        {
            get
            {
                return !this.Handle.IsInvalid && !this.Handle.IsClosed && !this.Native.HasExited;
            }
        }

        /// <summary>
        /// The remote process handle opened with all rights.
        /// </summary>
        public SafeMemoryHandle Handle
        {
            get;
        }

        /// <summary>
        /// Factory for manipulating memory space.
        /// </summary>
        public MemoryFactory Memory
        {
            get;
            protected set;
        }

        /// <summary>
        /// Factory for manipulating modules and libraries.
        /// </summary>
        public ModuleFactory Modules
        {
            get;
            protected set;
        }

        /// <summary>
        /// Provide access to the opened process.
        /// </summary>
        public Process Native
        {
            get;
        }

        /// <summary>
        /// The Process Environment Block of the process.
        /// </summary>
        public ManagedPeb Peb
        {
            get;
        }

        /// <summary>
        /// Gets the unique identifier for the remote process.
        /// </summary>
        public int Pid
        {
            get
            {
                return this.Native.Id;
            }
        }

        /// <summary>
        /// Factory for manipulating threads.
        /// </summary>
        public ThreadFactory Threads
        {
            get;
            protected set;
        }

        /// <summary>
        /// Factory for manipulating windows.
        /// </summary>
        public WindowFactory Windows
        {
            get;
            protected set;
        }

        /// <summary>
        /// Gets the specified module in the remote process.
        /// </summary>
        /// <param name="ModuleName">The name of module (not case sensitive).</param>
        /// <returns>A new instance of a <see cref="RemoteModule"/> class.</returns>
        public RemoteModule this[string ModuleName]
        {
            get
            {
                return this.Modules[ModuleName];
            }
        }

        /// <summary>
        /// Gets a pointer to the specified address in the remote process.
        /// </summary>
        /// <param name="Address">The address pointed.</param>
        /// <param name="IsRelative">[Optional] State if the address is relative to the main module.</param>
        /// <returns>A new instance of a <see cref="RemotePointer"/> class.</returns>
        public RemotePointer this[IntPtr Address, bool IsRelative = true]
        {
            get
            {
                return new RemotePointer(this, IsRelative ? this.MakeAbsolute(Address) : Address);
            }
        }

        /// <summary>
        /// Releases all resources used by the <see cref="BattleGroundMemory"/> object.
        /// </summary>
        public virtual void Dispose()
        {
            if (this.OnDispose != null)
            {
                this.OnDispose(this, new EventArgs());
            }

            this.Factories.ForEach(Factory => Factory.Dispose());
            this.Handle.Close();
            GC.SuppressFinalize(this);
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

            return Obj.GetType() == this.GetType() && this.Equals((BattleGroundMemory)Obj);
        }

        /// <summary>
        /// Returns a value indicating whether this instance is equal to a specified object.
        /// </summary>
        public bool Equals(BattleGroundMemory Other)
        {
            if (object.ReferenceEquals(null, Other))
            {
                return false;
            }

            return object.ReferenceEquals(this, Other) || this.Handle.Equals(Other.Handle);
        }

        /// <summary>
        /// Serves as a hash function for a particular type. 
        /// </summary>
        public override int GetHashCode()
        {
            return this.Handle.GetHashCode();
        }

        /// <summary>
        /// Makes an absolute address from a relative one based on the main module.
        /// </summary>
        /// <param name="Address">The relative address.</param>
        /// <returns>The absolute address.</returns>
        public IntPtr MakeAbsolute(IntPtr Address)
        {
            if (Address.ToInt64() > this.Modules.MainModule.Size)
            {
                throw new ArgumentOutOfRangeException("Address", "The relative address cannot be greater than the main module size.");
            }

            return new IntPtr(this.Modules.MainModule.BaseAddress.ToInt64() + Address.ToInt64());
        }

        /// <summary>
        /// Makes a relative address from an absolute one based on the main module.
        /// </summary>
        /// <param name="Address">The absolute address.</param>
        /// <returns>The relative address.</returns>
        public IntPtr MakeRelative(IntPtr Address)
        {
            if (Address.ToInt64() < this.Modules.MainModule.BaseAddress.ToInt64())
            {
                throw new ArgumentOutOfRangeException("Address", "The absolute address cannot be smaller than the main module base address.");
            }

            return new IntPtr(Address.ToInt64() - this.Modules.MainModule.BaseAddress.ToInt64());
        }

        public static bool operator ==(BattleGroundMemory Left, BattleGroundMemory Right)
        {
            return object.Equals(Left, Right);
        }

        public static bool operator !=(BattleGroundMemory Left, BattleGroundMemory Right)
        {
            return !object.Equals(Left, Right);
        }

        /// <summary>
        /// Reads the value of a specified type in the remote process.
        /// </summary>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <param name="Address">The address where the value is read.</param>
        /// <param name="IsRelative">[Optional] State if the address is relative to the main module.</param>
        /// <returns>A value.</returns>
        public T Read<T>(IntPtr Address, bool IsRelative = true)
        {
            return MarshalType<T>.ByteArrayToObject(this.ReadBytes(Address, MarshalType<T>.Size, IsRelative));
        }

        /// <summary>
        /// Reads the value of a specified type in the remote process.
        /// </summary>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <param name="Address">The address where the value is read.</param>
        /// <param name="IsRelative">[Optional] State if the address is relative to the main module.</param>
        /// <returns>A value.</returns>
        public T Read<T>(Enum Address, bool IsRelative = true)
        {
            return this.Read<T>(new IntPtr(Convert.ToInt64(Address)), IsRelative);
        }

        /// <summary>
        /// Reads an array of a specified type in the remote process.
        /// </summary>
        /// <typeparam name="T">The type of the values.</typeparam>
        /// <param name="Address">The address where the values is read.</param>
        /// <param name="Count">The number of cells in the array.</param>
        /// <param name="IsRelative">[Optional] State if the address is relative to the main module.</param>
        /// <returns>An array.</returns>
        public T[] Read<T>(IntPtr Address, int Count, bool IsRelative = true)
        {
            var array = new T[Count];
            var bytes = this.ReadBytes(Address, MarshalType<T>.Size * Count, IsRelative);
            if (typeof(T) != typeof(byte))
            {
                for (var i = 0; i < Count; i++)
                {
                    array[i] = MarshalType<T>.ByteArrayToObject(bytes, MarshalType<T>.Size * i);
                }
            }
            else
            {
                Buffer.BlockCopy(bytes, 0, array, 0, Count);
            }

            return array;
        }

        /// <summary>
        /// Reads an array of a specified type in the remote process.
        /// </summary>
        /// <typeparam name="T">The type of the values.</typeparam>
        /// <param name="Address">The address where the values is read.</param>
        /// <param name="Count">The number of cells in the array.</param>
        /// <param name="IsRelative">[Optional] State if the address is relative to the main module.</param>
        /// <returns>An array.</returns>
        public T[] Read<T>(Enum Address, int Count, bool IsRelative = true)
        {
            return this.Read<T>(new IntPtr(Convert.ToInt64(Address)), Count, IsRelative);
        }

        /// <summary>
        /// Reads a string with a specified encoding in the remote process.
        /// </summary>
        /// <param name="Address">The address where the string is read.</param>
        /// <param name="Encoding">The encoding used.</param>
        /// <param name="IsRelative">[Optional] State if the address is relative to the main module.</param>
        /// <param name="MaxLength">[Optional] The number of maximum bytes to read. The string is automatically cropped at this end ('\0' char).</param>
        /// <returns>The string.</returns>
        public string ReadString(IntPtr Address, Encoding Encoding, bool IsRelative = true, int MaxLength = 512)
        {
            var data = Encoding.GetString(this.ReadBytes(Address, MaxLength, IsRelative));
            var EndOfStringPosition = data.IndexOf('\0');
            return EndOfStringPosition == -1 ? data : data.Substring(0, EndOfStringPosition);
        }

        /// <summary>
        /// Reads a string with a specified encoding in the remote process.
        /// </summary>
        /// <param name="Address">The address where the string is read.</param>
        /// <param name="Encoding">The encoding used.</param>
        /// <param name="IsRelative">[Optional] State if the address is relative to the main module.</param>
        /// <param name="MaxLength">[Optional] The number of maximum bytes to read. The string is automatically cropped at this end ('\0' char).</param>
        /// <returns>The string.</returns>
        public string ReadString(Enum Address, Encoding Encoding, bool IsRelative = true, int MaxLength = 512)
        {
            return this.ReadString(new IntPtr(Convert.ToInt64(Address)), Encoding, IsRelative, MaxLength);
        }

        /// <summary>
        /// Reads a string using the encoding UTF8 in the remote process.
        /// </summary>
        /// <param name="Address">The address where the string is read.</param>
        /// <param name="IsRelative">[Optional] State if the address is relative to the main module.</param>
        /// <param name="MaxLength">[Optional] The number of maximum bytes to read. The string is automatically cropped at this end ('\0' char).</param>
        /// <returns>The string.</returns>
        public string ReadString(IntPtr Address, bool IsRelative = true, int MaxLength = 512)
        {
            return this.ReadString(Address, Encoding.UTF8, IsRelative, MaxLength);
        }

        /// <summary>
        /// Reads a string using the encoding UTF8 in the remote process.
        /// </summary>
        /// <param name="Address">The address where the string is read.</param>
        /// <param name="IsRelative">[Optional] State if the address is relative to the main module.</param>
        /// <param name="MaxLength">[Optional] The number of maximum bytes to read. The string is automatically cropped at this end ('\0' char).</param>
        /// <returns>The string.</returns>
        public string ReadString(Enum Address, bool IsRelative = true, int MaxLength = 512)
        {
            return this.ReadString(new IntPtr(Convert.ToInt64(Address)), IsRelative, MaxLength);
        }

        /// <summary>
        /// Writes the values of a specified type in the remote process.
        /// </summary>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <param name="Address">The address where the value is written.</param>
        /// <param name="Value">The value to write.</param>
        /// <param name="IsRelative">[Optional] State if the address is relative to the main module.</param>
        public void Write<T>(IntPtr Address, T Value, bool IsRelative = true)
        {
            this.WriteBytes(Address, MarshalType<T>.ObjectToByteArray(Value), IsRelative);
        }

        /// <summary>
        /// Writes the values of a specified type in the remote process.
        /// </summary>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <param name="Address">The address where the value is written.</param>
        /// <param name="Value">The value to write.</param>
        /// <param name="IsRelative">[Optional] State if the address is relative to the main module.</param>
        public void Write<T>(Enum Address, T Value, bool IsRelative = true)
        {
            this.Write(new IntPtr(Convert.ToInt64(Address)), Value, IsRelative);
        }

        /// <summary>
        /// Writes an array of a specified type in the remote process.
        /// </summary>
        /// <typeparam name="T">The type of the values.</typeparam>
        /// <param name="Address">The address where the values is written.</param>
        /// <param name="Array">The array to write.</param>
        /// <param name="IsRelative">[Optional] State if the address is relative to the main module.</param>
        public void Write<T>(IntPtr Address, T[] Array, bool IsRelative = true)
        {
            var ValuesInBytes = new byte[MarshalType<T>.Size * Array.Length];
            for (var i = 0; i < Array.Length; i++)
            {
                var OffsetInArray = MarshalType<T>.Size * i;
                Buffer.BlockCopy(MarshalType<T>.ObjectToByteArray(Array[i]), 0, ValuesInBytes, OffsetInArray, MarshalType<T>.Size);
            }

            this.WriteBytes(Address, ValuesInBytes, IsRelative);
        }

        /// <summary>
        /// Writes an array of a specified type in the remote process.
        /// </summary>
        /// <typeparam name="T">The type of the values.</typeparam>
        /// <param name="Address">The address where the values is written.</param>
        /// <param name="Array">The array to write.</param>
        /// <param name="IsRelative">[Optional] State if the address is relative to the main module.</param>
        public void Write<T>(Enum Address, T[] Array, bool IsRelative = true)
        {
            this.Write(new IntPtr(Convert.ToInt64(Address)), Array, IsRelative);
        }

        /// <summary>
        /// Writes a string with a specified encoding in the remote process.
        /// </summary>
        /// <param name="Address">The address where the string is written.</param>
        /// <param name="Text">The text to write.</param>
        /// <param name="Encoding">The encoding used.</param>
        /// <param name="IsRelative">[Optional] State if the address is relative to the main module.</param>
        public void WriteString(IntPtr Address, string Text, Encoding Encoding, bool IsRelative = true)
        {
            this.WriteBytes(Address, Encoding.GetBytes(Text + '\0'), IsRelative);
        }

        /// <summary>
        /// Writes a string with a specified encoding in the remote process.
        /// </summary>
        /// <param name="Address">The address where the string is written.</param>
        /// <param name="Text">The text to write.</param>
        /// <param name="Encoding">The encoding used.</param>
        /// <param name="IsRelative">[Optional] State if the address is relative to the main module.</param>
        public void WriteString(Enum Address, string Text, Encoding Encoding, bool IsRelative = true)
        {
            this.WriteString(new IntPtr(Convert.ToInt64(Address)), Text, Encoding, IsRelative);
        }

        /// <summary>
        /// Writes a string using the encoding UTF8 in the remote process.
        /// </summary>
        /// <param name="Address">The address where the string is written.</param>
        /// <param name="Text">The text to write.</param>
        /// <param name="IsRelative">[Optional] State if the address is relative to the main module.</param>
        public void WriteString(IntPtr Address, string Text, bool IsRelative = true)
        {
            this.WriteString(Address, Text, Encoding.UTF8, IsRelative);
        }

        /// <summary>
        /// Writes a string using the encoding UTF8 in the remote process.
        /// </summary>
        /// <param name="Address">The address where the string is written.</param>
        /// <param name="Text">The text to write.</param>
        /// <param name="IsRelative">[Optional] State if the address is relative to the main module.</param>
        public void WriteString(Enum Address, string Text, bool IsRelative = true)
        {
            this.WriteString(new IntPtr(Convert.ToInt64(Address)), Text, IsRelative);
        }

        /// <summary>
        /// Reads an array of bytes in the remote process.
        /// </summary>
        /// <param name="Address">The address where the array is read.</param>
        /// <param name="Count">The number of cells.</param>
        /// <param name="IsRelative">[Optional] State if the address is relative to the main module.</param>
        /// <returns>The array of bytes.</returns>
        protected byte[] ReadBytes(IntPtr Address, int Count, bool IsRelative = true)
        {
            return MemoryCore.ReadBytes(this.Handle, IsRelative ? this.MakeAbsolute(Address) : Address, Count);
        }

        /// <summary>
        /// Write an array of bytes in the remote process.
        /// </summary>
        /// <param name="Address">The address where the array is written.</param>
        /// <param name="ByteArray">The array of bytes to write.</param>
        /// <param name="IsRelative">[Optional] State if the address is relative to the main module.</param>
        protected void WriteBytes(IntPtr Address, byte[] ByteArray, bool IsRelative = true)
        {
            using (new MemoryProtection(this, IsRelative ? this.MakeAbsolute(Address) : Address, MarshalType<byte>.Size * ByteArray.Length))
            {
                MemoryCore.WriteBytes(this.Handle, IsRelative ? this.MakeAbsolute(Address) : Address, ByteArray);
            }
        }
    }
}