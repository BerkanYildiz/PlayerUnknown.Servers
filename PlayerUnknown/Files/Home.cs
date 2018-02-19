namespace PlayerUnknown.Files
{
    using System.IO;
    using System.Text;

    using Newtonsoft.Json.Linq;

    public sealed class Home
    {
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Home" /> has been already initalized.
        /// </summary>
        public static bool Initalized
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the home json file content.
        /// </summary>
        public static JObject HomeJson;

        /// <summary>
        /// Gets the inventory json file content.
        /// </summary>
        public static JObject InventoryJson;

        /// <summary>
        /// Initializes a new instance of the <see cref="Home" /> class.
        /// </summary>
        public static void Initialize()
        {
            if (Home.Initalized)
            {
                return;
            }

            Home.Initalized = true;

            if (Directory.Exists("Gamefiles/"))
            {
                if (File.Exists("Gamefiles/starting_home.json"))
                {
                    string RawFile = File.ReadAllText("Gamefiles/starting_home.json", Encoding.UTF8);

                    if (!string.IsNullOrEmpty(RawFile))
                    {
                        Home.HomeJson = JObject.Parse(RawFile);
                    }
                    else
                    {
                        Logging.Error(typeof(Home), "string.IsNullOrEmpty(RawFile) == true at Initialize().");
                    }
                }
                else
                {
                    Logging.Error(typeof(Home), "File.Exists(Path) != true at Initialize().");
                }

                if (File.Exists("Gamefiles/inventory.json"))
                {
                    string RawFile = File.ReadAllText("Gamefiles/inventory.json", Encoding.UTF8);

                    if (!string.IsNullOrEmpty(RawFile))
                    {
                        Home.InventoryJson = JObject.Parse(RawFile);
                    }
                    else
                    {
                        Logging.Error(typeof(Home), "string.IsNullOrEmpty(RawFile) == true at Initialize().");
                    }
                }
                else
                {
                    Logging.Error(typeof(Home), "File.Exists(Path) != true at Initialize().");
                }
            }
            else
            {
                Logging.Error(typeof(Home), "Directory.Exists(Path) != true at Initialize().");
            }
        }
    }
}