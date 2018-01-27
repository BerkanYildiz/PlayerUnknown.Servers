namespace PlayerUnknown.Reader.Modules
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;

    using PlayerUnknown.Reader.Memory;
    using PlayerUnknown.Reader.Native;

    /// <summary>
    /// Class repesenting a module in the remote process.
    /// </summary>
    public class RemoteModule : RemoteRegion
    {
        /// <summary>
        /// The dictionary containing all cached functions of the remote module.
        /// </summary>
        internal static readonly IDictionary<Tuple<string, SafeMemoryHandle>, RemoteFunction> CachedFunctions = new Dictionary<Tuple<string, SafeMemoryHandle>, RemoteFunction>();

        /// <summary>
        /// Initializes a new instance of the <see cref="RemoteModule"/> class.
        /// </summary>
        /// <param name="BattleGroundMemory">The reference of the <see cref="BattleGroundMemory"/> object.</param>
        /// <param name="Module">The native <see cref="ProcessModule"/> object corresponding to this module.</param>
        internal RemoteModule(BattleGroundMemory BattleGroundMemory, ProcessModule Module)
            : base(BattleGroundMemory, Module.BaseAddress)
        {
            // Save the parameter
            this.Native = Module;
        }

        /// <summary>
        /// State if this is the main module of the remote process.
        /// </summary>
        public bool IsMainModule
        {
            get
            {
                return this.BattleGroundMemory.Native.MainModule.BaseAddress == this.BaseAddress;
            }
        }

        /// <summary>
        /// Gets if the <see cref="RemoteModule"/> is valid.
        /// </summary>
        public override bool IsValid
        {
            get
            {
                return base.IsValid && this.BattleGroundMemory.Native.Modules.Cast<ProcessModule>().Any(M => M.BaseAddress == this.BaseAddress && M.ModuleName == this.Name);
            }
        }

        /// <summary>
        /// The name of the module.
        /// </summary>
        public string Name
        {
            get
            {
                return this.Native.ModuleName;
            }
        }

        /// <summary>
        /// The native <see cref="ProcessModule"/> object corresponding to this module.
        /// </summary>
        public ProcessModule Native
        {
            get;
        }

        /// <summary>
        /// The full path of the module.
        /// </summary>
        public string Path
        {
            get
            {
                return this.Native.FileName;
            }
        }

        /// <summary>
        /// The size of the module in the memory of the remote process.
        /// </summary>
        public int Size
        {
            get
            {
                return this.Native.ModuleMemorySize;
            }
        }

        /// <summary>
        /// Gets the specified function in the remote module.
        /// </summary>
        /// <param name="FunctionName">The name of the function.</param>
        /// <returns>A new instance of a <see cref="RemoteFunction"/> class.</returns>
        public RemoteFunction this[string FunctionName]
        {
            get
            {
                return this.FindFunction(FunctionName);
            }
        }

        /// <summary>
        /// Ejects the loaded dynamic-link library (DLL) module.
        /// </summary>
        public void Eject()
        {
            // Eject the module
            this.BattleGroundMemory.Modules.Eject(this);

            // Remove the pointer
            this.BaseAddress = IntPtr.Zero;
        }

        /// <summary>
        /// Finds the specified function in the remote module.
        /// </summary>
        /// <param name="FunctionName">The name of the function (case sensitive).</param>
        /// <returns>A new instance of a <see cref="RemoteFunction"/> class.</returns>
        /// <remarks>
        /// Interesting article on how DLL loading works: http://msdn.microsoft.com/en-us/magazine/bb985014.aspx
        /// </remarks>
        public RemoteFunction FindFunction(string FunctionName)
        {
            // Create the tuple
            var tuple = Tuple.Create(FunctionName, this.BattleGroundMemory.Handle);

            // Check if the function is already cached
            if (RemoteModule.CachedFunctions.ContainsKey(tuple))
            {
                return RemoteModule.CachedFunctions[tuple];
            }

            // If the function is not cached
            // Check if the local process has this module loaded
            var LocalModule = Process.GetCurrentProcess().Modules.Cast<ProcessModule>().FirstOrDefault(M => M.FileName.ToLower() == this.Path.ToLower());
            var IsManuallyLoaded = false;
            try
            {
                // If this is not the case, load the module inside the local process
                if (LocalModule == null)
                {
                    IsManuallyLoaded = true;
                    LocalModule = ModuleCore.LoadLibrary(this.Native.FileName);
                }

                // Get the offset of the function
                var offset = ModuleCore.GetProcAddress(LocalModule, FunctionName).ToInt64() - LocalModule.BaseAddress.ToInt64();

                // Rebase the function with the remote module
                var function = new RemoteFunction(this.BattleGroundMemory, new IntPtr(this.Native.BaseAddress.ToInt64() + offset), FunctionName);

                // Store the function in the cache
                RemoteModule.CachedFunctions.Add(tuple, function);

                // Return the function rebased with the remote module
                return function;
            }
            finally
            {
                // Free the module if it was manually loaded
                if (IsManuallyLoaded)
                {
                    ModuleCore.FreeLibrary(LocalModule);
                }
            }
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        public override string ToString()
        {
            return string.Format("BaseAddress = 0x{0:X} Name = {1}", this.BaseAddress.ToInt64(), this.Name);
        }

        /// <summary>
        /// Frees the loaded dynamic-link library (DLL) module and, if necessary, decrements its reference count.
        /// </summary>
        /// <param name="BattleGroundMemory">The reference of the <see cref="BattleGroundMemory"/> object.</param>
        /// <param name="Module">The module to eject.</param>
        internal static void InternalEject(BattleGroundMemory BattleGroundMemory, RemoteModule Module)
        {
            // Call FreeLibrary remotely
            BattleGroundMemory.Threads.CreateAndJoin(BattleGroundMemory["kernel32"]["FreeLibrary"].BaseAddress, Module.BaseAddress);
        }
    }
}