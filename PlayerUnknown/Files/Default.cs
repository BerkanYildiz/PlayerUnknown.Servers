namespace PlayerUnknown.Files
{
    using System.IO;
    using System.Text;

    using Newtonsoft.Json.Linq;

    public sealed class Default
    {
        /// <summary>
        /// Gets the home json file content.
        /// </summary>
        public JObject HomeJson
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the inventory json file content.
        /// </summary>
        public JObject InventoryJson
        {
            get;
            private set;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Default"/> class.
        /// </summary>
        public Default()
        {
            if (Directory.Exists("Gamefiles/"))
            {
                this.ParseHome();
                this.ParseInventory();
            }
            else
            {
                Logging.Error(typeof(Default), "Directory.Exists(Path) != true at Initialize().");
            }
        }

        /// <summary>
        /// Parses the home json file.
        /// </summary>
        public void ParseHome()
        {
            if (File.Exists("Gamefiles/starting_home.json"))
            {
                string RawFile = File.ReadAllText("Gamefiles/starting_home.json", Encoding.UTF8);

                if (!string.IsNullOrEmpty(RawFile))
                {
                    this.HomeJson = JObject.Parse(RawFile);
                }
                else
                {
                    Logging.Error(typeof(Default), "string.IsNullOrEmpty(RawFile) == true at Initialize().");
                }
            }
            else
            {
                Logging.Error(typeof(Default), "File.Exists(Path) != true at Initialize().");
            }
        }

        /// <summary>
        /// Parses the inventory json.
        /// </summary>
        public void ParseInventory()
        {
            if (File.Exists("Gamefiles/inventory.json"))
            {
                string RawFile = File.ReadAllText("Gamefiles/inventory.json", Encoding.UTF8);

                if (!string.IsNullOrEmpty(RawFile))
                {
                    this.InventoryJson = JObject.Parse(RawFile);
                }
                else
                {
                    Logging.Error(typeof(Default), "string.IsNullOrEmpty(RawFile) == true at Initialize().");
                }
            }
            else
            {
                Logging.Error(typeof(Default), "File.Exists(Path) != true at Initialize().");
            }
        }
    }
}