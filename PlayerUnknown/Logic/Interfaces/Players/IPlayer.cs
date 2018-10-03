namespace PlayerUnknown.Logic.Interfaces.Players
{
    public interface IPlayer : IObject
    {
        /// <summary>
        /// Gets or sets the profile.
        /// </summary>
        IProfile Profile
        {
            get;
        }

        /// <summary>
        /// Gets or sets the inventory.
        /// </summary>
        IInventory Inventory
        {
            get;
        }

        /// <summary>
        /// Gets or sets the record.
        /// </summary>
        IRecord Record
        {
            get;
        }

        /// <summary>
        /// Gets or sets the account.
        /// </summary>
        IAccount Account
        {
            get;
        }
        
        /// <summary>
        /// Gets the lobby application configuration.
        /// </summary>
        IAppConfig LobbyAppConfig
        {
            get;
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
    }
}
