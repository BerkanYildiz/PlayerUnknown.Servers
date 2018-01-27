namespace PlayerUnknown.Reader.Assembly
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using PlayerUnknown.Reader.Assembly.Assembler;
    using PlayerUnknown.Reader.Assembly.CallingConvention;
    using PlayerUnknown.Reader.Internals;
    using PlayerUnknown.Reader.Memory;
    using PlayerUnknown.Reader.Threading;

    /// <summary>
    /// Class providing tools for manipulating assembly code.
    /// </summary>
    public class AssemblyFactory : IFactory
    {
        /// <summary>
        /// The reference of the <see cref="BattleGroundMemory"/> object.
        /// </summary>
        protected readonly BattleGroundMemory BattleGroundMemory;

        /// <summary>
        /// Initializes a new instance of the <see cref="AssemblyFactory"/> class.
        /// </summary>
        /// <param name="BattleGroundMemory">The reference of the <see cref="BattleGroundMemory"/> object.</param>
        internal AssemblyFactory(BattleGroundMemory BattleGroundMemory)
        {
            // Save the parameter
            this.BattleGroundMemory = BattleGroundMemory;

            // Create the tool
            this.Assembler = new Fasm32Assembler();
        }

        /// <summary>
        /// The assembler used by the factory.
        /// </summary>
        public IAssembler Assembler
        {
            get;
            set;
        }

        /// <summary>
        /// Begins a new transaction to inject and execute assembly code into the process at the specified address.
        /// </summary>
        /// <param name="Address">The address where the assembly code is injected.</param>
        /// <param name="AutoExecute">Indicates whether the assembly code is executed once the object is disposed.</param>
        /// <returns>The return value is a new transaction.</returns>
        public AssemblyTransaction BeginTransaction(IntPtr Address, bool AutoExecute = true)
        {
            return new AssemblyTransaction(this.BattleGroundMemory, Address, AutoExecute);
        }

        /// <summary>
        /// Begins a new transaction to inject and execute assembly code into the process.
        /// </summary>
        /// <param name="AutoExecute">Indicates whether the assembly code is executed once the object is disposed.</param>
        /// <returns>The return value is a new transaction.</returns>
        public AssemblyTransaction BeginTransaction(bool AutoExecute = true)
        {
            return new AssemblyTransaction(this.BattleGroundMemory, AutoExecute);
        }

        /// <summary>
        /// Releases all resources used by the <see cref="AssemblyFactory"/> object.
        /// </summary>
        public void Dispose()
        {
            // Nothing to dispose... yet
        }

        /// <summary>
        /// Executes the assembly code located in the remote process at the specified address.
        /// </summary>
        /// <param name="Address">The address where the assembly code is located.</param>
        /// <returns>The return value is the exit code of the thread created to execute the assembly code.</returns>
        public T Execute<T>(IntPtr Address)
        {
            // Execute and join the code in a new thread
            var thread = this.BattleGroundMemory.Threads.CreateAndJoin(Address);

            // Return the exit code of the thread
            return thread.GetExitCode<T>();
        }

        /// <summary>
        /// Executes the assembly code located in the remote process at the specified address.
        /// </summary>
        /// <param name="Address">The address where the assembly code is located.</param>
        /// <returns>The return value is the exit code of the thread created to execute the assembly code.</returns>
        public IntPtr Execute(IntPtr Address)
        {
            return this.Execute<IntPtr>(Address);
        }

        /// <summary>
        /// Executes the assembly code located in the remote process at the specified address.
        /// </summary>
        /// <param name="Address">The address where the assembly code is located.</param>
        /// <param name="Parameter">The parameter used to execute the assembly code.</param>
        /// <returns>The return value is the exit code of the thread created to execute the assembly code.</returns>
        public T Execute<T>(IntPtr Address, dynamic Parameter)
        {
            // Execute and join the code in a new thread
            RemoteThread thread = this.BattleGroundMemory.Threads.CreateAndJoin(Address, Parameter);

            // Return the exit code of the thread
            return thread.GetExitCode<T>();
        }

        /// <summary>
        /// Executes the assembly code located in the remote process at the specified address.
        /// </summary>
        /// <param name="Address">The address where the assembly code is located.</param>
        /// <param name="Parameter">The parameter used to execute the assembly code.</param>
        /// <returns>The return value is the exit code of the thread created to execute the assembly code.</returns>
        public IntPtr Execute(IntPtr Address, dynamic Parameter)
        {
            return Execute<IntPtr>(Address, Parameter);
        }

        /// <summary>
        /// Executes the assembly code located in the remote process at the specified address.
        /// </summary>
        /// <param name="Address">The address where the assembly code is located.</param>
        /// <param name="CallingConvention">The calling convention used to execute the assembly code with the parameters.</param>
        /// <param name="Parameters">An array of parameters used to execute the assembly code.</param>
        /// <returns>The return value is the exit code of the thread created to execute the assembly code.</returns>
        public T Execute<T>(IntPtr Address, CallingConventions CallingConvention, params dynamic[] Parameters)
        {
            // Marshal the parameters
            var MarshalledParameters = Parameters.Select(P => MarshalValue.Marshal(this.BattleGroundMemory, P)).Cast<IMarshalledValue>().ToArray();

            // Start a transaction
            AssemblyTransaction t;
            using (t = this.BeginTransaction())
            {
                // Get the object dedicated to create mnemonics for the given calling convention
                var calling = CallingConventionSelector.Get(CallingConvention);

                // Push the parameters
                t.AddLine(calling.FormatParameters(MarshalledParameters.Select(P => P.Reference).ToArray()));

                // Call the function
                t.AddLine(calling.FormatCalling(Address));

                // Clean the parameters
                if (calling.Cleanup == CleanupTypes.Caller)
                {
                    t.AddLine(calling.FormatCleaning(MarshalledParameters.Length));
                }

                // Add the return mnemonic
                t.AddLine("retn");
            }

            // Clean the marshalled parameters
            foreach (var parameter in MarshalledParameters)
            {
                parameter.Dispose();
            }

            // Return the exit code
            return t.GetExitCode<T>();
        }

        /// <summary>
        /// Executes the assembly code located in the remote process at the specified address.
        /// </summary>
        /// <param name="Address">The address where the assembly code is located.</param>
        /// <param name="CallingConvention">The calling convention used to execute the assembly code with the parameters.</param>
        /// <param name="Parameters">An array of parameters used to execute the assembly code.</param>
        /// <returns>The return value is the exit code of the thread created to execute the assembly code.</returns>
        public IntPtr Execute(IntPtr Address, CallingConventions CallingConvention, params dynamic[] Parameters)
        {
            return this.Execute<IntPtr>(Address, CallingConvention, Parameters);
        }

        /// <summary>
        /// Executes asynchronously the assembly code located in the remote process at the specified address.
        /// </summary>
        /// <param name="Address">The address where the assembly code is located.</param>
        /// <returns>The return value is an asynchronous operation that return the exit code of the thread created to execute the assembly code.</returns>
        public Task<T> ExecuteAsync<T>(IntPtr Address)
        {
            return Task.Run(() => this.Execute<T>(Address));
        }

        /// <summary>
        /// Executes asynchronously the assembly code located in the remote process at the specified address.
        /// </summary>
        /// <param name="Address">The address where the assembly code is located.</param>
        /// <returns>The return value is an asynchronous operation that return the exit code of the thread created to execute the assembly code.</returns>
        public Task<IntPtr> ExecuteAsync(IntPtr Address)
        {
            return this.ExecuteAsync<IntPtr>(Address);
        }

        /// <summary>
        /// Executes asynchronously the assembly code located in the remote process at the specified address.
        /// </summary>
        /// <param name="Address">The address where the assembly code is located.</param>
        /// <param name="Parameter">The parameter used to execute the assembly code.</param>
        /// <returns>The return value is an asynchronous operation that return the exit code of the thread created to execute the assembly code.</returns>
        public Task<T> ExecuteAsync<T>(IntPtr Address, dynamic Parameter)
        {
            return Task.Run(() => (Task<T>)Execute<T>(Address, Parameter));
        }

        /// <summary>
        /// Executes asynchronously the assembly code located in the remote process at the specified address.
        /// </summary>
        /// <param name="Address">The address where the assembly code is located.</param>
        /// <param name="Parameter">The parameter used to execute the assembly code.</param>
        /// <returns>The return value is an asynchronous operation that return the exit code of the thread created to execute the assembly code.</returns>
        public Task<IntPtr> ExecuteAsync(IntPtr Address, dynamic Parameter)
        {
            return ExecuteAsync<IntPtr>(Address, Parameter);
        }

        /// <summary>
        /// Executes asynchronously the assembly code located in the remote process at the specified address.
        /// </summary>
        /// <param name="Address">The address where the assembly code is located.</param>
        /// <param name="CallingConvention">The calling convention used to execute the assembly code with the parameters.</param>
        /// <param name="Parameters">An array of parameters used to execute the assembly code.</param>
        /// <returns>The return value is an asynchronous operation that return the exit code of the thread created to execute the assembly code.</returns>
        public Task<T> ExecuteAsync<T>(IntPtr Address, CallingConventions CallingConvention, params dynamic[] Parameters)
        {
            return Task.Run(() => this.Execute<T>(Address, CallingConvention, Parameters));
        }

        /// <summary>
        /// Executes asynchronously the assembly code located in the remote process at the specified address.
        /// </summary>
        /// <param name="Address">The address where the assembly code is located.</param>
        /// <param name="CallingConvention">The calling convention used to execute the assembly code with the parameters.</param>
        /// <param name="Parameters">An array of parameters used to execute the assembly code.</param>
        /// <returns>The return value is an asynchronous operation that return the exit code of the thread created to execute the assembly code.</returns>
        public Task<IntPtr> ExecuteAsync(IntPtr Address, CallingConventions CallingConvention, params dynamic[] Parameters)
        {
            return this.ExecuteAsync<IntPtr>(Address, CallingConvention, Parameters);
        }

        /// <summary>
        /// Assembles mnemonics and injects the corresponding assembly code into the remote process at the specified address.
        /// </summary>
        /// <param name="Asm">The mnemonics to inject.</param>
        /// <param name="Address">The address where the assembly code is injected.</param>
        public void Inject(string Asm, IntPtr Address)
        {
            this.BattleGroundMemory.Write(Address, this.Assembler.Assemble(Asm, Address), false);
        }

        /// <summary>
        /// Assembles mnemonics and injects the corresponding assembly code into the remote process at the specified address.
        /// </summary>
        /// <param name="Asm">An array containing the mnemonics to inject.</param>
        /// <param name="Address">The address where the assembly code is injected.</param>
        public void Inject(string[] Asm, IntPtr Address)
        {
            this.Inject(string.Join("\n", Asm), Address);
        }

        /// <summary>
        /// Assembles mnemonics and injects the corresponding assembly code into the remote process.
        /// </summary>
        /// <param name="Asm">The mnemonics to inject.</param>
        /// <returns>The address where the assembly code is injected.</returns>
        public RemoteAllocation Inject(string Asm)
        {
            // Assemble the assembly code
            var code = this.Assembler.Assemble(Asm);

            // Allocate a chunk of memory to store the assembly code
            var memory = this.BattleGroundMemory.Memory.Allocate(code.Length);

            // Inject the code
            this.Inject(Asm, memory.BaseAddress);

            // Return the memory allocated
            return memory;
        }

        /// <summary>
        /// Assembles mnemonics and injects the corresponding assembly code into the remote process.
        /// </summary>
        /// <param name="Asm">An array containing the mnemonics to inject.</param>
        /// <returns>The address where the assembly code is injected.</returns>
        public RemoteAllocation Inject(string[] Asm)
        {
            return this.Inject(string.Join("\n", Asm));
        }

        /// <summary>
        /// Assembles, injects and executes the mnemonics into the remote process at the specified address.
        /// </summary>
        /// <param name="Asm">The mnemonics to inject.</param>
        /// <param name="Address">The address where the assembly code is injected.</param>
        /// <returns>The return value is the exit code of the thread created to execute the assembly code.</returns>
        public T InjectAndExecute<T>(string Asm, IntPtr Address)
        {
            // Inject the assembly code
            this.Inject(Asm, Address);

            // Execute the code
            return this.Execute<T>(Address);
        }

        /// <summary>
        /// Assembles, injects and executes the mnemonics into the remote process at the specified address.
        /// </summary>
        /// <param name="Asm">The mnemonics to inject.</param>
        /// <param name="Address">The address where the assembly code is injected.</param>
        /// <returns>The return value is the exit code of the thread created to execute the assembly code.</returns>
        public IntPtr InjectAndExecute(string Asm, IntPtr Address)
        {
            return this.InjectAndExecute<IntPtr>(Asm, Address);
        }

        /// <summary>
        /// Assembles, injects and executes the mnemonics into the remote process at the specified address.
        /// </summary>
        /// <param name="Asm">An array containing the mnemonics to inject.</param>
        /// <param name="Address">The address where the assembly code is injected.</param>
        /// <returns>The return value is the exit code of the thread created to execute the assembly code.</returns>
        public T InjectAndExecute<T>(string[] Asm, IntPtr Address)
        {
            return this.InjectAndExecute<T>(string.Join("\n", Asm), Address);
        }

        /// <summary>
        /// Assembles, injects and executes the mnemonics into the remote process at the specified address.
        /// </summary>
        /// <param name="Asm">An array containing the mnemonics to inject.</param>
        /// <param name="Address">The address where the assembly code is injected.</param>
        /// <returns>The return value is the exit code of the thread created to execute the assembly code.</returns>
        public IntPtr InjectAndExecute(string[] Asm, IntPtr Address)
        {
            return this.InjectAndExecute<IntPtr>(Asm, Address);
        }

        /// <summary>
        /// Assembles, injects and executes the mnemonics into the remote process.
        /// </summary>
        /// <param name="Asm">The mnemonics to inject.</param>
        /// <returns>The return value is the exit code of the thread created to execute the assembly code.</returns>
        public T InjectAndExecute<T>(string Asm)
        {
            // Inject the assembly code
            using (var memory = this.Inject(Asm))
            {
                // Execute the code
                return this.Execute<T>(memory.BaseAddress);
            }
        }

        /// <summary>
        /// Assembles, injects and executes the mnemonics into the remote process.
        /// </summary>
        /// <param name="Asm">The mnemonics to inject.</param>
        /// <returns>The return value is the exit code of the thread created to execute the assembly code.</returns>
        public IntPtr InjectAndExecute(string Asm)
        {
            return this.InjectAndExecute<IntPtr>(Asm);
        }

        /// <summary>
        /// Assembles, injects and executes the mnemonics into the remote process.
        /// </summary>
        /// <param name="Asm">An array containing the mnemonics to inject.</param>
        /// <returns>The return value is the exit code of the thread created to execute the assembly code.</returns>
        public T InjectAndExecute<T>(string[] Asm)
        {
            return this.InjectAndExecute<T>(string.Join("\n", Asm));
        }

        /// <summary>
        /// Assembles, injects and executes the mnemonics into the remote process.
        /// </summary>
        /// <param name="Asm">An array containing the mnemonics to inject.</param>
        /// <returns>The return value is the exit code of the thread created to execute the assembly code.</returns>
        public IntPtr InjectAndExecute(string[] Asm)
        {
            return this.InjectAndExecute<IntPtr>(Asm);
        }

        /// <summary>
        /// Assembles, injects and executes asynchronously the mnemonics into the remote process at the specified address.
        /// </summary>
        /// <param name="Asm">The mnemonics to inject.</param>
        /// <param name="Address">The address where the assembly code is injected.</param>
        /// <returns>The return value is an asynchronous operation that return the exit code of the thread created to execute the assembly code.</returns>
        public Task<T> InjectAndExecuteAsync<T>(string Asm, IntPtr Address)
        {
            return Task.Run(() => this.InjectAndExecute<T>(Asm, Address));
        }

        /// <summary>
        /// Assembles, injects and executes asynchronously the mnemonics into the remote process at the specified address.
        /// </summary>
        /// <param name="Asm">The mnemonics to inject.</param>
        /// <param name="Address">The address where the assembly code is injected.</param>
        /// <returns>The return value is an asynchronous operation that return the exit code of the thread created to execute the assembly code.</returns>
        public Task<IntPtr> InjectAndExecuteAsync(string Asm, IntPtr Address)
        {
            return this.InjectAndExecuteAsync<IntPtr>(Asm, Address);
        }

        /// <summary>
        /// Assembles, injects and executes asynchronously the mnemonics into the remote process at the specified address.
        /// </summary>
        /// <param name="Asm">An array containing the mnemonics to inject.</param>
        /// <param name="Address">The address where the assembly code is injected.</param>
        /// <returns>The return value is an asynchronous operation that return the exit code of the thread created to execute the assembly code.</returns>
        public Task<T> InjectAndExecuteAsync<T>(string[] Asm, IntPtr Address)
        {
            return Task.Run(() => this.InjectAndExecute<T>(Asm, Address));
        }

        /// <summary>
        /// Assembles, injects and executes asynchronously the mnemonics into the remote process at the specified address.
        /// </summary>
        /// <param name="Asm">An array containing the mnemonics to inject.</param>
        /// <param name="Address">The address where the assembly code is injected.</param>
        /// <returns>The return value is an asynchronous operation that return the exit code of the thread created to execute the assembly code.</returns>
        public Task<IntPtr> InjectAndExecuteAsync(string[] Asm, IntPtr Address)
        {
            return this.InjectAndExecuteAsync<IntPtr>(Asm, Address);
        }

        /// <summary>
        /// Assembles, injects and executes asynchronously the mnemonics into the remote process.
        /// </summary>
        /// <param name="Asm">The mnemonics to inject.</param>
        /// <returns>The return value is an asynchronous operation that return the exit code of the thread created to execute the assembly code.</returns>
        public Task<T> InjectAndExecuteAsync<T>(string Asm)
        {
            return Task.Run(() => this.InjectAndExecute<T>(Asm));
        }

        /// <summary>
        /// Assembles, injects and executes asynchronously the mnemonics into the remote process.
        /// </summary>
        /// <param name="Asm">The mnemonics to inject.</param>
        /// <returns>The return value is an asynchronous operation that return the exit code of the thread created to execute the assembly code.</returns>
        public Task<IntPtr> InjectAndExecuteAsync(string Asm)
        {
            return this.InjectAndExecuteAsync<IntPtr>(Asm);
        }

        /// <summary>
        /// Assembles, injects and executes asynchronously the mnemonics into the remote process.
        /// </summary>
        /// <param name="Asm">An array containing the mnemonics to inject.</param>
        /// <returns>The return value is an asynchronous operation that return the exit code of the thread created to execute the assembly code.</returns>
        public Task<T> InjectAndExecuteAsync<T>(string[] Asm)
        {
            return Task.Run(() => this.InjectAndExecute<T>(Asm));
        }

        /// <summary>
        /// Assembles, injects and executes asynchronously the mnemonics into the remote process.
        /// </summary>
        /// <param name="Asm">An array containing the mnemonics to inject.</param>
        /// <returns>The return value is an asynchronous operation that return the exit code of the thread created to execute the assembly code.</returns>
        public Task<IntPtr> InjectAndExecuteAsync(string[] Asm)
        {
            return this.InjectAndExecuteAsync<IntPtr>(Asm);
        }
    }
}