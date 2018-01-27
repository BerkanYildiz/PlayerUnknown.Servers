namespace PlayerUnknown.Exceptions
{
    using System;

    public class ProcessNotFoundException : Exception
    {
        /// <summary>
        /// Obtient un message qui décrit l'exception actuelle.
        /// </summary>
        public string Message
        {
            get;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProcessNotFoundException"/> class.
        /// </summary>
        public ProcessNotFoundException() : base()
        {
            Logging.Error(this.GetType(), "The process could not be found.");
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProcessNotFoundException"/> class.
        /// </summary>
        /// <param name="Message">The message.</param>
        public ProcessNotFoundException(string Message) : base(Message)
        {
            this.Message = Message;

            if (string.IsNullOrEmpty(Message) == false)
            {
                Logging.Error(this.GetType(), Message);
            }
        }
    }
}
