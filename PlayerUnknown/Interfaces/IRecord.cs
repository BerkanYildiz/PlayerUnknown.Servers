namespace PlayerUnknown.Interfaces
{
    using Newtonsoft.Json.Linq;

    public interface IRecord
    {
        /// <summary>
        /// Saves this instance into a json object.
        /// </summary>
        JObject ToJson();
    }
}
