namespace PlayerUnknown.Files
{
    using PlayerUnknown.Files.Traductions;

    public class Translations
    {
        /// <summary>
        /// Gets the available types for translations.
        /// </summary>
        public enum TranslationType
        {
            Default,
            Items,
            Ui,
            Common
        }

        /// <summary>
        /// Gets the translations for the items.
        /// </summary>
        public ItemTranslations Items
        {
            get;
        }

        /// <summary>
        /// Gets the translations for the ui.
        /// </summary>
        public UiTranslations Ui
        {
            get;
        }

        /// <summary>
        /// Gets the translations for the commons.
        /// </summary>
        public CommonTranslations Common
        {
            get;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Translations"/> class.
        /// </summary>
        public Translations()
        {
            this.Items  = new ItemTranslations();
            this.Ui     = new UiTranslations();
            this.Common = new CommonTranslations();
        }
    }
}
