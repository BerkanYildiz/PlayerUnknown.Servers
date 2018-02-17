namespace PlayerUnknown.Lobby.Services.Api
{
    using Newtonsoft.Json;

    using PlayerUnknown.Files;
    using PlayerUnknown.Lobby.Models.Sessions;
    using PlayerUnknown.Logic.Components;
    using PlayerUnknown.Network;

    using WebSocketSharp;

    public static class UserProxyApi
    {
        public static void Ping(MessageEventArgs Args, Message Message)
        {
            // Ping.
        }

        public static void GetInventory(MessageEventArgs Args, Message Message, PubgSession Session)
        {
            // GetInventory.
        }

        public static void GetPartyData(MessageEventArgs Args, Message Message)
        {
            // GetPartyData.
        }

        public static void GetOpenGameInfo(MessageEventArgs Args, Message Message)
        {
            // GetOpenGameInfo.
        }

        public static void GetUserMatchState(MessageEventArgs Args, Message Message)
        {
            // GetUserMatchState.
        }

        public static void GetAnnouncement(MessageEventArgs Args, Message Message)
        {
            // GetAnnouncement.
        }

        public static void GetActivatedEvents(MessageEventArgs Args, Message Message)
        {
            // GetActivatedEvents.
        }
    }
}
