namespace PlayerUnknown.Lobby.Services.Api
{
    using PlayerUnknown.Lobby.Models.Sessions;
    using PlayerUnknown.Logic.Network;

    public static class ClientApi
    {
        public static void ConnectionAccepted(PubgSession Session)
        {
            Session.Client.SendMessage("[0,null,\"ClientApi\",\"ConnectionAccepted\",\"account.d97a9d0dc25948f18348816373392734\",{\"profile\":{\"Nickname\":\"xxVertigo\",\"ProfileStatus\":null,\"InviteAllow\":\"all\",\"Skin\":{\"Gender\":\"female\",\"Hair\":\"skindesc.female.hair.02.02\",\"Face\":\"skindesc.female.face.01.01\",\"Presets\":\"female:F_Hair_B_02:F_Face_01:F_NudeBody_01\"}},\"inventory\":null,\"record\":null,\"account\":{\"AccountId\":\"account.d97a9d0dc25948f18348816373392734\",\"Origin\":\"steam.76561198073428385\",\"Region\":\"na\",\"PartnerId\":null,\"MatchAndPerspective\":\"solo\"},\"inviteAllow\":\"all\",\"playinggame\":null,\"avatarUrl\":\"https://steamcdn-a.akamaihd.net/steamcommunity/public/images/avatars/58/58996926e5392cfeafbc867571cb7fc75fb5ecba.jpg\",\"lobbyAppConfig\":{\"REPORT_URL\":\"https://prod-live-report-server.pubgbluehole.com/report\"},\"pingServers\":[{\"Key\":\"na\",\"Value\":\"35.153.218.241:31112\"},{\"Key\":\"as\",\"Value\":\"13.125.109.210:31112\"},{\"Key\":\"sea\",\"Value\":\"13.229.164.235:31112\"},{\"Key\":\"oc\",\"Value\":\"13.210.157.45:31112\"},{\"Key\":\"eu\",\"Value\":\"18.196.152.192:31112\"},{\"Key\":\"sa\",\"Value\":\"18.231.45.91:31112\"},{\"Key\":\"krjp\",\"Value\":\"13.125.109.210:31112\"},{\"Key\":\"kakao\",\"Value\":\"13.125.109.210:31112\"}],\"broOnlineServices\":{\"AuthenticationOff\":false,\"InventoryOff\":false,\"EquipOff\":false,\"StoreOff\":false,\"SkinOff\":false,\"PromotionCodeOff\":false,\"RecordOff\":false,\"MatchMakerOff\":false,\"PartyOff\":false,\"CustomGameOff\":false,\"OuterOff\":false,\"RejoinOff\":false,\"LoginCheckOff\":false,\"SteamCircuitBreakerOff\":false,\"LobbyVoiceChatOff\":false,\"TemporaryBanOff\":false,\"ClientValidationOff\":false,\"VpnCheckOff\":false},\"autoRequestMatch\":{\"On\":false,\"Region\":null,\"MatchType\":null}}]");
           
            /* var Message = new Message(0, null, "ClientApi", "ConnectionAccepted");

            Message.Parameters.Add(Session.Account.AccountId);
            Message.Parameters.Add(Session.Player.Save());

            Session.Client.SendMessage(Message); */

            Invalidate(Session, "party");
            Invalidate(Session, "match");
            Invalidate(Session, "game");
            Invalidate(Session, "event");
        }

        public static void Invalidate(PubgSession Session, string Class)
        {
            var Message = new Message(0, null, "ClientApi", "Invalidate");

            Message.Parameters.Add(Session.Account.AccountId);
            Message.Parameters.Add("client." + Class);
            Message.Parameters.Add(null);
            Message.Parameters.Add(null);

            Session.Client.SendMessage(Message);
        }
    }
}
