namespace PlayerUnknown.Files.Traductions
{
    using System.Collections.Generic;
    using System.IO;

    using Newtonsoft.Json.Linq;

    using PlayerUnknown.Files.Traductions.Values;

    public class ItemTranslations
    {
        /// <summary>
        /// Gets the translations.
        /// </summary>
        public Dictionary<string, Dictionary<string, ItemTranslation>> Translations
        {
            get;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ItemTranslations"/> class.
        /// </summary>
        public ItemTranslations()
        {
            var TranslationFiles = Directory.GetFiles(@"Gamefiles\translations\items\", "*.json");

            if (TranslationFiles.Length > 0)
            {
                this.Translations = new Dictionary<string, Dictionary<string, ItemTranslation>>(TranslationFiles.Length);

                foreach (var TranslationFile in TranslationFiles)
                {
                    string Language = Path.GetFileNameWithoutExtension(TranslationFile);

                    if (string.IsNullOrEmpty(Language) == false)
                    {
                        var Content = File.ReadAllText(TranslationFile);
                        var Json    = JObject.Parse(Content);

                        if (Json != null && Json.HasValues)
                        {
                            this.Translations.Add(Language, new Dictionary<string, ItemTranslation>());

                            foreach (var Item in Json)
                            {
                                var Translation = new ItemTranslation();
                                Translation.Load(Item.Value.ToObject<JObject>());
                                this.Translations[Language].Add(Item.Key, Translation);
                            }
                        }
                        else
                        {
                            Log.Warning(this.GetType(), "Json is either null or don't have values at ItemTranslations().");
                        }
                    }
                    else
                    {
                        Log.Warning(this.GetType(), "Language key is empty at ItemTranslations().");
                    }
                }
            }
            else
            {
                Log.Warning(this.GetType(), "TranslationFiles.Length <= 0 at ItemTranslations().");
            }
        }

        /// <summary>
        /// Translates the specified key.
        /// </summary>
        /// <param name="Language">The language.</param>
        /// <param name="Key">The key.</param>
        public ItemTranslation Translate(string Language, string Key)
        {
            if (this.Translations.ContainsKey(Language))
            {
                if (this.Translations[Language].TryGetValue(Key, out ItemTranslation Translation))
                {
                    return Translation;
                }
                else
                {
                    Log.Warning(this.GetType(), "Couldn't translate {" + Key +"} to " + Language + ".");
                }
            }
            else
            {
                Log.Warning(this.GetType(), "Couldn't translate {" + Key + "} to " + Language + ".");
            }

            return null;
        }

        /// <summary>
        /// Tries to translate the specified key.
        /// </summary>
        /// <param name="Key">The key.</param>
        public bool TryTranslate(string Language, string Key, out ItemTranslation Translation)
        {
            if (this.Translations.ContainsKey(Language))
            {
                if (this.Translations[Language].TryGetValue(Key, out Translation))
                {
                    return true;
                }
                else
                {
                    Log.Warning(this.GetType(), "Couldn't translate {" + Key + "} to " + Language + ".");
                }
            }
            else
            {
                Log.Warning(this.GetType(), "Couldn't translate {" + Key + "} to " + Language + ".");
            }

            Translation = null;

            return false;
        }
    }
}
