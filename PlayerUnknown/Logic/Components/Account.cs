namespace PlayerUnknown.Logic.Components
{
    using Newtonsoft.Json;

    [JsonObject(MemberSerialization.OptIn)]
    public sealed class Account
    {
        [JsonProperty("AccountId")]     public string AccountId;
        [JsonProperty("AppId")]         public string AppId;
        [JsonProperty("IsPartner")]     public bool IsPartner;
        [JsonProperty("OuterSource")]   public string OuterSource;
        [JsonProperty("PartnerId")]     public string PartnerId;
        [JsonProperty("PartnerLevel")]  public string PartnerLevel;
        [JsonProperty("PlayerNetId")]   public string PlayerNetId;
        [JsonProperty("UserSerial")]    public string UserSerial;
        [JsonProperty("Region")]        public string Region;
    }
}
