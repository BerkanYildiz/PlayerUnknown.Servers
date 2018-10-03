namespace PlayerUnknown.Exceptions
{
    using System;

    public class PubgCurrencyException : Exception
    {
        /// <summary>
        /// Obtient un message qui décrit l'exception actuelle.
        /// </summary>
        public string Message
        {
            get;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PubgCurrencyException"/> class.
        /// </summary>
        public PubgCurrencyException() : base()
        {
            Log.Error(this.GetType(), "The currency system threw an exception.");
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PubgCurrencyException"/> class.
        /// </summary>
        /// <param name="Message">The message.</param>
        public PubgCurrencyException(string Message) : base(Message)
        {
            this.Message = Message;

            if (string.IsNullOrEmpty(Message) == false)
            {
                Log.Error(this.GetType(), Message);
            }
        }
    }
}
