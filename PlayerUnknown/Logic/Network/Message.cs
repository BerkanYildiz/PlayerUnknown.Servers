namespace PlayerUnknown.Logic.Network
{
    using System.Collections.Generic;

    using Newtonsoft.Json.Linq;

    using PlayerUnknown.Exceptions;
    using PlayerUnknown.Logic.Interfaces;

    public class Message : IMessage
    {
        /// <summary>
        /// Gets the identifier.
        /// </summary>
        public long Identifier
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the parameters.
        /// </summary>
        public List<object> Parameters
        {
            get;
            private set;
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
        /// Parses the specified json.
        /// </summary>
        /// <param name="Data">The json.</param>
        /// <exception cref="PubgMessageException"/>
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
        /// Loads the specified json.
        /// </summary>
        /// <param name="Json">The json.</param>
        public void Load(JArray Json)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Turns this <see cref="Message"/> into a JSON string.
        /// </summary>
        public JArray Save()
        {
            JArray Json = new JArray(this.Identifier);

            foreach (object Parameter in this.Parameters)
            {
                Json.Add(Parameter);
            }

            return Json;
        }
    }
}
