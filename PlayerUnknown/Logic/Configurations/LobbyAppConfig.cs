namespace PlayerUnknown.Logic.Configurations
{
    using Newtonsoft.Json.Linq;

    using PlayerUnknown.Logic.Interfaces;

    public class LobbyAppConfig : IAppConfig
    {
        /// <summary>
        /// Gets the report URL.
        /// </summary>
        public string ReportUrl
        {
            get;
            private set;
        } = "http://report.playbattlegrounds.com/report";

        /// <summary>
        /// Saves this instance into a json object.
        /// </summary>
        /// <returns></returns>
        public JObject Save()
        {
            JObject Json = new JObject();

            Json.Add("REPORT_URL", this.ReportUrl);

            return Json;
        }

        /// <summary>
        /// Loads the specified json.
        /// </summary>
        /// <param name="Json">The json.</param>
        public void Load(JObject Json)
        {
            if (Json.ContainsKey("REPORT_URL"))
            {
                this.ReportUrl = Json.GetValue("REPORT_URL").ToObject<string>();
            }
        }
    }
}
