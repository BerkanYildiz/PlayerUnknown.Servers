namespace PlayerUnknown.Reader.Threading
{
    using System;

    /// <summary>
    /// Class containing a frozen thread. If an instance of this class is disposed, its associated thread is resumed.
    /// </summary>
    public class FrozenThread : IDisposable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FrozenThread"/> class.
        /// </summary>
        /// <param name="Thread">The frozen thread.</param>
        internal FrozenThread(RemoteThread Thread)
        {
            // Save the parameter
            this.Thread = Thread;
        }

        /// <summary>
        /// The frozen thread.
        /// </summary>
        public RemoteThread Thread
        {
            get;
        }

        /// <summary>
        /// Releases all resources used by the <see cref="RemoteThread"/> object.
        /// </summary>
        public virtual void Dispose()
        {
            // Unfreeze the thread
            this.Thread.Resume();
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        public override string ToString()
        {
            return string.Format("Id = {0}", this.Thread.Id);
        }
    }
}