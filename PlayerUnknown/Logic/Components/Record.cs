namespace PlayerUnknown.Logic.Components
{
    using Newtonsoft.Json.Linq;

    using PlayerUnknown.Logic.Interfaces;
    using PlayerUnknown.Logic.Interfaces.Players;

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
        /// Loads the specified json.
        /// </summary>
        /// <param name="Json">The json.</param>
        public void Load(JObject Json)
        {
            
        }

        /// <summary>
        /// Saves this instance into a json object.
        /// </summary>
        public JObject Save()
        {
            JObject Json = new JObject();

            return Json;
        }
    }
}
