namespace PlayerUnknown.Exceptions
{
    using System;

    public class PubgMessageException : Exception
    {
        /// <summary>
        /// Obtient un message qui décrit l'exception actuelle.
        /// </summary>
        public string Message
        {
            get;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PubgMessageException"/> class.
        /// </summary>
        public PubgMessageException() : base()
        {
            Logging.Error(this.GetType(), "The PUBG Message threw an exception.");
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PubgMessageException"/> class.
        /// </summary>
        /// <param name="Message">The message.</param>
        public PubgMessageException(string Message) : base(Message)
        {
            this.Message = Message;

            if (string.IsNullOrEmpty(Message) == false)
            {
                Logging.Error(this.GetType(), Message);
            }
        }
    }
}
