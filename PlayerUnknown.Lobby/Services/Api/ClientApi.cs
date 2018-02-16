namespace PlayerUnknown.Lobby.Services.Api
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    using PlayerUnknown.Lobby.Models.Sessions;
    using PlayerUnknown.Network;

    public static class ClientApi
    {
        public static void ConnectionAccepted(PubgSession Session)
        {
            var Message = new Message
            {
                Service = "ClientApi",
                Method  = "ConnectionAccepted"
            };

            Message.Parameters.Add(Session.Account.AccountId);
            Message.Parameters.Add(JObject.Parse(JsonConvert.SerializeObject(Session.Player)));

            Session.Client.SendMessage(Message);

            Invalidate(Session, "party");
            Invalidate(Session, "match");
            Invalidate(Session, "game");
            Invalidate(Session, "event");
        }

        public static void Invalidate(PubgSession Session, string Class)
        {
            var Message = new Message
            {
                Service = "ClientApi",
                Method  = "Invalidate"
            };

            Message.Parameters.Add(Session.Account.AccountId);
            Message.Parameters.Add("client." + Class);
            Message.Parameters.Add(null);
            Message.Parameters.Add(null);

            Session.Client.SendMessage(Message);
        }
    }
}
