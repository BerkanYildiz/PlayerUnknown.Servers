namespace PlayerUnknown.Reader.Native
{
    using System;

    using PlayerUnknown.Reader.Memory;
    using PlayerUnknown.Reader.Threading;

    /// <summary>
    /// Class representing the Thread Environment Block of a remote thread.
    /// </summary>
    public class ManagedTeb : RemotePointer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ManagedTeb"/> class.
        /// </summary>
        /// <param name="BattleGroundMemory">The reference of the <see cref="BattleGroundMemory"/> object.</param>
        /// <param name="Address">The location of the teb.</param>
        internal ManagedTeb(BattleGroundMemory BattleGroundMemory, IntPtr Address)
            : base(BattleGroundMemory, Address)
        {
        }

        /// <summary>
        /// Current Structured Exception Handling (SEH) frame.
        /// </summary>
        public IntPtr CurrentSehFrame
        {
            get
            {
                return this.Read<IntPtr>(TebStructure.CurrentSehFrame);
            }

            set
            {
                this.Write(TebStructure.CurrentSehFrame, value);
            }
        }

        /// <summary>
        /// The top of stack.
        /// </summary>
        public IntPtr TopOfStack
        {
            get
            {
                return this.Read<IntPtr>(TebStructure.TopOfStack);
            }

            set
            {
                this.Write(TebStructure.TopOfStack, value);
            }
        }

        /// <summary>
        /// The current bottom of stack.
        /// </summary>
        public IntPtr BottomOfStack
        {
            get
            {
                return this.Read<IntPtr>(TebStructure.BottomOfStack);
            }

            set
            {
                this.Write(TebStructure.BottomOfStack, value);
            }
        }

        /// <summary>
        /// The TEB sub system.
        /// </summary>
        public IntPtr SubSystemTeb
        {
            get
            {
                return this.Read<IntPtr>(TebStructure.SubSystemTeb);
            }

            set
            {
                this.Write(TebStructure.SubSystemTeb, value);
            }
        }

        /// <summary>
        /// The fiber data.
        /// </summary>
        public IntPtr FiberData
        {
            get
            {
                return this.Read<IntPtr>(TebStructure.FiberData);
            }

            set
            {
                this.Write(TebStructure.FiberData, value);
            }
        }

        /// <summary>
        /// The arbitrary data slot.
        /// </summary>
        public IntPtr ArbitraryDataSlot
        {
            get
            {
                return this.Read<IntPtr>(TebStructure.ArbitraryDataSlot);
            }

            set
            {
                this.Write(TebStructure.ArbitraryDataSlot, value);
            }
        }

        /// <summary>
        /// The linear address of Thread Environment Block (TEB).
        /// </summary>
        public IntPtr Teb
        {
            get
            {
                return this.Read<IntPtr>(TebStructure.Teb);
            }

            set
            {
                this.Write(TebStructure.Teb, value);
            }
        }

        /// <summary>
        /// The environment pointer.
        /// </summary>
        public IntPtr EnvironmentPointer
        {
            get
            {
                return this.Read<IntPtr>(TebStructure.EnvironmentPointer);
            }

            set
            {
                this.Write(TebStructure.EnvironmentPointer, value);
            }
        }

        /// <summary>
        /// The process Id.
        /// </summary>
        public int ProcessId
        {
            get
            {
                return this.Read<int>(TebStructure.ProcessId);
            }

            set
            {
                this.Write(TebStructure.ProcessId, value);
            }
        }

        /// <summary>
        /// The current thread Id.
        /// </summary>
        public int ThreadId
        {
            get
            {
                return this.Read<int>(TebStructure.ThreadId);
            }

            set
            {
                this.Write(TebStructure.ThreadId, value);
            }
        }

        /// <summary>
        /// The active RPC handle.
        /// </summary>
        public IntPtr RpcHandle
        {
            get
            {
                return this.Read<IntPtr>(TebStructure.RpcHandle);
            }

            set
            {
                this.Write(TebStructure.RpcHandle, value);
            }
        }

        /// <summary>
        /// The linear address of the thread-local storage (TLS) array.
        /// </summary>
        public IntPtr Tls
        {
            get
            {
                return this.Read<IntPtr>(TebStructure.Tls);
            }

            set
            {
                this.Write(TebStructure.Tls, value);
            }
        }

        /// <summary>
        /// The linear address of Process Environment Block (PEB).
        /// </summary>
        public IntPtr Peb
        {
            get
            {
                return this.Read<IntPtr>(TebStructure.Peb);
            }

            set
            {
                this.Write(TebStructure.Peb, value);
            }
        }

        /// <summary>
        /// The last error number.
        /// </summary>
        public int LastErrorNumber
        {
            get
            {
                return this.Read<int>(TebStructure.LastErrorNumber);
            }

            set
            {
                this.Write(TebStructure.LastErrorNumber, value);
            }
        }

        /// <summary>
        /// The count of owned critical sections.
        /// </summary>
        public int CriticalSectionsCount
        {
            get
            {
                return this.Read<int>(TebStructure.CriticalSectionsCount);
            }

            set
            {
                this.Write(TebStructure.CriticalSectionsCount, value);
            }
        }

        /// <summary>
        /// The address of CSR Client Thread.
        /// </summary>
        public IntPtr CsrClientThread
        {
            get
            {
                return this.Read<IntPtr>(TebStructure.CsrClientThread);
            }

            set
            {
                this.Write(TebStructure.CsrClientThread, value);
            }
        }

        /// <summary>
        /// Win32 Thread Information.
        /// </summary>
        public IntPtr Win32ThreadInfo
        {
            get
            {
                return this.Read<IntPtr>(TebStructure.Win32ThreadInfo);
            }

            set
            {
                this.Write(TebStructure.Win32ThreadInfo, value);
            }
        }

        /// <summary>
        /// Win32 client information (NT), user32 private data (Wine), 0x60 = LastError (Win95), 0x74 = LastError (WinME).
        /// </summary>
        public byte[] Win32ClientInfo
        {
            get
            {
                return this.Read<byte>(TebStructure.Win32ClientInfo, 124);
            }

            set
            {
                this.Write(TebStructure.Win32ClientInfo, value);
            }
        }

        /// <summary>
        /// Reserved for Wow64. Contains a pointer to FastSysCall in Wow64.
        /// </summary>
        public IntPtr WoW64Reserved
        {
            get
            {
                return this.Read<IntPtr>(TebStructure.WoW64Reserved);
            }

            set
            {
                this.Write(TebStructure.WoW64Reserved, value);
            }
        }

        /// <summary>
        /// The current locale
        /// </summary>
        public IntPtr CurrentLocale
        {
            get
            {
                return this.Read<IntPtr>(TebStructure.CurrentLocale);
            }

            set
            {
                this.Write(TebStructure.CurrentLocale, value);
            }
        }

        /// <summary>
        /// The FP Software Status Register.
        /// </summary>
        public IntPtr FpSoftwareStatusRegister
        {
            get
            {
                return this.Read<IntPtr>(TebStructure.FpSoftwareStatusRegister);
            }

            set
            {
                this.Write(TebStructure.FpSoftwareStatusRegister, value);
            }
        }

        /// <summary>
        /// Reserved for OS (NT), kernel32 private data (Wine).
        /// herein: FS:[0x124] 4 NT Pointer to KTHREAD (ETHREAD) structure.
        /// </summary>
        public byte[] SystemReserved1
        {
            get
            {
                return this.Read<byte>(TebStructure.SystemReserved1, 216);
            }

            set
            {
                this.Write(TebStructure.SystemReserved1, value);
            }
        }

        /// <summary>
        /// The exception code.
        /// </summary>
        public IntPtr ExceptionCode
        {
            get
            {
                return this.Read<IntPtr>(TebStructure.ExceptionCode);
            }

            set
            {
                this.Write(TebStructure.ExceptionCode, value);
            }
        }

        /// <summary>
        /// The activation context stack.
        /// </summary>
        public byte[] ActivationContextStack
        {
            get
            {
                return this.Read<byte>(TebStructure.ActivationContextStack, 18);
            }

            set
            {
                this.Write(TebStructure.ActivationContextStack, value);
            }
        }

        /// <summary>
        /// The spare bytes (NT), ntdll private data (Wine).
        /// </summary>
        public byte[] SpareBytes
        {
            get
            {
                return this.Read<byte>(TebStructure.SpareBytes, 26);
            }

            set
            {
                this.Write(TebStructure.SpareBytes, value);
            }
        }

        /// <summary>
        /// Reserved for OS (NT), ntdll private data (Wine).
        /// </summary>
        public byte[] SystemReserved2
        {
            get
            {
                return this.Read<byte>(TebStructure.SystemReserved2, 40);
            }

            set
            {
                this.Write(TebStructure.SystemReserved2, value);
            }
        }

        /// <summary>
        /// The GDI TEB Batch (OS), vm86 private data (Wine).
        /// </summary>
        public byte[] GdiTebBatch
        {
            get
            {
                return this.Read<byte>(TebStructure.GdiTebBatch, 1248);
            }

            set
            {
                this.Write(TebStructure.GdiTebBatch, value);
            }
        }

        /// <summary>
        /// The GDI Region.
        /// </summary>
        public IntPtr GdiRegion
        {
            get
            {
                return this.Read<IntPtr>(TebStructure.GdiRegion);
            }

            set
            {
                this.Write(TebStructure.GdiRegion, value);
            }
        }

        /// <summary>
        /// The GDI Pen.
        /// </summary>
        public IntPtr GdiPen
        {
            get
            {
                return this.Read<IntPtr>(TebStructure.GdiPen);
            }

            set
            {
                this.Write(TebStructure.GdiPen, value);
            }
        }

        /// <summary>
        /// The GDI Brush.
        /// </summary>
        public IntPtr GdiBrush
        {
            get
            {
                return this.Read<IntPtr>(TebStructure.GdiBrush);
            }

            set
            {
                this.Write(TebStructure.GdiBrush, value);
            }
        }

        /// <summary>
        /// The real process Id.
        /// </summary>
        public int RealProcessId
        {
            get
            {
                return this.Read<int>(TebStructure.RealProcessId);
            }

            set
            {
                this.Write(TebStructure.RealProcessId, value);
            }
        }

        /// <summary>
        /// The real thread Id.
        /// </summary>
        public int RealThreadId
        {
            get
            {
                return this.Read<int>(TebStructure.RealThreadId);
            }

            set
            {
                this.Write(TebStructure.RealThreadId, value);
            }
        }

        /// <summary>
        /// The GDI cached process handle.
        /// </summary>
        public IntPtr GdiCachedProcessHandle
        {
            get
            {
                return this.Read<IntPtr>(TebStructure.GdiCachedProcessHandle);
            }

            set
            {
                this.Write(TebStructure.GdiCachedProcessHandle, value);
            }
        }

        /// <summary>
        /// The GDI client process Id (PID).
        /// </summary>
        public IntPtr GdiClientProcessId
        {
            get
            {
                return this.Read<IntPtr>(TebStructure.GdiClientProcessId);
            }

            set
            {
                this.Write(TebStructure.GdiClientProcessId, value);
            }
        }

        /// <summary>
        /// The GDI client thread Id (TID).
        /// </summary>
        public IntPtr GdiClientThreadId
        {
            get
            {
                return this.Read<IntPtr>(TebStructure.GdiClientThreadId);
            }

            set
            {
                this.Write(TebStructure.GdiClientThreadId, value);
            }
        }

        /// <summary>
        /// The GDI thread locale information.
        /// </summary>
        public IntPtr GdiThreadLocalInfo
        {
            get
            {
                return this.Read<IntPtr>(TebStructure.GdiThreadLocalInfo);
            }

            set
            {
                this.Write(TebStructure.GdiThreadLocalInfo, value);
            }
        }

        /// <summary>
        /// Reserved for user application.
        /// </summary>
        public byte[] UserReserved1
        {
            get
            {
                return this.Read<byte>(TebStructure.UserReserved1, 20);
            }

            set
            {
                this.Write(TebStructure.UserReserved1, value);
            }
        }

        /// <summary>
        /// Reserved for GL.
        /// </summary>
        public byte[] GlReserved1
        {
            get
            {
                return this.Read<byte>(TebStructure.GlReserved1, 1248);
            }

            set
            {
                this.Write(TebStructure.GlReserved1, value);
            }
        }

        /// <summary>
        /// The last value status value.
        /// </summary>
        public int LastStatusValue
        {
            get
            {
                return this.Read<int>(TebStructure.LastStatusValue);
            }

            set
            {
                this.Write(TebStructure.LastStatusValue, value);
            }
        }

        /// <summary>
        /// The static UNICODE_STRING buffer.
        /// </summary>
        public byte[] StaticUnicodeString
        {
            get
            {
                return this.Read<byte>(TebStructure.StaticUnicodeString, 532);
            }

            set
            {
                this.Write(TebStructure.StaticUnicodeString, value);
            }
        }

        /// <summary>
        /// The pointer to deallocation stack.
        /// </summary>
        public IntPtr DeallocationStack
        {
            get
            {
                return this.Read<IntPtr>(TebStructure.DeallocationStack);
            }

            set
            {
                this.Write(TebStructure.DeallocationStack, value);
            }
        }

        /// <summary>
        /// The TLS slots, 4 byte per slot.
        /// </summary>
        public IntPtr[] TlsSlots
        {
            get
            {
                return this.Read<IntPtr>(TebStructure.TlsSlots, 64);
            }

            set
            {
                this.Write(TebStructure.TlsSlots, value);
            }
        }

        /// <summary>
        /// The TLS links (LIST_ENTRY structure).
        /// </summary>
        public long TlsLinks
        {
            get
            {
                return this.Read<long>(TebStructure.TlsLinks);
            }

            set
            {
                this.Write(TebStructure.TlsLinks, value);
            }
        }

        /// <summary>
        /// Virtual DOS Machine.
        /// </summary>
        public IntPtr Vdm
        {
            get
            {
                return this.Read<IntPtr>(TebStructure.Vdm);
            }

            set
            {
                this.Write(TebStructure.Vdm, value);
            }
        }

        /// <summary>
        /// Reserved for RPC.
        /// </summary>
        public IntPtr RpcReserved
        {
            get
            {
                return this.Read<IntPtr>(TebStructure.RpcReserved);
            }

            set
            {
                this.Write(TebStructure.RpcReserved, value);
            }
        }

        /// <summary>
        /// The thread error mode (RtlSetThreadErrorMode).
        /// </summary>
        public IntPtr ThreadErrorMode
        {
            get
            {
                return this.Read<IntPtr>(TebStructure.ThreadErrorMode);
            }

            set
            {
                this.Write(TebStructure.ThreadErrorMode, value);
            }
        }

        /// <summary>
        /// Finds the Thread Environment Block address of a specified thread.
        /// </summary>
        /// <param name="ThreadHandle">A handle of the thread.</param>
        /// <returns>A <see cref="IntPtr"/> pointer of the TEB.</returns>
        public static IntPtr FindTeb(SafeMemoryHandle ThreadHandle)
        {
            return ThreadCore.NtQueryInformationThread(ThreadHandle).TebBaseAdress;
        }
    }
}