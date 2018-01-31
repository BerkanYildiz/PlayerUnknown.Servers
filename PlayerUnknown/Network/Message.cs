namespace PlayerUnknown.Network
{
    using System.Collections.Generic;

    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    using PlayerUnknown.Exceptions;

    [JsonObject(MemberSerialization.OptIn)]
    public sealed class Message
    {
        [JsonProperty] public long Identifier;

        [JsonProperty] public string Unknown;
        [JsonProperty] public string Service;
        [JsonProperty] public string Method;

        [JsonProperty] public List<object> Parameters;

        /// <summary>
        /// Returns true if this <see cref="Message"/> is valid.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is valid; otherwise, <c>false</c>.
        /// </value>
        public bool IsValid
        {
            get
            {
                return !string.IsNullOrEmpty(this.Service) && !string.IsNullOrEmpty(this.Method);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Message"/> class.
        /// </summary>
        public Message()
        {
            this.Parameters = new List<object>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Message"/> class.
        /// </summary>
        /// <param name="Data">The data.</param>
        public Message(string Data) : this()
        {
            var Json = JArray.Parse(Data);

            if (Json != null)
            {
                if (Json.Count < 4)
                {
                    throw new PubgMessageException("Json.Count < 4 at Message(Data).");
                }

                this.Identifier = Json[0].ToObject<long>();
                this.Unknown    = Json[1].ToObject<string>();
                this.Service    = Json[2].ToObject<string>();
                this.Method     = Json[3].ToObject<string>();

                if (Json.Count > 4)
                {
                    for (int i = 4; i < Json.Count; i++)
                    {
                        this.Parameters.Add(Json[i]);
                    }
                }

                this.Log();
            }
            else
            {
                throw new PubgMessageException("Json == null at Message(Data).");
            }
        }

        /// <summary>
        /// Turns this <see cref="Message"/> into a JSON string.
        /// </summary>
        public string ToJson()
        {
            JArray Json = new JArray(this.Identifier, this.Unknown, this.Service, this.Method);

            foreach (object Parameter in this.Parameters)
            {
                Json.Add(Parameter);
            }

            return Json.ToString(Formatting.None);
        }

        /// <summary>
        /// Logs this instance.
        /// </summary>
        public void Log()
        {
            Logging.Info(this.GetType(), "ID : " + this.Identifier + ", " + this.Service + "->" + this.Method + ".");
        }
    }
}
