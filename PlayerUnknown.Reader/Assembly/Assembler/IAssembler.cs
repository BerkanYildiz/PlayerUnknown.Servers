namespace PlayerUnknown.Reader.Assembly.Assembler
{
    using System;

    /// <summary>
    /// Interface defining an assembler.
    /// </summary>
    public interface IAssembler
    {
        /// <summary>
        /// Assemble the specified assembly code.
        /// </summary>
        /// <param name="Asm">The assembly code.</param>
        /// <returns>An array of bytes containing the assembly code.</returns>
        byte[] Assemble(string Asm);

        /// <summary>
        /// Assemble the specified assembly code at a base address.
        /// </summary>
        /// <param name="Asm">The assembly code.</param>
        /// <param name="BaseAddress">The address where the code is rebased.</param>
        /// <returns>An array of bytes containing the assembly code.</returns>
        byte[] Assemble(string Asm, IntPtr BaseAddress);
    }
}