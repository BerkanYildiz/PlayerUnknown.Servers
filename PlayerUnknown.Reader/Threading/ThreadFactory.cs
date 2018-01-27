namespace PlayerUnknown.Reader.Threading
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;

    using PlayerUnknown.Reader.Internals;
    using PlayerUnknown.Reader.Native;

    /// <summary>
    /// Class providing tools for manipulating threads.
    /// </summary>
    public class ThreadFactory : IFactory
    {
        /// <summary>
        /// The reference of the <see cref="BattleGroundMemory"/> object.
        /// </summary>
        protected readonly BattleGroundMemory BattleGroundMemory;

        /// <summary>
        /// Initializes a new instance of the <see cref="ThreadFactory"/> class.
        /// </summary>
        /// <param name="BattleGroundMemory">The reference of the <see cref="BattleGroundMemory"/> object.</param>
        internal ThreadFactory(BattleGroundMemory BattleGroundMemory)
        {
            // Save the parameter
            this.BattleGroundMemory = BattleGroundMemory;
        }

        /// <summary>
        /// Gets the main thread of the remote process.
        /// </summary>
        public RemoteThread MainThread
        {
            get
            {
                return new RemoteThread(this.BattleGroundMemory, this.NativeThreads.Aggregate((Current, Next) => Next.StartTime < Current.StartTime ? Next : Current));
            }
        }

        /// <summary>
        /// Gets the threads from the remote process.
        /// </summary>
        public IEnumerable<RemoteThread> RemoteThreads
        {
            get
            {
                return this.NativeThreads.Select(T => new RemoteThread(this.BattleGroundMemory, T));
            }
        }

        /// <summary>
        /// Gets the native threads from the remote process.
        /// </summary>
        internal IEnumerable<ProcessThread> NativeThreads
        {
            get
            {
                // Refresh the process info
                this.BattleGroundMemory.Native.Refresh();

                // Enumerates all threads
                return this.BattleGroundMemory.Native.Threads.Cast<ProcessThread>();
            }
        }

        /// <summary>
        /// Gets the thread corresponding to an id.
        /// </summary>
        /// <param name="ThreadId">The unique identifier of the thread to get.</param>
        /// <returns>A new instance of a <see cref="RemoteThread"/> class.</returns>
        public RemoteThread this[int ThreadId]
        {
            get
            {
                return new RemoteThread(this.BattleGroundMemory, this.NativeThreads.First(T => T.Id == ThreadId));
            }
        }

        /// <summary>
        /// Creates a thread that runs in the remote process.
        /// </summary>
        /// <param name="Address">
        /// A pointer to the application-defined function to be executed by the thread and represents 
        /// the starting address of the thread in the remote process.
        /// </param>
        /// <param name="Parameter">A variable to be passed to the thread function.</param>
        /// <param name="IsStarted">Sets if the thread must be started just after being created.</param>
        /// <returns>A new instance of the <see cref="RemoteThread"/> class.</returns>
        public RemoteThread Create(IntPtr Address, dynamic Parameter, bool IsStarted = true)
        {
            // Marshal the parameter
            var MarshalledParameter = MarshalValue.Marshal(this.BattleGroundMemory, Parameter);

            // Create the thread
            var ret = ThreadCore.NtQueryInformationThread(ThreadCore.CreateRemoteThread(this.BattleGroundMemory.Handle, Address, MarshalledParameter.Reference, ThreadCreationFlags.Suspended));

            // Get the native thread previously created
            // Loop until the native thread is retrieved
            ProcessThread NativeThread;
            do
            {
                NativeThread = this.BattleGroundMemory.Threads.NativeThreads.FirstOrDefault(T => T.Id == ret.ThreadId);
            }
            while (NativeThread == null);

            // Find the managed object corresponding to this thread
            var result = new RemoteThread(this.BattleGroundMemory, NativeThread, MarshalledParameter);

            // If the thread must be started
            if (IsStarted)
            {
                result.Resume();
            }

            return result;
        }

        /// <summary>
        /// Creates a thread that runs in the remote process.
        /// </summary>
        /// <param name="Address">
        /// A pointer to the application-defined function to be executed by the thread and represents 
        /// the starting address of the thread in the remote process.
        /// </param>
        /// <param name="IsStarted">Sets if the thread must be started just after being created.</param>
        /// <returns>A new instance of the <see cref="RemoteThread"/> class.</returns>
        public RemoteThread Create(IntPtr Address, bool IsStarted = true)
        {
            // Create the thread
            var ret = ThreadCore.NtQueryInformationThread(ThreadCore.CreateRemoteThread(this.BattleGroundMemory.Handle, Address, IntPtr.Zero, ThreadCreationFlags.Suspended));

            // Get the native thread previously created
            // Loop until the native thread is retrieved
            ProcessThread NativeThread;
            do
            {
                NativeThread = this.BattleGroundMemory.Threads.NativeThreads.FirstOrDefault(T => T.Id == ret.ThreadId);
            }
            while (NativeThread == null);

            // Wrap the native thread in an object of the library
            var result = new RemoteThread(this.BattleGroundMemory, NativeThread);

            // If the thread must be started
            if (IsStarted)
            {
                result.Resume();
            }

            return result;
        }

        /// <summary>
        /// Creates a thread in the remote process and blocks the calling thread until the thread terminates.
        /// </summary>
        /// <param name="Address">
        /// A pointer to the application-defined function to be executed by the thread and represents 
        /// the starting address of the thread in the remote process.
        /// </param>
        /// <param name="Parameter">A variable to be passed to the thread function.</param>
        /// <returns>A new instance of the <see cref="RemoteThread"/> class.</returns>
        public RemoteThread CreateAndJoin(IntPtr Address, dynamic Parameter)
        {
            // Create the thread
            var ret = Create(Address, Parameter);

            // Wait the end of the thread
            ret.Join();

            // Return the thread
            return ret;
        }

        /// <summary>
        /// Creates a thread in the remote process and blocks the calling thread until the thread terminates.
        /// </summary>
        /// <param name="Address">
        /// A pointer to the application-defined function to be executed by the thread and represents 
        /// the starting address of the thread in the remote process.
        /// </param>
        /// <returns>A new instance of the <see cref="RemoteThread"/> class.</returns>
        public RemoteThread CreateAndJoin(IntPtr Address)
        {
            // Create the thread
            var ret = this.Create(Address);

            // Wait the end of the thread
            ret.Join();

            // Return the thread
            return ret;
        }

        /// <summary>
        /// Releases all resources used by the <see cref="ThreadFactory"/> object.
        /// </summary>
        public void Dispose()
        {
            // Nothing to dispose... yet
        }

        /// <summary>
        /// Gets a thread by its id in the remote process.
        /// </summary>
        /// <param name="Id">The id of the thread.</param>
        /// <returns>A new instance of the <see cref="RemoteThread"/> class.</returns>
        public RemoteThread GetThreadById(int Id)
        {
            return new RemoteThread(this.BattleGroundMemory, this.NativeThreads.First(T => T.Id == Id));
        }

        /// <summary>
        /// Resumes all threads.
        /// </summary>
        public void ResumeAll()
        {
            foreach (var thread in this.RemoteThreads)
            {
                thread.Resume();
            }
        }

        /// <summary>
        /// Suspends all threads.
        /// </summary>
        public void SuspendAll()
        {
            foreach (var thread in this.RemoteThreads)
            {
                thread.Suspend();
            }
        }
    }
}