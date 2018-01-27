namespace PlayerUnknown.Reader.Assembly
{
    using System;
    using System.Text;

    using PlayerUnknown.Reader.Internals;

    /// <summary>
    /// Class representing a transaction where the user can insert mnemonics.
    /// The code is then executed when the object is disposed.
    /// </summary>
    public class AssemblyTransaction : IDisposable
    {
        /// <summary>
        /// The reference of the <see cref="BattleGroundMemory"/> object.
        /// </summary>
        protected readonly BattleGroundMemory BattleGroundMemory;

        /// <summary>
        /// The builder contains all the mnemonics inserted by the user.
        /// </summary>
        protected StringBuilder Mnemonics;

        /// <summary>
        /// The exit code of the thread created to execute the assembly code.
        /// </summary>
        protected IntPtr ExitCode;

        /// <summary>
        /// Initializes a new instance of the <see cref="AssemblyTransaction"/> class.
        /// </summary>
        /// <param name="BattleGroundMemory">The reference of the <see cref="BattleGroundMemory"/> object.</param>
        /// <param name="Address">The address where the assembly code is injected.</param>
        /// <param name="AutoExecute">Indicates whether the assembly code is executed once the object is disposed.</param>
        public AssemblyTransaction(BattleGroundMemory BattleGroundMemory, IntPtr Address, bool AutoExecute)
        {
            // Save the parameters
            this.BattleGroundMemory = BattleGroundMemory;
            this.IsAutoExecuted = AutoExecute;
            this.Address = Address;

            // Initialize the string builder
            this.Mnemonics = new StringBuilder();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AssemblyTransaction"/> class.
        /// </summary>
        /// <param name="BattleGroundMemory">The reference of the <see cref="BattleGroundMemory"/> object.</param>
        /// <param name="AutoExecute">Indicates whether the assembly code is executed once the object is disposed.</param>
        public AssemblyTransaction(BattleGroundMemory BattleGroundMemory, bool AutoExecute)
            : this(BattleGroundMemory, IntPtr.Zero, AutoExecute)
        {
        }

        /// <summary>
        /// The address where to assembly code is assembled.
        /// </summary>
        public IntPtr Address
        {
            get;
        }

        /// <summary>
        /// Gets the value indicating whether the assembly code is executed once the object is disposed.
        /// </summary>
        public bool IsAutoExecuted
        {
            get;
            set;
        }

        /// <summary>
        /// Adds a mnemonic to the transaction.
        /// </summary>
        /// <param name="Asm">A composite format string.</param>
        /// <param name="Args">An object array that contains zero or more objects to format.</param>
        public void AddLine(string Asm, params object[] Args)
        {
            this.Mnemonics.AppendLine(string.Format(Asm, Args));
        }

        /// <summary>
        /// Assemble the assembly code of this transaction.
        /// </summary>
        /// <returns>An array of bytes containing the assembly code.</returns>
        public byte[] Assemble()
        {
            return this.BattleGroundMemory.Assembly.Assembler.Assemble(this.Mnemonics.ToString());
        }

        /// <summary>
        /// Removes all mnemonics from the transaction.
        /// </summary>
        public void Clear()
        {
            this.Mnemonics.Clear();
        }

        /// <summary>
        /// Releases all resources used by the <see cref="AssemblyTransaction"/> object.
        /// </summary>
        public virtual void Dispose()
        {
            // If a pointer was specified
            if (this.Address != IntPtr.Zero)
            {
                // If the assembly code must be executed
                if (this.IsAutoExecuted)
                {
                    this.ExitCode = this.BattleGroundMemory.Assembly.InjectAndExecute<IntPtr>(this.Mnemonics.ToString(), this.Address);
                }

                // Else the assembly code is just injected
                else
                {
                    this.BattleGroundMemory.Assembly.Inject(this.Mnemonics.ToString(), this.Address);
                }
            }

            // If no pointer was specified and the code assembly code must be executed
            if (this.Address == IntPtr.Zero && this.IsAutoExecuted)
            {
                this.ExitCode = this.BattleGroundMemory.Assembly.InjectAndExecute<IntPtr>(this.Mnemonics.ToString());
            }
        }

        /// <summary>
        /// Gets the termination status of the thread.
        /// </summary>
        public T GetExitCode<T>()
        {
            return MarshalType<T>.PtrToObject(this.BattleGroundMemory, this.ExitCode);
        }

        /// <summary>
        /// Inserts a mnemonic to the transaction at a given index.
        /// </summary>
        /// <param name="Index">The position in the transaction where insertion begins.</param>
        /// <param name="Asm">A composite format string.</param>
        /// <param name="Args">An object array that contains zero or more objects to format.</param>
        public void InsertLine(int Index, string Asm, params object[] Args)
        {
            this.Mnemonics.Insert(Index, string.Format(Asm, Args));
        }
    }
}