namespace PlayerUnknown.Reader.Threading
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    using PlayerUnknown.Reader.Internals;
    using PlayerUnknown.Reader.Native;

    using ThreadState = System.Diagnostics.ThreadState;

    /// <summary>
    /// Class repesenting a thread in the remote process.
    /// </summary>
    public class RemoteThread : IDisposable, IEquatable<RemoteThread>
    {
        /// <summary>
        /// The reference of the <see cref="BattleGroundMemory"/> object.
        /// </summary>
        protected readonly BattleGroundMemory BattleGroundMemory;

        /// <summary>
        /// The parameter passed to the thread when it was created.
        /// </summary>
        private readonly IMarshalledValue Parameter;

        /// <summary>
        /// The task involved in cleaning the parameter memory when the <see cref="RemoteThread"/> object is collected.
        /// </summary>
        private readonly Task ParameterCleaner;

        /// <summary>
        /// Initializes a new instance of the <see cref="RemoteThread"/> class.
        /// </summary>
        /// <param name="BattleGroundMemory">The reference of the <see cref="BattleGroundMemory"/> object.</param>
        /// <param name="Thread">The native <see cref="ProcessThread"/> object.</param>
        internal RemoteThread(BattleGroundMemory BattleGroundMemory, ProcessThread Thread)
        {
            // Save the parameters
            this.BattleGroundMemory = BattleGroundMemory;
            this.Native = Thread;

            // Save the thread id
            this.Id = Thread.Id;

            // Open the thread
            this.Handle = ThreadCore.OpenThread(ThreadAccessFlags.AllAccess, this.Id);

            // Initialize the TEB
            this.Teb = new ManagedTeb(this.BattleGroundMemory, ManagedTeb.FindTeb(this.Handle));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RemoteThread"/> class.
        /// </summary>
        /// <param name="BattleGroundMemory">The reference of the <see cref="BattleGroundMemory"/> object.</param>
        /// <param name="Thread">The native <see cref="ProcessThread"/> object.</param>
        /// <param name="Parameter">The parameter passed to the thread when it was created.</param>
        internal RemoteThread(BattleGroundMemory BattleGroundMemory, ProcessThread Thread, IMarshalledValue Parameter = null)
            : this(BattleGroundMemory, Thread)
        {
            // Save the parameter
            this.Parameter = Parameter;

            // Create the task
            this.ParameterCleaner = new Task(
                () =>
                    {
                        this.Join();
                        this.Parameter.Dispose();
                    });
        }

        /// <summary>
        /// Frees resources and perform other cleanup operations before it is reclaimed by garbage collection. 
        /// </summary>
        ~RemoteThread()
        {
            this.Dispose();
        }

        /// <summary>
        /// Gets or sets the full context of the thread.
        /// If the thread is not already suspended, performs a <see cref="Suspend"/> and <see cref="Resume"/> call on the thread.
        /// </summary>
        public ThreadContext Context
        {
            get
            {
                // Check if the thread is alive
                if (this.IsAlive)
                {
                    // Check if the thread is already suspended
                    var IsSuspended = this.IsSuspended;
                    try
                    {
                        // Suspend the thread if it wasn't
                        if (!IsSuspended)
                        {
                            this.Suspend();
                        }

                        // Get the context
                        return ThreadCore.GetThreadContext(this.Handle, ThreadContextFlags.All | ThreadContextFlags.FloatingPoint | ThreadContextFlags.DebugRegisters | ThreadContextFlags.ExtendedRegisters);
                    }
                    finally
                    {
                        // Resume the thread if it wasn't suspended
                        if (!IsSuspended)
                        {
                            this.Resume();
                        }
                    }
                }

                // The thread is closed, cannot set the context
                throw new ThreadStateException(string.Format("Couldn't set the context of the thread #{0} because it is terminated.", this.Id));
            }

            set
            {
                // Check if the thread is alive
                if (!this.IsAlive)
                {
                    return;
                }

                // Check if the thread is already suspended
                var IsSuspended = this.IsSuspended;
                try
                {
                    // Suspend the thread if it wasn't
                    if (!IsSuspended)
                    {
                        this.Suspend();
                    }

                    // Set the context
                    ThreadCore.SetThreadContext(this.Handle, value);
                }
                finally
                {
                    // Resume the thread if it wasn't suspended
                    if (!IsSuspended)
                    {
                        this.Resume();
                    }
                }
            }
        }

        /// <summary>
        /// The remote thread handle opened with all rights.
        /// </summary>
        public SafeMemoryHandle Handle
        {
            get;
        }

        /// <summary>
        /// Gets the unique identifier of the thread.
        /// </summary>
        public int Id
        {
            get;
        }

        /// <summary>
        /// Gets if the thread is alive.
        /// </summary>
        public bool IsAlive
        {
            get
            {
                return !this.IsTerminated;
            }
        }

        /// <summary>
        /// Gets if the thread is the main one in the remote process.
        /// </summary>
        public bool IsMainThread
        {
            get
            {
                return this == this.BattleGroundMemory.Threads.MainThread;
            }
        }

        /// <summary>
        /// Gets if the thread is suspended.
        /// </summary>
        public bool IsSuspended
        {
            get
            {
                // Refresh the thread info
                this.Refresh();

                // Return if the thread is suspended
                return this.Native != null && this.Native.ThreadState == ThreadState.Wait && this.Native.WaitReason == ThreadWaitReason.Suspended;
            }
        }

        /// <summary>
        /// Gets if the thread is terminated.
        /// </summary>
        public bool IsTerminated
        {
            get
            {
                // Refresh the thread info
                this.Refresh();

                // Check if the thread is terminated
                return this.Native == null;
            }
        }

        /// <summary>
        /// The native <see cref="ProcessThread"/> object corresponding to this thread.
        /// </summary>
        public ProcessThread Native
        {
            get;
            private set;
        }

        /// <summary>
        /// The Thread Environment Block of the thread.
        /// </summary>
        public ManagedTeb Teb
        {
            get;
        }

        /// <summary>
        /// Releases all resources used by the <see cref="RemoteThread"/> object.
        /// </summary>
        public virtual void Dispose()
        {
            // Close the thread handle
            this.Handle.Close();

            // Avoid the finalizer
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

            return Obj.GetType() == this.GetType() && this.Equals((RemoteThread)Obj);
        }

        /// <summary>
        /// Returns a value indicating whether this instance is equal to a specified object.
        /// </summary>
        public bool Equals(RemoteThread Other)
        {
            if (object.ReferenceEquals(null, Other))
            {
                return false;
            }

            return object.ReferenceEquals(this, Other) || this.Id == Other.Id && this.BattleGroundMemory.Equals(Other.BattleGroundMemory);
        }

        /// <summary>
        /// Gets the termination status of the thread.
        /// </summary>
        public T GetExitCode<T>()
        {
            // Get the exit code of the thread (can be nullable)
            var ret = ThreadCore.GetExitCodeThread(this.Handle);

            // Return the exit code or the default value of T if there's no exit code
            return ret.HasValue ? MarshalType<T>.PtrToObject(this.BattleGroundMemory, ret.Value) : default(T);
        }

        /// <summary>
        /// Serves as a hash function for a particular type. 
        /// </summary>
        public override int GetHashCode()
        {
            return this.Id.GetHashCode() ^ this.BattleGroundMemory.GetHashCode();
        }

        /// <summary>
        /// Gets the linear address of a specified segment.
        /// </summary>
        /// <param name="Segment">The segment to get.</param>
        /// <returns>A <see cref="IntPtr"/> pointer corresponding to the linear address of the segment.</returns>
        public IntPtr GetRealSegmentAddress(SegmentRegisters Segment)
        {
            // Get a selector entry for the segment
            LdtEntry entry;
            switch (Segment)
            {
                case SegmentRegisters.Cs:
                    entry = ThreadCore.GetThreadSelectorEntry(this.Handle, this.Context.SegCs);
                    break;
                case SegmentRegisters.Ds:
                    entry = ThreadCore.GetThreadSelectorEntry(this.Handle, this.Context.SegDs);
                    break;
                case SegmentRegisters.Es:
                    entry = ThreadCore.GetThreadSelectorEntry(this.Handle, this.Context.SegEs);
                    break;
                case SegmentRegisters.Fs:
                    entry = ThreadCore.GetThreadSelectorEntry(this.Handle, this.Context.SegFs);
                    break;
                case SegmentRegisters.Gs:
                    entry = ThreadCore.GetThreadSelectorEntry(this.Handle, this.Context.SegGs);
                    break;
                case SegmentRegisters.Ss:
                    entry = ThreadCore.GetThreadSelectorEntry(this.Handle, this.Context.SegSs);
                    break;
                default:
                    throw new InvalidEnumArgumentException("segment");
            }

            // Compute the linear address
            return new IntPtr(entry.BaseLow | (entry.BaseMid << 16) | (entry.BaseHi << 24));
        }

        public static bool operator ==(RemoteThread Left, RemoteThread Right)
        {
            return object.Equals(Left, Right);
        }

        public static bool operator !=(RemoteThread Left, RemoteThread Right)
        {
            return !object.Equals(Left, Right);
        }

        /// <summary>
        /// Discards any information about this thread that has been cached inside the process component.
        /// </summary>
        public void Refresh()
        {
            if (this.Native == null)
            {
                return;
            }

            // Refresh the process info
            this.BattleGroundMemory.Native.Refresh();

            // Get new info about the thread
            this.Native = this.BattleGroundMemory.Threads.NativeThreads.FirstOrDefault(T => T.Id == this.Native.Id);
        }

        /// <summary>
        /// Blocks the calling thread until the thread terminates.
        /// </summary>
        public void Join()
        {
            ThreadCore.WaitForSingleObject(this.Handle);
        }

        /// <summary>
        /// Blocks the calling thread until a thread terminates or the specified time elapses.
        /// </summary>
        /// <param name="Time">The timeout.</param>
        /// <returns>The return value is a flag that indicates if the thread terminated or if the time elapsed.</returns>
        public WaitValues Join(TimeSpan Time)
        {
            return ThreadCore.WaitForSingleObject(this.Handle, Time);
        }

        /// <summary>
        /// Resumes a thread that has been suspended.
        /// </summary>
        public void Resume()
        {
            // Check if the thread is still alive
            if (!this.IsAlive)
            {
                return;
            }

            // Start the thread
            ThreadCore.ResumeThread(this.Handle);

            // Start a task to clean the memory used by the parameter if we created the thread
            if (this.Parameter != null && !this.ParameterCleaner.IsCompleted)
            {
                this.ParameterCleaner.Start();
            }
        }

        /// <summary>
        /// Either suspends the thread, or if the thread is already suspended, has no effect.
        /// </summary>
        /// <returns>A new instance of the <see cref="FrozenThread"/> class. If this object is disposed, the thread is resumed.</returns>
        public FrozenThread Suspend()
        {
            if (this.IsAlive)
            {
                ThreadCore.SuspendThread(this.Handle);
                return new FrozenThread(this);
            }

            return null;
        }

        /// <summary>
        /// Terminates the thread.
        /// </summary>
        /// <param name="ExitCode">The exit code of the thread to close.</param>
        public void Terminate(int ExitCode = 0)
        {
            if (this.IsAlive)
            {
                ThreadCore.TerminateThread(this.Handle, ExitCode);
            }
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        public override string ToString()
        {
            return string.Format("Id = {0} IsAlive = {1} IsMainThread = {2}", this.Id, this.IsAlive, this.IsMainThread);
        }
    }
}