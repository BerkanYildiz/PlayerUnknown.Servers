namespace PlayerUnknown.Interfaces
{
    using Newtonsoft.Json.Linq;

    public interface IPlayer
    {
        /// <summary>
        /// Gets or sets the profile.
        /// </summary>
        IProfile Profile
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the inventory.
        /// </summary>
        IInventory Inventory
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the record.
        /// </summary>
        IRecord Record
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the account.
        /// </summary>
        IAccount Account
        {
            get;
            set;
        }
        
        /// <summary>
        /// Gets or sets the username.
        /// </summary>
        string Username
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        string Password
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the invite allow.
        /// </summary>
        string InviteAllow
        {
            get;
        }

        /// <summary>
        /// Gets the playing game.
        /// </summary>
        string PlayingGame
        {
            get;
        }

        /// <summary>
        /// Gets the avatar URL.
        /// </summary>
        string AvatarUrl
        {
            get;
        }

        /// <summary>
        /// Gets the lobby application configuration.
        /// </summary>
        object LobbyAppConfig
        {
            get;
        }

        /// <summary>
        /// Saves this instance into a json object.
        /// </summary>
        JObject ToJson();
    }
}
