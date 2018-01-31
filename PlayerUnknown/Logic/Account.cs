namespace PlayerUnknown.Logic
{
    using Newtonsoft.Json;

    [JsonObject(MemberSerialization.OptIn)]
    public sealed class Account
    {
        [JsonProperty("AccountId")]     public string AccountId;
        [JsonProperty("PartnerId")]     public string PartnerId;
        [JsonProperty("Region")]        public string Region;
        [JsonProperty("Password")]      public string Password;
    }
}
