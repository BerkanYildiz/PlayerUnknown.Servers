namespace PlayerUnknown.Network
{
    using System.Collections.Generic;

    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    using PlayerUnknown.Exceptions;

    [JsonObject(MemberSerialization.OptIn)]
    public class Message
    {
        [JsonProperty] public long Identifier;
        [JsonProperty] public List<object> Parameters;

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
        /// <param name="Identifier">The identifier.</param>
        /// <param name="Parameters">The parameters.</param>
        public Message(long Identifier, params object[] Parameters)
        {
            this.Identifier = Identifier;
            this.Parameters = new List<object>(Parameters);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Message"/> class.
        /// </summary>
        /// <param name="Data">The data.</param>
        public Message(string Data)
        {
            this.Parse(Data);
        }

        /// <summary>
        /// Parses the specified data.
        /// </summary>
        /// <param name="Data">The data.</param>
        public void Parse(string Data)
        {
            if (string.IsNullOrEmpty(Data) == false)
            {
                var Json = JArray.Parse(Data);

                if (Json != null)
                {
                    if (Json.Count < 4)
                    {
                        throw new PubgMessageException("Json.Count < 4 at Parse(Data).");
                    }

                    this.Identifier = Json[0].ToObject<long>();

                    if (this.Identifier >= 0)
                    {
                        Json.RemoveAt(0);

                        if (Json.HasValues)
                        {
                            this.Parameters = new List<object>(Json.Count);

                            foreach (var Property in Json)
                            {
                                switch (Property.Type)
                                {
                                    case JTokenType.String:
                                    {
                                        this.Parameters.Add((string) Property);
                                        break;
                                    }

                                    case JTokenType.Boolean:
                                    {
                                        this.Parameters.Add((bool) Property);
                                        break;
                                    }

                                    case JTokenType.Null:
                                    {
                                        this.Parameters.Add(null);
                                        break;
                                    }

                                    default:
                                    {
                                        this.Parameters.Add((object) Property);
                                        break;
                                    }
                                }
                            }
                        }
                        else
                        {
                            throw new PubgMessageException("Json.HasValues != true at Parse(Data).");
                        }
                    }
                    else
                    {
                        throw new PubgMessageException("this.Identifier < 0 at Parse(Data).");
                    }
                }
                else
                {
                    throw new PubgMessageException("Json == null at Parse(Data).");
                }
            }
            else
            {
                throw new PubgMessageException("string.IsNullOrEmpty(Data) != false at Parse(Data).");
            }
        }

        /// <summary>
        /// Sets the result.
        /// </summary>
        /// <param name="Result">The result.</param>
        /// <param name="Error">The error.</param>
        public void SetResult(object Result = null, object Error = null)
        {
            JObject Json = new JObject();

            if (Error != null)
            {
                Json.Add("Error", new JValue(Error));
            }
            else
            {
                Json.Add("Error", null);
            }

            if (Result != null)
            {
                if (Result is JArray)
                {
                    Json.Add("Result", (JArray) Result);
                }
                else if (Result is JObject)
                {
                    Json.Add("Result", (JObject)Result);
                }
                else
                {
                    Json.Add("Result", new JValue(Result));
                }
            }
            else
            {
                Json.Add("Result", null);
            }
            
            this.Parameters.Add(Json);
        }

        /// <summary>
        /// Turns this <see cref="Message"/> into a JSON string.
        /// </summary>
        public string ToJson()
        {
            JArray Json = new JArray(this.Identifier);

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
            Logging.Info(this.GetType(), this.ToString());
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        public override string ToString()
        {
            return this.Identifier + " - " + this.Parameters[1] + "->" + this.Parameters[2] + ".";
        }
    }
}
