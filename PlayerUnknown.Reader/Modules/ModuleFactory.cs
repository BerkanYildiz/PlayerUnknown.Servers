namespace PlayerUnknown.Reader.Modules
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;

    using PlayerUnknown.Reader.Internals;
    using PlayerUnknown.Reader.Memory;

    /// <summary>
    /// Class providing tools for manipulating modules and libraries.
    /// </summary>
    public class ModuleFactory : IFactory
    {
        /// <summary>
        /// The reference of the <see cref="BattleGroundMemory"/> object.
        /// </summary>
        protected readonly BattleGroundMemory BattleGroundMemory;

        /// <summary>
        /// The list containing all injected modules (writable).
        /// </summary>
        protected readonly List<InjectedModule> InternalInjectedModules;

        /// <summary>
        /// Initializes a new instance of the <see cref="ModuleFactory"/> class.
        /// </summary>
        /// <param name="BattleGroundMemory">The reference of the <see cref="BattleGroundMemory"/> object.</param>
        internal ModuleFactory(BattleGroundMemory BattleGroundMemory)
        {
            // Save the parameter
            this.BattleGroundMemory = BattleGroundMemory;

            // Create a list containing all injected modules
            this.InternalInjectedModules = new List<InjectedModule>();

            // Save a reference of the main module (the main module is required for a lot of operations, cached for speed reasons)
            this.MainModule = this.FetchModule(this.BattleGroundMemory.Native.MainModule);
        }

        /// <summary>
        /// Frees resources and perform other cleanup operations before it is reclaimed by garbage collection.
        /// </summary>
        ~ModuleFactory()
        {
            this.Dispose();
        }

        /// <summary>
        /// A collection containing all injected modules.
        /// </summary>
        public IEnumerable<InjectedModule> InjectedModules
        {
            get
            {
                return this.InternalInjectedModules.AsReadOnly();
            }
        }

        /// <summary>
        /// Gets the main module for the remote process.
        /// </summary>
        public RemoteModule MainModule
        {
            get;
        }

        /// <summary>
        /// Gets the modules that have been loaded in the remote process.
        /// </summary>
        public IEnumerable<RemoteModule> RemoteModules
        {
            get
            {
                // Yield managed modules for ones contained in the target process
                return this.NativeModules.Select(this.FetchModule);
            }
        }

        /// <summary>
        /// Gets the native modules that have been loaded in the remote process.
        /// </summary>
        internal IEnumerable<ProcessModule> NativeModules
        {
            get
            {
                return this.BattleGroundMemory.Native.Modules.Cast<ProcessModule>();
            }
        }

        /// <summary>
        /// Gets a pointer from the remote process.
        /// </summary>
        /// <param name="Address">The address of the pointer.</param>
        /// <returns>A new instance of a <see cref="RemotePointer"/> class.</returns>
        public RemotePointer this[IntPtr Address]
        {
            get
            {
                return new RemotePointer(this.BattleGroundMemory, Address);
            }
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
                return this.FetchModule(ModuleName);
            }
        }

        /// <summary>
        /// Releases all resources used by the <see cref="ModuleFactory"/> object.
        /// </summary>
        public virtual void Dispose()
        {
            // Release all injected modules which must be disposed
            foreach (var InjectedModule in this.InternalInjectedModules.Where(M => M.MustBeDisposed))
            {
                InjectedModule.Dispose();
            }

            // Clean the cached functions related to this process
            foreach (var CachedFunction in RemoteModule.CachedFunctions.ToArray())
            {
                if (CachedFunction.Key.Item2 == this.BattleGroundMemory.Handle)
                {
                    RemoteModule.CachedFunctions.Remove(CachedFunction);
                }
            }

            // Avoid the finalizer
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Frees the loaded dynamic-link library (DLL) module and, if necessary, decrements its reference count.
        /// </summary>
        /// <param name="Module">The module to eject.</param>
        public void Eject(RemoteModule Module)
        {
            // If the module is valid
            if (!Module.IsValid)
            {
                return;
            }

            // Find if the module is an injected one
            var injected = this.InternalInjectedModules.FirstOrDefault(M => M.Equals(Module));
            if (injected != null)
            {
                this.InternalInjectedModules.Remove(injected);
            }

            // Eject the module
            RemoteModule.InternalEject(this.BattleGroundMemory, Module);
        }

        /// <summary>
        /// Frees the loaded dynamic-link library (DLL) module and, if necessary, decrements its reference count.
        /// </summary>
        /// <param name="ModuleName">The name of module to eject.</param>
        public void Eject(string ModuleName)
        {
            // Fint the module to eject
            var module = this.RemoteModules.FirstOrDefault(M => M.Name == ModuleName);

            // Eject the module is it's valid
            if (module != null)
            {
                RemoteModule.InternalEject(this.BattleGroundMemory, module);
            }
        }

        /// <summary>
        /// Injects the specified module into the address space of the remote process.
        /// </summary>
        /// <param name="Path">The path of the module. This can be either a library module (a .dll file) or an executable module (an .exe file).</param>
        /// <param name="MustBeDisposed">The module will be ejected when the finalizer collects the object.</param>
        /// <returns>A new instance of the <see cref="InjectedModule"/>class.</returns>
        public InjectedModule Inject(string Path, bool MustBeDisposed = true)
        {
            // Injects the module
            var module = InjectedModule.InternalInject(this.BattleGroundMemory, Path);

            // Add the module in the list
            this.InternalInjectedModules.Add(module);

            // Return the module
            return module;
        }

        /// <summary>
        /// Fetches a module from the remote process.
        /// </summary>
        /// <param name="ModuleName">A module name (not case sensitive). If the file name extension is omitted, the default library extension .dll is appended.</param>
        /// <returns>A new instance of a <see cref="RemoteModule"/> class.</returns>
        protected RemoteModule FetchModule(string ModuleName)
        {
            // Convert module name with lower chars
            ModuleName = ModuleName.ToLower();

            // Check if the module name has an extension
            if (!Path.HasExtension(ModuleName))
            {
                ModuleName += ".dll";
            }

            // Fetch and return the module
            return new RemoteModule(this.BattleGroundMemory, this.NativeModules.First(M => M.ModuleName.ToLower() == ModuleName));
        }

        /// <summary>
        /// Fetches a module from the remote process.
        /// </summary>
        /// <param name="Module">A module in the remote process.</param>
        /// <returns>A new instance of a <see cref="RemoteModule"/> class.</returns>
        private RemoteModule FetchModule(ProcessModule Module)
        {
            return this.FetchModule(Module.ModuleName);
        }
    }
}