namespace PlayerUnknown.Reader.Memory
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;

    using PlayerUnknown.Reader.Helpers;
    using PlayerUnknown.Reader.Internals;
    using PlayerUnknown.Reader.Native;

    /// <summary>
    /// Static core class providing tools for memory editing.
    /// </summary>
    public static class MemoryCore
    {
        /// <summary>
        /// Reserves a region of memory within the virtual address space of a specified process.
        /// </summary>
        /// <param name="ProcessHandle">The handle to a process.</param>
        /// <param name="Size">The size of the region of memory to allocate, in bytes.</param>
        /// <param name="ProtectionFlags">The memory protection for the region of pages to be allocated.</param>
        /// <param name="AllocationFlags">The type of memory allocation.</param>
        /// <returns>The base address of the allocated region.</returns>
        public static IntPtr Allocate(SafeMemoryHandle ProcessHandle, int Size, MemoryProtectionFlags ProtectionFlags = MemoryProtectionFlags.ExecuteReadWrite, MemoryAllocationFlags AllocationFlags = MemoryAllocationFlags.Commit)
        {
            // Check if the handle is valid
            HandleManipulator.ValidateAsArgument(ProcessHandle, "processHandle");

            // Allocate a memory page
            var ret = NativeMethods.VirtualAllocEx(ProcessHandle, IntPtr.Zero, Size, AllocationFlags, ProtectionFlags);

            // Check whether the memory page is valid
            if (ret != IntPtr.Zero)
            {
                return ret;
            }

            // If the pointer isn't valid, throws an exception
            throw new Win32Exception(string.Format("Couldn't allocate memory of {0} byte(s).", Size));
        }

        /// <summary>
        /// Closes an open object handle.
        /// </summary>
        /// <param name="Handle">A valid handle to an open object.</param>
        public static void CloseHandle(IntPtr Handle)
        {
            // Check if the handle is valid
            HandleManipulator.ValidateAsArgument(Handle, "handle");

            // Close the handle
            if (!NativeMethods.CloseHandle(Handle))
            {
                throw new Win32Exception(string.Format("Couldn't close he handle 0x{0}.", Handle));
            }
        }

        /// <summary>
        /// Releases a region of memory within the virtual address space of a specified process.
        /// </summary>
        /// <param name="ProcessHandle">A handle to a process.</param>
        /// <param name="Address">A pointer to the starting address of the region of memory to be freed.</param>
        public static void Free(SafeMemoryHandle ProcessHandle, IntPtr Address)
        {
            // Check if the handles are valid
            HandleManipulator.ValidateAsArgument(ProcessHandle, "processHandle");
            HandleManipulator.ValidateAsArgument(Address, "address");

            // Free the memory
            if (!NativeMethods.VirtualFreeEx(ProcessHandle, Address, 0, MemoryReleaseFlags.Release))
            {
                // If the memory wasn't correctly freed, throws an exception
                throw new Win32Exception(string.Format("The memory page 0x{0} cannot be freed.", Address.ToString("X")));
            }
        }

        /// <summary>
        /// etrieves information about the specified process.
        /// </summary>
        /// <param name="ProcessHandle">A handle to the process to query.</param>
        /// <returns>A <see cref="ProcessBasicInformation"/> structure containg process information.</returns>
        public static ProcessBasicInformation NtQueryInformationProcess(SafeMemoryHandle ProcessHandle)
        {
            // Check if the handle is valid
            HandleManipulator.ValidateAsArgument(ProcessHandle, "processHandle");

            // Create a structure to store process info
            var info = new ProcessBasicInformation();

            // Get the process info
            var ret = NativeMethods.NtQueryInformationProcess(ProcessHandle, ProcessInformationClass.ProcessBasicInformation, ref info, info.Size, IntPtr.Zero);

            // If the function succeeded
            if (ret == 0)
            {
                return info;
            }

            // Else, couldn't get the process info, throws an exception
            throw new ApplicationException(string.Format("Couldn't get the information from the process, error code '{0}'.", ret));
        }

        /// <summary>
        /// Opens an existing local process object.
        /// </summary>
        /// <param name="AccessFlags">The access level to the process object.</param>
        /// <param name="ProcessId">The identifier of the local process to be opened.</param>
        /// <returns>An open handle to the specified process.</returns>
        public static SafeMemoryHandle OpenProcess(ProcessAccessFlags AccessFlags, int ProcessId)
        {
            // Get an handle from the remote process
            var handle = NativeMethods.OpenProcess(AccessFlags, false, ProcessId);

            // Check whether the handle is valid
            if (!handle.IsInvalid && !handle.IsClosed)
            {
                return handle;
            }

            // Else the handle isn't valid, throws an exception
            throw new Win32Exception(string.Format("Couldn't open the process {0}.", ProcessId));
        }

        /// <summary>
        /// Reads an array of bytes in the memory form the target process.
        /// </summary>
        /// <param name="ProcessHandle">A handle to the process with memory that is being read.</param>
        /// <param name="Address">A pointer to the base address in the specified process from which to read.</param>
        /// <param name="Size">The number of bytes to be read from the specified process.</param>
        /// <returns>The collection of read bytes.</returns>
        public static byte[] ReadBytes(SafeMemoryHandle ProcessHandle, IntPtr Address, int Size)
        {
            // Check if the handles are valid
            HandleManipulator.ValidateAsArgument(ProcessHandle, "processHandle");
            HandleManipulator.ValidateAsArgument(Address, "address");

            // Allocate the buffer
            var buffer = new byte[Size];
            int NbBytesRead;

            // Read the data from the target process
            if (NativeMethods.ReadProcessMemory(ProcessHandle, Address, buffer, Size, out NbBytesRead) && Size == NbBytesRead)
            {
                return buffer;
            }

            // Else the data couldn't be read, throws an exception
            throw new Win32Exception(string.Format("Couldn't read {0} byte(s) from 0x{1}.", Size, Address.ToString("X")));
        }

        /// <summary>
        /// Changes the protection on a region of committed pages in the virtual address space of a specified process.
        /// </summary>
        /// <param name="ProcessHandle">A handle to the process whose memory protection is to be changed.</param>
        /// <param name="Address">A pointer to the base address of the region of pages whose access protection attributes are to be changed.</param>
        /// <param name="Size">The size of the region whose access protection attributes are changed, in bytes.</param>
        /// <param name="Protection">The memory protection option.</param>
        /// <returns>The old protection of the region in a <see cref="Native.MemoryBasicInformation"/> structure.</returns>
        public static MemoryProtectionFlags ChangeProtection(SafeMemoryHandle ProcessHandle, IntPtr Address, int Size, MemoryProtectionFlags Protection)
        {
            // Check if the handles are valid
            HandleManipulator.ValidateAsArgument(ProcessHandle, "processHandle");
            HandleManipulator.ValidateAsArgument(Address, "address");

            // Create the variable storing the old protection of the memory page
            MemoryProtectionFlags OldProtection;

            // Change the protection in the target process
            if (NativeMethods.VirtualProtectEx(ProcessHandle, Address, Size, Protection, out OldProtection))
            {
                // Return the old protection
                return OldProtection;
            }

            // Else the protection couldn't be changed, throws an exception
            throw new Win32Exception(string.Format("Couldn't change the protection of the memory at 0x{0} of {1} byte(s) to {2}.", Address.ToString("X"), Size, Protection));
        }

        /// <summary>
        /// Retrieves information about a range of pages within the virtual address space of a specified process.
        /// </summary>
        /// <param name="ProcessHandle">A handle to the process whose memory information is queried.</param>
        /// <param name="BaseAddress">A pointer to the base address of the region of pages to be queried.</param>
        /// <returns>A <see cref="Native.MemoryBasicInformation"/> structures in which information about the specified page range is returned.</returns>
        public static MemoryBasicInformation Query(SafeMemoryHandle ProcessHandle, IntPtr BaseAddress)
        {
            // Allocate the structure to store information of memory
            MemoryBasicInformation MemoryInfo;

            // Query the memory region
            if (NativeMethods.VirtualQueryEx(ProcessHandle, BaseAddress, out MemoryInfo, MarshalType<MemoryBasicInformation>.Size) != 0)
            {
                return MemoryInfo;
            }

            // Else the information couldn't be got
            throw new Win32Exception(string.Format("Couldn't query information about the memory region 0x{0}", BaseAddress.ToString("X")));
        }

        /// <summary>
        /// Retrieves information about a range of pages within the virtual address space of a specified process.
        /// </summary>
        /// <param name="ProcessHandle">A handle to the process whose memory information is queried.</param>
        /// <param name="AddressFrom">A pointer to the starting address of the region of pages to be queried.</param>
        /// <param name="AddressTo">A pointer to the ending address of the region of pages to be queried.</param>
        /// <returns>A collection of <see cref="Native.MemoryBasicInformation"/> structures.</returns>
        public static IEnumerable<MemoryBasicInformation> Query(SafeMemoryHandle ProcessHandle, IntPtr AddressFrom, IntPtr AddressTo)
        {
            // Check if the handle is valid
            HandleManipulator.ValidateAsArgument(ProcessHandle, "processHandle");

            // Convert the addresses to Int64
            var NumberFrom = AddressFrom.ToInt64();
            var NumberTo = AddressTo.ToInt64();

            // The first address must be lower than the second
            if (NumberFrom >= NumberTo)
            {
                throw new ArgumentException("The starting address must be lower than the ending address.", "AddressFrom");
            }

            // Create the variable storing the result of the call of VirtualQueryEx
            int ret;

            // Enumerate the memory pages
            do
            {
                // Allocate the structure to store information of memory
                MemoryBasicInformation MemoryInfo;

                // Get the next memory page
                ret = NativeMethods.VirtualQueryEx(ProcessHandle, new IntPtr(NumberFrom), out MemoryInfo, MarshalType<MemoryBasicInformation>.Size);

                // Increment the starting address with the size of the page
                NumberFrom += MemoryInfo.RegionSize;

                // Return the memory page
                if (MemoryInfo.State != MemoryStateFlags.Free)
                {
                    yield return MemoryInfo;
                }
            }
            while (NumberFrom < NumberTo && ret != 0);
        }

        /// <summary>
        /// Writes data to an area of memory in a specified process.
        /// </summary>
        /// <param name="ProcessHandle">A handle to the process memory to be modified.</param>
        /// <param name="Address">A pointer to the base address in the specified process to which data is written.</param>
        /// <param name="ByteArray">A buffer that contains data to be written in the address space of the specified process.</param>
        /// <returns>The number of bytes written.</returns>
        public static int WriteBytes(SafeMemoryHandle ProcessHandle, IntPtr Address, byte[] ByteArray)
        {
            // Check if the handles are valid
            HandleManipulator.ValidateAsArgument(ProcessHandle, "processHandle");
            HandleManipulator.ValidateAsArgument(Address, "address");

            // Create the variable storing the number of bytes written
            int NbBytesWritten;

            // Write the data to the target process
            if (NativeMethods.WriteProcessMemory(ProcessHandle, Address, ByteArray, ByteArray.Length, out NbBytesWritten))
            {
                // Check whether the length of the data written is equal to the inital array
                if (NbBytesWritten == ByteArray.Length)
                {
                    return NbBytesWritten;
                }
            }

            // Else the data couldn't be written, throws an exception
            throw new Win32Exception(string.Format("Couldn't write {0} bytes to 0x{1}", ByteArray.Length, Address.ToString("X")));
        }
    }
}