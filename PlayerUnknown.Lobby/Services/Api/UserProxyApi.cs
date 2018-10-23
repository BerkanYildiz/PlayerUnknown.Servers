namespace PlayerUnknown.Lobby.Services.Api
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    using PlayerUnknown.Lobby.Models.Sessions;
    using PlayerUnknown.Logic.Network;

    public static class UserProxyApi
    {
        public static void Ping(Message Message, PubgSession Session)
        {
            // Ping.
        }

        public static void GetInventory(Message Message, PubgSession Session)
        {
            var Callback = new Message(-Message.Identifier, null, true);

            if (Session.IsAuthenticated)
            {
                var Inventory = JsonConvert.SerializeObject(Session.Player.Inventory);

                if (Inventory != null)
                {
                    Callback.SetResult(Result: JObject.Parse(Inventory));
                }
                else
                {
                    Callback.SetResult(Error: "This faggot's inventory is null.");
                }
            }
            else
            {
                Callback.SetResult(Error: "This faggot is not authenticated.");
            }

            Session.Client.SendMessage(Callback);
        }

        public static void GetPartyData(Message Message, PubgSession Session)
        {
            // GetPartyData.
        }

        public static void GetOpenGameInfo(Message Message, PubgSession Session)
        {
            // GetOpenGameInfo.

            Session.Client.SendMessage("[-10012,null,true,{\"Error\":null,\"Result\":{\"OfficialDivisionIds\":[{\"IdType\":\"division\",\"GameType\":\"bro\",\"LeagueType\":\"official\",\"SeasonType\":\"2018-04\",\"RegionType\":\"as\",\"PartyType\":\"solo\"},{\"IdType\":\"division\",\"GameType\":\"bro\",\"LeagueType\":\"official\",\"SeasonType\":\"2018-04\",\"RegionType\":\"as\",\"PartyType\":\"duo\"},{\"IdType\":\"division\",\"GameType\":\"bro\",\"LeagueType\":\"official\",\"SeasonType\":\"2018-04\",\"RegionType\":\"as\",\"PartyType\":\"squad\"},{\"IdType\":\"division\",\"GameType\":\"bro\",\"LeagueType\":\"official\",\"SeasonType\":\"2018-04\",\"RegionType\":\"as\",\"PartyType\":\"solo-fpp\"},{\"IdType\":\"division\",\"GameType\":\"bro\",\"LeagueType\":\"official\",\"SeasonType\":\"2018-04\",\"RegionType\":\"as\",\"PartyType\":\"duo-fpp\"},{\"IdType\":\"division\",\"GameType\":\"bro\",\"LeagueType\":\"official\",\"SeasonType\":\"2018-04\",\"RegionType\":\"as\",\"PartyType\":\"squad-fpp\"},{\"IdType\":\"division\",\"GameType\":\"bro\",\"LeagueType\":\"official\",\"SeasonType\":\"2018-04\",\"RegionType\":\"eu\",\"PartyType\":\"solo\"},{\"IdType\":\"division\",\"GameType\":\"bro\",\"LeagueType\":\"official\",\"SeasonType\":\"2018-04\",\"RegionType\":\"eu\",\"PartyType\":\"duo\"},{\"IdType\":\"division\",\"GameType\":\"bro\",\"LeagueType\":\"official\",\"SeasonType\":\"2018-04\",\"RegionType\":\"eu\",\"PartyType\":\"squad\"},{\"IdType\":\"division\",\"GameType\":\"bro\",\"LeagueType\":\"official\",\"SeasonType\":\"2018-04\",\"RegionType\":\"eu\",\"PartyType\":\"solo-fpp\"},{\"IdType\":\"division\",\"GameType\":\"bro\",\"LeagueType\":\"official\",\"SeasonType\":\"2018-04\",\"RegionType\":\"eu\",\"PartyType\":\"duo-fpp\"},{\"IdType\":\"division\",\"GameType\":\"bro\",\"LeagueType\":\"official\",\"SeasonType\":\"2018-04\",\"RegionType\":\"eu\",\"PartyType\":\"squad-fpp\"},{\"IdType\":\"division\",\"GameType\":\"bro\",\"LeagueType\":\"official\",\"SeasonType\":\"2018-04\",\"RegionType\":\"oc\",\"PartyType\":\"solo\"},{\"IdType\":\"division\",\"GameType\":\"bro\",\"LeagueType\":\"official\",\"SeasonType\":\"2018-04\",\"RegionType\":\"oc\",\"PartyType\":\"duo\"},{\"IdType\":\"division\",\"GameType\":\"bro\",\"LeagueType\":\"official\",\"SeasonType\":\"2018-04\",\"RegionType\":\"oc\",\"PartyType\":\"squad\"},{\"IdType\":\"division\",\"GameType\":\"bro\",\"LeagueType\":\"official\",\"SeasonType\":\"2018-04\",\"RegionType\":\"oc\",\"PartyType\":\"solo-fpp\"},{\"IdType\":\"division\",\"GameType\":\"bro\",\"LeagueType\":\"official\",\"SeasonType\":\"2018-04\",\"RegionType\":\"oc\",\"PartyType\":\"duo-fpp\"},{\"IdType\":\"division\",\"GameType\":\"bro\",\"LeagueType\":\"official\",\"SeasonType\":\"2018-04\",\"RegionType\":\"oc\",\"PartyType\":\"squad-fpp\"},{\"IdType\":\"division\",\"GameType\":\"bro\",\"LeagueType\":\"official\",\"SeasonType\":\"2018-04\",\"RegionType\":\"na\",\"PartyType\":\"solo\"},{\"IdType\":\"division\",\"GameType\":\"bro\",\"LeagueType\":\"official\",\"SeasonType\":\"2018-04\",\"RegionType\":\"na\",\"PartyType\":\"duo\"},{\"IdType\":\"division\",\"GameType\":\"bro\",\"LeagueType\":\"official\",\"SeasonType\":\"2018-04\",\"RegionType\":\"na\",\"PartyType\":\"squad\"},{\"IdType\":\"division\",\"GameType\":\"bro\",\"LeagueType\":\"official\",\"SeasonType\":\"2018-04\",\"RegionType\":\"na\",\"PartyType\":\"solo-fpp\"},{\"IdType\":\"division\",\"GameType\":\"bro\",\"LeagueType\":\"official\",\"SeasonType\":\"2018-04\",\"RegionType\":\"na\",\"PartyType\":\"duo-fpp\"},{\"IdType\":\"division\",\"GameType\":\"bro\",\"LeagueType\":\"official\",\"SeasonType\":\"2018-04\",\"RegionType\":\"na\",\"PartyType\":\"squad-fpp\"},{\"IdType\":\"division\",\"GameType\":\"bro\",\"LeagueType\":\"official\",\"SeasonType\":\"2018-04\",\"RegionType\":\"sa\",\"PartyType\":\"solo\"},{\"IdType\":\"division\",\"GameType\":\"bro\",\"LeagueType\":\"official\",\"SeasonType\":\"2018-04\",\"RegionType\":\"sa\",\"PartyType\":\"duo\"},{\"IdType\":\"division\",\"GameType\":\"bro\",\"LeagueType\":\"official\",\"SeasonType\":\"2018-04\",\"RegionType\":\"sa\",\"PartyType\":\"squad\"},{\"IdType\":\"division\",\"GameType\":\"bro\",\"LeagueType\":\"official\",\"SeasonType\":\"2018-04\",\"RegionType\":\"sa\",\"PartyType\":\"solo-fpp\"},{\"IdType\":\"division\",\"GameType\":\"bro\",\"LeagueType\":\"official\",\"SeasonType\":\"2018-04\",\"RegionType\":\"sa\",\"PartyType\":\"squad-fpp\"},{\"IdType\":\"division\",\"GameType\":\"bro\",\"LeagueType\":\"official\",\"SeasonType\":\"2018-04\",\"RegionType\":\"sea\",\"PartyType\":\"solo\"},{\"IdType\":\"division\",\"GameType\":\"bro\",\"LeagueType\":\"official\",\"SeasonType\":\"2018-04\",\"RegionType\":\"sea\",\"PartyType\":\"duo\"},{\"IdType\":\"division\",\"GameType\":\"bro\",\"LeagueType\":\"official\",\"SeasonType\":\"2018-04\",\"RegionType\":\"sea\",\"PartyType\":\"squad\"},{\"IdType\":\"division\",\"GameType\":\"bro\",\"LeagueType\":\"official\",\"SeasonType\":\"2018-04\",\"RegionType\":\"sea\",\"PartyType\":\"squad-fpp\"}],\"EventModes\":[]}}]");
        }

        public static void GetUserMatchState(Message Message, PubgSession Session)
        {
            var Callback = new Message(-Message.Identifier, null, true);

            if (Session.IsAuthenticated)
            {
                Callback.SetResult(Result: 3);
            }
            else
            {
                Callback.SetResult(Error: "This faggot is not authenticated.");
            }

            Session.Client.SendMessage(Callback);
        }

        public static void GetAnnouncement(Message Message, PubgSession Session)
        {
            var Callback = new Message(-Message.Identifier, null, true);

            if (Session.IsAuthenticated)
            {
                Callback.SetResult(Result: null);
            }
            else
            {
                Callback.SetResult(Error: "This nagger is not authenticated.");
            }

            Session.Client.SendMessage(Callback);
        }

        public static void GetActivatedEvents(Message Message, PubgSession Session)
        {
            var Callback = new Message(-Message.Identifier, null, true);

            if (Session.IsAuthenticated)
            {
                Callback.SetResult(Result: new JArray());
            }
            else
            {
                Callback.SetResult(Error: "This nagger is not authenticated.");
            }

            Session.Client.SendMessage(Callback);
        }

        public static void ChangeInviteAllow(Message Message, PubgSession Session)
        {
            var Callback = new Message(-Message.Identifier, null, true);

            if (Session.IsAuthenticated)
            {
                if (Session.Player.InviteAllow == null)
                {
                    Session.Player.Profile.InviteAllow = "none";
                }
                else
                {
                    if (Session.Player.InviteAllow == "all")
                    {
                        Session.Player.Profile.InviteAllow = "none";
                    }
                    else
                    {
                        Session.Player.Profile.InviteAllow = "all";
                    }
                }

                Callback.SetResult(Result: Session.Player.InviteAllow);
            }
            else
            {
                Callback.SetResult(Error: "This nagger is not authenticated.");
            }

            Session.Client.SendMessage(Callback);
        }
    }
}
