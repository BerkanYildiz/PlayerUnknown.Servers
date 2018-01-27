namespace PlayerUnknown.Reader.Helpers
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Linq;

    using PlayerUnknown.Reader.Native;

    /// <summary>
    /// Static helper class providing tools for manipulating handles.
    /// </summary>
    public static class HandleManipulator
    {
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
                throw new Win32Exception("Couldn't close the handle correctly.");
            }
        }

        /// <summary>
        /// Converts an handle into a <see cref="Process"/> object assuming this is a process handle.
        /// </summary>
        /// <param name="ProcessHandle">A valid handle to an opened process.</param>
        /// <returns>A <see cref="Process"/> object from the specified handle.</returns>
        public static Process HandleToProcess(SafeMemoryHandle ProcessHandle)
        {
            // Search the process by iterating the processes list
            return Process.GetProcesses().First(P => P.Id == HandleManipulator.HandleToProcessId(ProcessHandle));
        }

        /// <summary>
        /// Converts an handle into a process id assuming this is a process handle.
        /// </summary>
        /// <param name="ProcessHandle">A valid handle to an opened process.</param>
        /// <returns>A process id from the specified handle.</returns>
        public static int HandleToProcessId(SafeMemoryHandle ProcessHandle)
        {
            // Check if the handle is valid
            HandleManipulator.ValidateAsArgument(ProcessHandle, "processHandle");

            // Find the process id
            var ret = NativeMethods.GetProcessId(ProcessHandle);

            // If the process id is valid
            if (ret != 0)
            {
                return ret;
            }

            // Else the function failed, throws an exception
            throw new Win32Exception("Couldn't find the process id of the specified handle.");
        }

        /// <summary>
        /// Converts an handle into a <see cref="ProcessThread"/> object assuming this is a thread handle.
        /// </summary>
        /// <param name="ThreadHandle">A valid handle to an opened thread.</param>
        /// <returns>A <see cref="ProcessThread"/> object from the specified handle.</returns>
        public static ProcessThread HandleToThread(SafeMemoryHandle ThreadHandle)
        {
            // Search the thread by iterating the processes list
            foreach (var process in Process.GetProcesses())
            {
                var ret = process.Threads.Cast<ProcessThread>().FirstOrDefault(T => T.Id == HandleManipulator.HandleToThreadId(ThreadHandle));
                if (ret != null)
                {
                    return ret;
                }
            }

            // If no thread was found, throws a exception like the First() function with no element
            throw new InvalidOperationException("Sequence contains no matching element");
        }

        /// <summary>
        /// Converts an handle into a thread id assuming this is a thread handle.
        /// </summary>
        /// <param name="ThreadHandle">A valid handle to an opened thread.</param>
        /// <returns>A thread id from the specified handle.</returns>
        public static int HandleToThreadId(SafeMemoryHandle ThreadHandle)
        {
            // Check if the handle is valid
            HandleManipulator.ValidateAsArgument(ThreadHandle, "threadHandle");

            // Find the thread id
            var ret = NativeMethods.GetThreadId(ThreadHandle);

            // If the thread id is valid
            if (ret != 0)
            {
                return ret;
            }

            // Else the function failed, throws an exception
            throw new Win32Exception("Couldn't find the thread id of the specified handle.");
        }

        /// <summary>
        /// Validates an handle to fit correctly as argument.
        /// </summary>
        /// <param name="Handle">A handle to validate.</param>
        /// <param name="ArgumentName">The name of the argument that represents the handle in its original function.</param>
        public static void ValidateAsArgument(IntPtr Handle, string ArgumentName)
        {
            // Check if the handle is not null
            if (Handle == null)
            {
                throw new ArgumentNullException(ArgumentName);
            }

            // Check if the handle is valid
            if (Handle == IntPtr.Zero)
            {
                throw new ArgumentException("The handle is not valid.", ArgumentName);
            }
        }

        /// <summary>
        /// Validates an handle to fit correctly as argument.
        /// </summary>
        /// <param name="Handle">A handle to validate.</param>
        /// <param name="ArgumentName">The name of the argument that represents the handle in its original function.</param>
        public static void ValidateAsArgument(SafeMemoryHandle Handle, string ArgumentName)
        {
            // Check if the handle is not null
            if (Handle == null)
            {
                throw new ArgumentNullException(ArgumentName);
            }

            // Check if the handle is valid
            if (Handle.IsClosed || Handle.IsInvalid)
            {
                throw new ArgumentException("The handle is not valid or closed.", ArgumentName);
            }
        }
    }
}