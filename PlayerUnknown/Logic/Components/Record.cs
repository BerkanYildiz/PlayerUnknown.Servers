namespace PlayerUnknown.Logic.Components
{
    using Newtonsoft.Json.Linq;

    using PlayerUnknown.Interfaces;

    public sealed class Record : IRecord
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Record"/> class.
        /// </summary>
        public Record()
        {
            // Record.
        }

        /// <summary>
        /// Saves this instance into a json object.
        /// </summary>
        public JObject ToJson()
        {
            JObject Json = new JObject();

            return Json;
        }
    }
}
