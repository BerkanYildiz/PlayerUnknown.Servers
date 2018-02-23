namespace PlayerUnknown.Helpers
{
    using System;
    using System.Linq;
    using System.Runtime.InteropServices;
    using System.Text;

    public class Memory
    {
        /// <summary>
        /// Gets the handle.
        /// </summary>
        private IntPtr Handle
        {
            get;
        }

        /// <summary>
        /// Gets the base.
        /// </summary>
        public IntPtr Base
        {
            get;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Memory"/> class.
        /// </summary>
        /// <param name="Handle">The handle.</param>
        /// <param name="Base">The base.</param>
        public Memory(IntPtr Handle, IntPtr Base)
        {
            this.Handle = Handle;
            this.Base   = Base;
        }

        /// <summary>
        /// Get module handle
        /// </summary>
        /// <param name="ModuleName">Module Name</param>
        public static T GetStructure<T>(byte[] Bytes)
        {
            var handle = GCHandle.Alloc(Bytes, GCHandleType.Pinned);
            var structure = (T) Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(T));
            handle.Free();
            return structure;
        }

        /// <summary>
        /// Gets the structure.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Bytes">The bytes.</param>
        /// <param name="Index">The index.</param>
        public static T GetStructure<T>(byte[] Bytes, int Index)
        {
            int size = Marshal.SizeOf(typeof(T));
            byte[] tmp = new byte[size];
            Array.Copy(Bytes, Index, tmp, 0, size);
            return Memory.GetStructure<T>(tmp);
        }

        /// <summary>
        /// Read process memory
        /// </summary>
        /// <typeparam name="T">Data Type</typeparam>
        /// <param name="Address">Memory Address</param>
        /// <returns></returns>
        public T Read<T>(IntPtr Address)
        {
            var size = Marshal.SizeOf(typeof(T));
            var data = this.Read(Address, size);
            return Memory.GetStructure<T>(data);
        }

        /// <summary>
        /// Write Process Memory
        /// </summary>
        /// <typeparam name="T">Data Type</typeparam>
        /// <param name="Input">Data</param>
        /// <param name="Address">Memory Address</param>
        public void Write<T>(T Input, IntPtr Address)
        {
            int size = Marshal.SizeOf(Input);
            byte[] arr = new byte[size];

            IntPtr ptr = Marshal.AllocHGlobal(size);
            Marshal.StructureToPtr(Input, ptr, true);
            Marshal.Copy(ptr, arr, 0, size);
            Marshal.FreeHGlobal(ptr);
            this.WriteMemory(arr, Address);
        }

        /// <summary>
        /// Read a chunk from memory
        /// </summary>
        /// <param name="Address">Address</param>
        /// <param name="data">Data</param>
        /// <param name="Length">Length of chunk</param>
        public byte[] Read(IntPtr Address, int Length)
        {
            byte[] TempData = new byte[Length];
            bool result = Win32.ReadProcessMemory(this.Handle, Address, TempData, Length, 0);
            return TempData;
        }

        /// <summary>
        /// Read a char[255] from memory
        /// </summary>
        /// <param name="Address">Address</param>
        /// <returns>String from memory</returns>
        public string ReadString(IntPtr Address)
        {
            byte[] NumArray = this.Read(Address, 255);
            var str = Encoding.Default.GetString(NumArray);

            if (str.Contains('\0'))
                str = str.Substring(0, str.IndexOf('\0'));
            return str;
        }

        /// <summary>
        /// Writes bytes in memory.
        /// </summary>
        /// <param name="Bytes">The bytes.</param>
        /// <param name="Address">The address.</param> 
        public void WriteMemory(byte[] Bytes, IntPtr Address)
        {
            Win32.WriteProcessMemory(this.Handle, Address, Bytes, Bytes.Length, 0);
        }
    }
}