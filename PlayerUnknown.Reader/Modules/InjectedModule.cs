namespace PlayerUnknown.Reader.Modules
{
    using System;
    using System.Diagnostics;
    using System.Linq;

    using PlayerUnknown.Reader.Internals;

    /// <summary>
    /// Class representing an injected module in a remote process.
    /// </summary>
    public class InjectedModule : RemoteModule, IDisposableState
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InjectedModule"/> class.
        /// </summary>
        /// <param name="BattleGroundMemory">The reference of the <see cref="BattleGroundMemory"/> object.</param>
        /// <param name="Module">The native <see cref="ProcessModule"/> object corresponding to the injected module.</param>
        /// <param name="MustBeDisposed">The module will be ejected when the finalizer collects the object.</param>
        internal InjectedModule(BattleGroundMemory BattleGroundMemory, ProcessModule Module, bool MustBeDisposed = true)
            : base(BattleGroundMemory, Module)
        {
            // Save the parameter
            this.MustBeDisposed = MustBeDisposed;
        }

        /// <summary>
        /// Frees resources and perform other cleanup operations before it is reclaimed by garbage collection.
        /// </summary>
        ~InjectedModule()
        {
            if (this.MustBeDisposed)
            {
                this.Dispose();
            }
        }

        /// <summary>
        /// Gets a value indicating whether the element is disposed.
        /// </summary>
        public bool IsDisposed
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets a value indicating whether the element must be disposed when the Garbage Collector collects the object.
        /// </summary>
        public bool MustBeDisposed
        {
            get;
            set;
        }

        /// <summary>
        /// Releases all resources used by the <see cref="InjectedModule"/> object.
        /// </summary>
        public virtual void Dispose()
        {
            if (!this.IsDisposed)
            {
                // Set the flag to true
                this.IsDisposed = true;

                // Eject the module
                this.BattleGroundMemory.Modules.Eject(this);

                // Avoid the finalizer 
                GC.SuppressFinalize(this);
            }
        }

        /// <summary>
        /// Injects the specified module into the address space of the remote process.
        /// </summary>
        /// <param name="BattleGroundMemory">The reference of the <see cref="BattleGroundMemory"/> object.</param>
        /// <param name="Path">The path of the module. This can be either a library module (a .dll file) or an executable module (an .exe file).</param>
        /// <returns>A new instance of the <see cref="InjectedModule"/>class.</returns>
        internal static InjectedModule InternalInject(BattleGroundMemory BattleGroundMemory, string Path)
        {
            // Call LoadLibraryA remotely
            var thread = BattleGroundMemory.Threads.CreateAndJoin(BattleGroundMemory["kernel32"]["LoadLibraryA"].BaseAddress, Path);

            // Get the inject module
            if (thread.GetExitCode<IntPtr>() != IntPtr.Zero)
            {
                return new InjectedModule(BattleGroundMemory, BattleGroundMemory.Modules.NativeModules.First(M => M.BaseAddress == thread.GetExitCode<IntPtr>()));
            }

            return null;
        }
    }
}