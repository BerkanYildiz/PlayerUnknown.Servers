namespace PlayerUnknown.Reader.Native
{
    using System;

    using PlayerUnknown.Reader.Memory;

    /// <summary>
    /// Class representing the Process Environment Block of a remote process.
    /// </summary>
    public class ManagedPeb : RemotePointer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ManagedPeb"/> class.
        /// </summary>
        /// <param name="BattleGroundMemory">The reference of the <see cref="BattleGroundMemory"/> object.</param>
        /// <param name="Address">The location of the peb.</param>
        internal ManagedPeb(BattleGroundMemory BattleGroundMemory, IntPtr Address)
            : base(BattleGroundMemory, Address)
        {
        }

        public byte InheritedAddressSpace
        {
            get
            {
                return this.Read<byte>(PebStructure.InheritedAddressSpace);
            }

            set
            {
                this.Write(PebStructure.InheritedAddressSpace, value);
            }
        }

        public byte ReadImageFileExecOptions
        {
            get
            {
                return this.Read<byte>(PebStructure.ReadImageFileExecOptions);
            }

            set
            {
                this.Write(PebStructure.ReadImageFileExecOptions, value);
            }
        }

        public bool BeingDebugged
        {
            get
            {
                return this.Read<bool>(PebStructure.BeingDebugged);
            }

            set
            {
                this.Write(PebStructure.BeingDebugged, value);
            }
        }

        public byte SpareBool
        {
            get
            {
                return this.Read<byte>(PebStructure.SpareBool);
            }

            set
            {
                this.Write(PebStructure.SpareBool, value);
            }
        }

        public IntPtr Mutant
        {
            get
            {
                return this.Read<IntPtr>(PebStructure.Mutant);
            }

            set
            {
                this.Write(PebStructure.Mutant, value);
            }
        }

        public IntPtr Ldr
        {
            get
            {
                return this.Read<IntPtr>(PebStructure.Ldr);
            }

            set
            {
                this.Write(PebStructure.Ldr, value);
            }
        }

        public IntPtr ProcessParameters
        {
            get
            {
                return this.Read<IntPtr>(PebStructure.ProcessParameters);
            }

            set
            {
                this.Write(PebStructure.ProcessParameters, value);
            }
        }

        public IntPtr SubSystemData
        {
            get
            {
                return this.Read<IntPtr>(PebStructure.SubSystemData);
            }

            set
            {
                this.Write(PebStructure.SubSystemData, value);
            }
        }

        public IntPtr ProcessHeap
        {
            get
            {
                return this.Read<IntPtr>(PebStructure.ProcessHeap);
            }

            set
            {
                this.Write(PebStructure.ProcessHeap, value);
            }
        }

        public IntPtr FastPebLock
        {
            get
            {
                return this.Read<IntPtr>(PebStructure.FastPebLock);
            }

            set
            {
                this.Write(PebStructure.FastPebLock, value);
            }
        }

        public IntPtr FastPebLockRoutine
        {
            get
            {
                return this.Read<IntPtr>(PebStructure.FastPebLockRoutine);
            }

            set
            {
                this.Write(PebStructure.FastPebLockRoutine, value);
            }
        }

        public IntPtr FastPebUnlockRoutine
        {
            get
            {
                return this.Read<IntPtr>(PebStructure.FastPebUnlockRoutine);
            }

            set
            {
                this.Write(PebStructure.FastPebUnlockRoutine, value);
            }
        }

        public IntPtr EnvironmentUpdateCount
        {
            get
            {
                return this.Read<IntPtr>(PebStructure.EnvironmentUpdateCount);
            }

            set
            {
                this.Write(PebStructure.EnvironmentUpdateCount, value);
            }
        }

        public IntPtr KernelCallbackTable
        {
            get
            {
                return this.Read<IntPtr>(PebStructure.KernelCallbackTable);
            }

            set
            {
                this.Write(PebStructure.KernelCallbackTable, value);
            }
        }

        public int SystemReserved
        {
            get
            {
                return this.Read<int>(PebStructure.SystemReserved);
            }

            set
            {
                this.Write(PebStructure.SystemReserved, value);
            }
        }

        public int AtlThunkSListPtr32
        {
            get
            {
                return this.Read<int>(PebStructure.AtlThunkSListPtr32);
            }

            set
            {
                this.Write(PebStructure.AtlThunkSListPtr32, value);
            }
        }

        public IntPtr FreeList
        {
            get
            {
                return this.Read<IntPtr>(PebStructure.FreeList);
            }

            set
            {
                this.Write(PebStructure.FreeList, value);
            }
        }

        public IntPtr TlsExpansionCounter
        {
            get
            {
                return this.Read<IntPtr>(PebStructure.TlsExpansionCounter);
            }

            set
            {
                this.Write(PebStructure.TlsExpansionCounter, value);
            }
        }

        public IntPtr TlsBitmap
        {
            get
            {
                return this.Read<IntPtr>(PebStructure.TlsBitmap);
            }

            set
            {
                this.Write(PebStructure.TlsBitmap, value);
            }
        }

        public long TlsBitmapBits
        {
            get
            {
                return this.Read<long>(PebStructure.TlsBitmapBits);
            }

            set
            {
                this.Write(PebStructure.TlsBitmapBits, value);
            }
        }

        public IntPtr ReadOnlySharedMemoryBase
        {
            get
            {
                return this.Read<IntPtr>(PebStructure.ReadOnlySharedMemoryBase);
            }

            set
            {
                this.Write(PebStructure.ReadOnlySharedMemoryBase, value);
            }
        }

        public IntPtr ReadOnlySharedMemoryHeap
        {
            get
            {
                return this.Read<IntPtr>(PebStructure.ReadOnlySharedMemoryHeap);
            }

            set
            {
                this.Write(PebStructure.ReadOnlySharedMemoryHeap, value);
            }
        }

        public IntPtr ReadOnlyStaticServerData
        {
            get
            {
                return this.Read<IntPtr>(PebStructure.ReadOnlyStaticServerData);
            }

            set
            {
                this.Write(PebStructure.ReadOnlyStaticServerData, value);
            }
        }

        public IntPtr AnsiCodePageData
        {
            get
            {
                return this.Read<IntPtr>(PebStructure.AnsiCodePageData);
            }

            set
            {
                this.Write(PebStructure.AnsiCodePageData, value);
            }
        }

        public IntPtr OemCodePageData
        {
            get
            {
                return this.Read<IntPtr>(PebStructure.OemCodePageData);
            }

            set
            {
                this.Write(PebStructure.OemCodePageData, value);
            }
        }

        public IntPtr UnicodeCaseTableData
        {
            get
            {
                return this.Read<IntPtr>(PebStructure.UnicodeCaseTableData);
            }

            set
            {
                this.Write(PebStructure.UnicodeCaseTableData, value);
            }
        }

        public int NumberOfProcessors
        {
            get
            {
                return this.Read<int>(PebStructure.NumberOfProcessors);
            }

            set
            {
                this.Write(PebStructure.NumberOfProcessors, value);
            }
        }

        public long NtGlobalFlag
        {
            get
            {
                return this.Read<long>(PebStructure.NtGlobalFlag);
            }

            set
            {
                this.Write(PebStructure.NtGlobalFlag, value);
            }
        }

        public long CriticalSectionTimeout
        {
            get
            {
                return this.Read<long>(PebStructure.CriticalSectionTimeout);
            }

            set
            {
                this.Write(PebStructure.CriticalSectionTimeout, value);
            }
        }

        public IntPtr HeapSegmentReserve
        {
            get
            {
                return this.Read<IntPtr>(PebStructure.HeapSegmentReserve);
            }

            set
            {
                this.Write(PebStructure.HeapSegmentReserve, value);
            }
        }

        public IntPtr HeapSegmentCommit
        {
            get
            {
                return this.Read<IntPtr>(PebStructure.HeapSegmentCommit);
            }

            set
            {
                this.Write(PebStructure.HeapSegmentCommit, value);
            }
        }

        public IntPtr HeapDeCommitTotalFreeThreshold
        {
            get
            {
                return this.Read<IntPtr>(PebStructure.HeapDeCommitTotalFreeThreshold);
            }

            set
            {
                this.Write(PebStructure.HeapDeCommitTotalFreeThreshold, value);
            }
        }

        public IntPtr HeapDeCommitFreeBlockThreshold
        {
            get
            {
                return this.Read<IntPtr>(PebStructure.HeapDeCommitFreeBlockThreshold);
            }

            set
            {
                this.Write(PebStructure.HeapDeCommitFreeBlockThreshold, value);
            }
        }

        public int NumberOfHeaps
        {
            get
            {
                return this.Read<int>(PebStructure.NumberOfHeaps);
            }

            set
            {
                this.Write(PebStructure.NumberOfHeaps, value);
            }
        }

        public int MaximumNumberOfHeaps
        {
            get
            {
                return this.Read<int>(PebStructure.MaximumNumberOfHeaps);
            }

            set
            {
                this.Write(PebStructure.MaximumNumberOfHeaps, value);
            }
        }

        public IntPtr ProcessHeaps
        {
            get
            {
                return this.Read<IntPtr>(PebStructure.ProcessHeaps);
            }

            set
            {
                this.Write(PebStructure.ProcessHeaps, value);
            }
        }

        public IntPtr GdiSharedHandleTable
        {
            get
            {
                return this.Read<IntPtr>(PebStructure.GdiSharedHandleTable);
            }

            set
            {
                this.Write(PebStructure.GdiSharedHandleTable, value);
            }
        }

        public IntPtr ProcessStarterHelper
        {
            get
            {
                return this.Read<IntPtr>(PebStructure.ProcessStarterHelper);
            }

            set
            {
                this.Write(PebStructure.ProcessStarterHelper, value);
            }
        }

        public IntPtr GdiDcAttributeList
        {
            get
            {
                return this.Read<IntPtr>(PebStructure.GdiDcAttributeList);
            }

            set
            {
                this.Write(PebStructure.GdiDcAttributeList, value);
            }
        }

        public IntPtr LoaderLock
        {
            get
            {
                return this.Read<IntPtr>(PebStructure.LoaderLock);
            }

            set
            {
                this.Write(PebStructure.LoaderLock, value);
            }
        }

        public int OsMajorVersion
        {
            get
            {
                return this.Read<int>(PebStructure.OsMajorVersion);
            }

            set
            {
                this.Write(PebStructure.OsMajorVersion, value);
            }
        }

        public int OsMinorVersion
        {
            get
            {
                return this.Read<int>(PebStructure.OsMinorVersion);
            }

            set
            {
                this.Write(PebStructure.OsMinorVersion, value);
            }
        }

        public ushort OsBuildNumber
        {
            get
            {
                return this.Read<ushort>(PebStructure.OsBuildNumber);
            }

            set
            {
                this.Write(PebStructure.OsBuildNumber, value);
            }
        }

        public ushort OsCsdVersion
        {
            get
            {
                return this.Read<ushort>(PebStructure.OsCsdVersion);
            }

            set
            {
                this.Write(PebStructure.OsCsdVersion, value);
            }
        }

        public int OsPlatformId
        {
            get
            {
                return this.Read<int>(PebStructure.OsPlatformId);
            }

            set
            {
                this.Write(PebStructure.OsPlatformId, value);
            }
        }

        public int ImageSubsystem
        {
            get
            {
                return this.Read<int>(PebStructure.ImageSubsystem);
            }

            set
            {
                this.Write(PebStructure.ImageSubsystem, value);
            }
        }

        public int ImageSubsystemMajorVersion
        {
            get
            {
                return this.Read<int>(PebStructure.ImageSubsystemMajorVersion);
            }

            set
            {
                this.Write(PebStructure.ImageSubsystemMajorVersion, value);
            }
        }

        public IntPtr ImageSubsystemMinorVersion
        {
            get
            {
                return this.Read<IntPtr>(PebStructure.ImageSubsystemMinorVersion);
            }

            set
            {
                this.Write(PebStructure.ImageSubsystemMinorVersion, value);
            }
        }

        public IntPtr ImageProcessAffinityMask
        {
            get
            {
                return this.Read<IntPtr>(PebStructure.ImageProcessAffinityMask);
            }

            set
            {
                this.Write(PebStructure.ImageProcessAffinityMask, value);
            }
        }

        public IntPtr[] GdiHandleBuffer
        {
            get
            {
                return this.Read<IntPtr>(PebStructure.GdiHandleBuffer, 0x22);
            }

            set
            {
                this.Write(PebStructure.GdiHandleBuffer, value);
            }
        }

        public IntPtr PostProcessInitRoutine
        {
            get
            {
                return this.Read<IntPtr>(PebStructure.PostProcessInitRoutine);
            }

            set
            {
                this.Write(PebStructure.PostProcessInitRoutine, value);
            }
        }

        public IntPtr TlsExpansionBitmap
        {
            get
            {
                return this.Read<IntPtr>(PebStructure.TlsExpansionBitmap);
            }

            set
            {
                this.Write(PebStructure.TlsExpansionBitmap, value);
            }
        }

        public IntPtr[] TlsExpansionBitmapBits
        {
            get
            {
                return this.Read<IntPtr>(PebStructure.TlsExpansionBitmapBits, 0x20);
            }

            set
            {
                this.Write(PebStructure.TlsExpansionBitmapBits, value);
            }
        }

        public IntPtr SessionId
        {
            get
            {
                return this.Read<IntPtr>(PebStructure.SessionId);
            }

            set
            {
                this.Write(PebStructure.SessionId, value);
            }
        }

        public long AppCompatFlags
        {
            get
            {
                return this.Read<long>(PebStructure.AppCompatFlags);
            }

            set
            {
                this.Write(PebStructure.AppCompatFlags, value);
            }
        }

        public long AppCompatFlagsUser
        {
            get
            {
                return this.Read<long>(PebStructure.AppCompatFlagsUser);
            }

            set
            {
                this.Write(PebStructure.AppCompatFlagsUser, value);
            }
        }

        public IntPtr ShimData
        {
            get
            {
                return this.Read<IntPtr>(PebStructure.ShimData);
            }

            set
            {
                this.Write(PebStructure.ShimData, value);
            }
        }

        public IntPtr AppCompatInfo
        {
            get
            {
                return this.Read<IntPtr>(PebStructure.AppCompatInfo);
            }

            set
            {
                this.Write(PebStructure.AppCompatInfo, value);
            }
        }

        public long CsdVersion
        {
            get
            {
                return this.Read<long>(PebStructure.CsdVersion);
            }

            set
            {
                this.Write(PebStructure.CsdVersion, value);
            }
        }

        public IntPtr ActivationContextData
        {
            get
            {
                return this.Read<IntPtr>(PebStructure.ActivationContextData);
            }

            set
            {
                this.Write(PebStructure.ActivationContextData, value);
            }
        }

        public IntPtr ProcessAssemblyStorageMap
        {
            get
            {
                return this.Read<IntPtr>(PebStructure.ProcessAssemblyStorageMap);
            }

            set
            {
                this.Write(PebStructure.ProcessAssemblyStorageMap, value);
            }
        }

        public IntPtr SystemDefaultActivationContextData
        {
            get
            {
                return this.Read<IntPtr>(PebStructure.SystemDefaultActivationContextData);
            }

            set
            {
                this.Write(PebStructure.SystemDefaultActivationContextData, value);
            }
        }

        public IntPtr SystemAssemblyStorageMap
        {
            get
            {
                return this.Read<IntPtr>(PebStructure.SystemAssemblyStorageMap);
            }

            set
            {
                this.Write(PebStructure.SystemAssemblyStorageMap, value);
            }
        }

        public IntPtr MinimumStackCommit
        {
            get
            {
                return this.Read<IntPtr>(PebStructure.MinimumStackCommit);
            }

            set
            {
                this.Write(PebStructure.MinimumStackCommit, value);
            }
        }

        /// <summary>
        /// Finds the Process Environment Block address of a specified process.
        /// </summary>
        /// <param name="ProcessHandle">A handle of the process.</param>
        /// <returns>A <see cref="IntPtr"/> pointer of the PEB.</returns>
        public static IntPtr FindPeb(SafeMemoryHandle ProcessHandle)
        {
            return MemoryCore.NtQueryInformationProcess(ProcessHandle).PebBaseAddress;
        }
    }
}