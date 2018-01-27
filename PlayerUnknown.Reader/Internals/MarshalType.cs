namespace PlayerUnknown.Reader.Internals
{
    using System;
    using System.Runtime.InteropServices;
    using System.Text;

    using PlayerUnknown.Reader.Memory;

    /// <summary>
    /// Static class providing tools for extracting information related to types.
    /// </summary>
    /// <typeparam name="T">Type to analyze.</typeparam>
    public static class MarshalType<T>
    {
        /// <summary>
        /// Initializes static information related to the specified type.
        /// </summary>
        static MarshalType()
        {
            // Gather information related to the provided type
            MarshalType<T>.IsIntPtr = typeof(T) == typeof(IntPtr);
            MarshalType<T>.RealType = typeof(T);
            MarshalType<T>.Size = MarshalType<T>.TypeCode == TypeCode.Boolean ? 1 : Marshal.SizeOf(MarshalType<T>.RealType);
            MarshalType<T>.TypeCode = Type.GetTypeCode(MarshalType<T>.RealType);

            // Check if the type can be stored in registers
            MarshalType<T>.CanBeStoredInRegisters = MarshalType<T>.IsIntPtr ||
#if x64
                TypeCode == TypeCode.Int64 ||
                TypeCode == TypeCode.UInt64 ||
#endif
                                                    MarshalType<T>.TypeCode == TypeCode.Boolean || MarshalType<T>.TypeCode == TypeCode.Byte || MarshalType<T>.TypeCode == TypeCode.Char || MarshalType<T>.TypeCode == TypeCode.Int16 || MarshalType<T>.TypeCode == TypeCode.Int32 || MarshalType<T>.TypeCode == TypeCode.Int64 || MarshalType<T>.TypeCode == TypeCode.SByte || MarshalType<T>.TypeCode == TypeCode.Single || MarshalType<T>.TypeCode == TypeCode.UInt16 || MarshalType<T>.TypeCode == TypeCode.UInt32;
        }

        /// <summary>
        /// Gets if the type can be stored in a registers (for example ACX, ECX, ...).
        /// </summary>
        public static bool CanBeStoredInRegisters
        {
            get;
        }

        /// <summary>
        /// State if the type is <see cref="IntPtr"/>.
        /// </summary>
        public static bool IsIntPtr
        {
            get;
        }

        /// <summary>
        /// The real type.
        /// </summary>
        public static Type RealType
        {
            get;
        }

        /// <summary>
        /// The size of the type.
        /// </summary>
        public static int Size
        {
            get;
        }

        /// <summary>
        /// The typecode of the type.
        /// </summary>
        public static TypeCode TypeCode
        {
            get;
        }

        /// <summary>
        /// Marshals a managed object to an array of bytes.
        /// </summary>
        /// <param name="Obj">The object to marshal.</param>
        /// <returns>A array of bytes corresponding to the managed object.</returns>
        public static byte[] ObjectToByteArray(T Obj)
        {
            // We'll tried to avoid marshalling as it really slows the process
            // First, check if the type can be converted without marhsalling
            switch (MarshalType<T>.TypeCode)
            {
                case TypeCode.Object:
                    if (MarshalType<T>.IsIntPtr)
                    {
                        switch (MarshalType<T>.Size)
                        {
                            case 4:
                                return BitConverter.GetBytes(((IntPtr)(object)Obj).ToInt32());
                            case 8:
                                return BitConverter.GetBytes(((IntPtr)(object)Obj).ToInt64());
                        }
                    }

                    break;
                case TypeCode.Boolean:
                    return BitConverter.GetBytes((bool)(object)Obj);
                case TypeCode.Char:
                    return Encoding.UTF8.GetBytes(
                        new[]
                            {
                                (char)(object)Obj
                            });
                case TypeCode.Double:
                    return BitConverter.GetBytes((double)(object)Obj);
                case TypeCode.Int16:
                    return BitConverter.GetBytes((short)(object)Obj);
                case TypeCode.Int32:
                    return BitConverter.GetBytes((int)(object)Obj);
                case TypeCode.Int64:
                    return BitConverter.GetBytes((long)(object)Obj);
                case TypeCode.Single:
                    return BitConverter.GetBytes((float)(object)Obj);
                case TypeCode.String:
                    throw new InvalidCastException("This method doesn't support string conversion.");
                case TypeCode.UInt16:
                    return BitConverter.GetBytes((ushort)(object)Obj);
                case TypeCode.UInt32:
                    return BitConverter.GetBytes((uint)(object)Obj);
                case TypeCode.UInt64:
                    return BitConverter.GetBytes((ulong)(object)Obj);
            }

            // Check if it's not a common type
            // Allocate a block of unmanaged memory
            using (var unmanaged = new LocalUnmanagedMemory(MarshalType<T>.Size))
            {
                // Write the object inside the unmanaged memory
                unmanaged.Write(Obj);

                // Return the content of the block of unmanaged memory
                return unmanaged.Read();
            }
        }

        /// <summary>
        /// Marshals an array of byte to a managed object.
        /// </summary>
        /// <param name="ByteArray">The array of bytes corresponding to a managed object.</param>
        /// <param name="Index">[Optional] Where to start the conversion of bytes to the managed object.</param>
        /// <returns>A managed object.</returns>
        public static T ByteArrayToObject(byte[] ByteArray, int Index = 0)
        {
            // We'll tried to avoid marshalling as it really slows the process
            // First, check if the type can be converted without marshalling
            switch (MarshalType<T>.TypeCode)
            {
                case TypeCode.Object:
                    if (MarshalType<T>.IsIntPtr)
                    {
                        switch (ByteArray.Length)
                        {
                            case 1:
                                return (T)(object)new IntPtr(
                                    BitConverter.ToInt32(
                                        new byte[]
                                            {
                                                ByteArray[Index], 0x0, 0x0, 0x0
                                            },
                                        Index));
                            case 2:
                                return (T)(object)new IntPtr(
                                    BitConverter.ToInt32(
                                        new byte[]
                                            {
                                                ByteArray[Index], ByteArray[Index + 1], 0x0, 0x0
                                            },
                                        Index));
                            case 4:
                                return (T)(object)new IntPtr(BitConverter.ToInt32(ByteArray, Index));
                            case 8:
                                return (T)(object)new IntPtr(BitConverter.ToInt64(ByteArray, Index));
                        }
                    }

                    break;
                case TypeCode.Boolean:
                    return (T)(object)BitConverter.ToBoolean(ByteArray, Index);
                case TypeCode.Byte:
                    return (T)(object)ByteArray[Index];
                case TypeCode.Char:
                    return (T)(object)Encoding.UTF8.GetChars(ByteArray)[Index];
                case TypeCode.Double:
                    return (T)(object)BitConverter.ToDouble(ByteArray, Index);
                case TypeCode.Int16:
                    return (T)(object)BitConverter.ToInt16(ByteArray, Index);
                case TypeCode.Int32:
                    return (T)(object)BitConverter.ToInt32(ByteArray, Index);
                case TypeCode.Int64:
                    return (T)(object)BitConverter.ToInt64(ByteArray, Index);
                case TypeCode.Single:
                    return (T)(object)BitConverter.ToSingle(ByteArray, Index);
                case TypeCode.String:
                    throw new InvalidCastException("This method doesn't support string conversion.");
                case TypeCode.UInt16:
                    return (T)(object)BitConverter.ToUInt16(ByteArray, Index);
                case TypeCode.UInt32:
                    return (T)(object)BitConverter.ToUInt32(ByteArray, Index);
                case TypeCode.UInt64:
                    return (T)(object)BitConverter.ToUInt64(ByteArray, Index);
            }

            // Allocate a block of unmanaged memory
            using (var unmanaged = new LocalUnmanagedMemory(MarshalType<T>.Size))
            {
                // Write the array of bytes inside the unmanaged memory
                unmanaged.Write(ByteArray, Index);

                // Return a managed object created from the block of unmanaged memory
                return unmanaged.Read<T>();
            }
        }

        /// <summary>
        /// Converts a pointer to a given type. This function converts the value of the pointer or the pointed value,
        /// according if the data type is primitive or reference.
        /// </summary>
        /// <param name="BattleGroundMemory">The concerned process.</param>
        /// <param name="Pointer">The pointer to convert.</param>
        /// <returns>The return value is the pointer converted to the given data type.</returns>
        public static T PtrToObject(BattleGroundMemory BattleGroundMemory, IntPtr Pointer)
        {
            return MarshalType<T>.ByteArrayToObject(MarshalType<T>.CanBeStoredInRegisters ? BitConverter.GetBytes(Pointer.ToInt64()) : BattleGroundMemory.Read<byte>(Pointer, MarshalType<T>.Size, false));
        }
    }
}