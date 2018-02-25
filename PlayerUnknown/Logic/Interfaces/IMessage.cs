namespace PlayerUnknown.Logic.Interfaces
{
    using System.Collections.Generic;

    using Newtonsoft.Json.Linq;

    public interface IMessage
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

        /// <summary>
        /// Loads the specified json.
        /// </summary>
        /// <param name="Json">The json.</param>
        void Load(JArray Json);

        /// <summary>
        /// Saves this instance into a json array.
        /// </summary>
        JArray Save();
    }
}
