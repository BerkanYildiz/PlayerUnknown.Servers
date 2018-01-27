namespace PlayerUnknown.Reader.Assembly.Assembler
{
    using System;

    using Binarysharp.Assemblers.Fasm;

    /// <summary>
    /// Implement Fasm.NET compiler for 32-bit development.
    /// More info: https://github.com/ZenLulz/Fasm.NET
    /// </summary>
    public class Fasm32Assembler : IAssembler
    {
        /// <summary>
        /// Assemble the specified assembly code.
        /// </summary>
        /// <param name="Asm">The assembly code.</param>
        /// <returns>An array of bytes containing the assembly code.</returns>
        public byte[] Assemble(string Asm)
        {
            // Assemble and return the code
            return this.Assemble(Asm, IntPtr.Zero);
        }

        /// <summary>
        /// Assemble the specified assembly code at a base address.
        /// </summary>
        /// <param name="Asm">The assembly code.</param>
        /// <param name="BaseAddress">The address where the code is rebased.</param>
        /// <returns>An array of bytes containing the assembly code.</returns>
        public byte[] Assemble(string Asm, IntPtr BaseAddress)
        {
            // Rebase the code
            Asm = string.Format("use32\norg 0x{0:X8}\n", BaseAddress.ToInt64()) + Asm;

            // Assemble and return the code
            return FasmNet.Assemble(Asm);
        }
    }
}