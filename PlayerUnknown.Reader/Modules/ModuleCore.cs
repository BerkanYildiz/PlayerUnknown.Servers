namespace PlayerUnknown.Reader.Modules
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;

    using PlayerUnknown.Reader.Native;

    /// <summary>
    /// Static core class providing tools for manipulating modules and libraries.
    /// </summary>
    public static class ModuleCore
    {
        /// <summary>
        /// Retrieves the address of an exported function or variable from the specified dynamic-link library (DLL).
        /// </summary>
        /// <param name="ModuleName">The module name (not case-sensitive).</param>
        /// <param name="FunctionName">The function or variable name, or the function's ordinal value.</param>
        /// <returns>The address of the exported function.</returns>
        public static IntPtr GetProcAddress(string ModuleName, string FunctionName)
        {
            // Get the module
            var module = Process.GetCurrentProcess().Modules.Cast<ProcessModule>().FirstOrDefault(M => M.ModuleName.ToLower() == ModuleName.ToLower());

            // Check whether there is a module loaded with this name
            if (module == null)
            {
                throw new ArgumentException(string.Format("Couldn't get the module {0} because it doesn't exist in the current process.", ModuleName));
            }

            // Get the function address
            var ret = NativeMethods.GetProcAddress(module.BaseAddress, FunctionName);

            // Check whether the function was found
            if (ret != IntPtr.Zero)
            {
                return ret;
            }

            // Else the function was not found, throws an exception
            throw new Win32Exception(string.Format("Couldn't get the function address of {0}.", FunctionName));
        }

        /// <summary>
        /// Retrieves the address of an exported function or variable from the specified dynamic-link library (DLL).
        /// </summary>
        /// <param name="Module">The <see cref="ProcessModule"/> object corresponding to the module.</param>
        /// <param name="FunctionName">The function or variable name, or the function's ordinal value.</param>
        /// <returns>If the function succeeds, the return value is the address of the exported function.</returns>
        public static IntPtr GetProcAddress(ProcessModule Module, string FunctionName)
        {
            return ModuleCore.GetProcAddress(Module.ModuleName, FunctionName);
        }

        /// <summary>
        /// Frees the loaded dynamic-link library (DLL) module and, if necessary, decrements its reference count.
        /// </summary>
        /// <param name="LibraryName">The name of the library to free (not case-sensitive).</param>
        public static void FreeLibrary(string LibraryName)
        {
            // Get the module
            var module = Process.GetCurrentProcess().Modules.Cast<ProcessModule>().FirstOrDefault(M => M.ModuleName.ToLower() == LibraryName.ToLower());

            // Check whether there is a library loaded with this name
            if (module == null)
            {
                throw new ArgumentException(string.Format("Couldn't free the library {0} because it doesn't exist in the current process.", LibraryName));
            }

            // Free the library
            if (!NativeMethods.FreeLibrary(module.BaseAddress))
            {
                throw new Win32Exception(string.Format("Couldn't free the library {0}.", LibraryName));
            }
        }

        /// <summary>
        /// Frees the loaded dynamic-link library (DLL) module and, if necessary, decrements its reference count.
        /// </summary>
        /// <param name="Module">The <see cref="ProcessModule"/> object corresponding to the library to free.</param>
        public static void FreeLibrary(ProcessModule Module)
        {
            ModuleCore.FreeLibrary(Module.ModuleName);
        }

        /// <summary>
        /// Loads the specified module into the address space of the calling process.
        /// </summary>
        /// <param name="LibraryPath">The name of the module. This can be either a library module (a .dll file) or an executable module (an .exe file).</param>
        /// <returns>A <see cref="ProcessModule"/> corresponding to the loaded library.</returns>
        public static ProcessModule LoadLibrary(string LibraryPath)
        {
            // Check whether the file exists
            if (!File.Exists(LibraryPath))
            {
                throw new FileNotFoundException(string.Format("Couldn't load the library {0} because the file doesn't exist.", LibraryPath));
            }

            // Load the library
            if (NativeMethods.LoadLibrary(LibraryPath) == IntPtr.Zero)
            {
                throw new Win32Exception(string.Format("Couldn't load the library {0}.", LibraryPath));
            }

            // Enumerate the loaded modules and return the one newly added
            return Process.GetCurrentProcess().Modules.Cast<ProcessModule>().First(M => M.FileName == LibraryPath);
        }
    }
}