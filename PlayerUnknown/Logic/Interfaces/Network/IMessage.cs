namespace PlayerUnknown.Logic.Interfaces.Network
{
    using System.Collections.Generic;

    public interface IMessage : IObject
    {
        /// <summary>
        /// Gets the identifier.
        /// </summary>
        long Identifier
        {
            get;
        }

        /// <summary>
        /// Gets the parameters.
        /// </summary>
        List<object> Parameters
        {
            get;
        }

        /// <summary>
        /// Parses the specified json.
        /// </summary>
        /// <param name="Json">The json.</param>
        void Parse(string Json);
    }
}
