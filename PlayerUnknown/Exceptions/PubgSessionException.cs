namespace PlayerUnknown.Exceptions
{
    using System;

    public class PubgSessionException : Exception
    {
        /// <summary>
        /// Obtient un message qui décrit l'exception actuelle.
        /// </summary>
        public string Message
        {
            get;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PubgSessionException"/> class.
        /// </summary>
        public PubgSessionException() : base()
        {
            Log.Error(this.GetType(), "The session system threw an exception.");
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PubgSessionException"/> class.
        /// </summary>
        /// <param name="Message">The message.</param>
        public PubgSessionException(string Message) : base(Message)
        {
            this.Message = Message;

            if (string.IsNullOrEmpty(Message) == false)
            {
                Log.Error(this.GetType(), Message);
            }
        }
    }
}
