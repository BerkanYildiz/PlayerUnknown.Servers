namespace PlayerUnknown.Logic.Interfaces.Players
{
    public interface ICurrency : IObject
    {
        /// <summary>
        /// Gets the currency identifier.
        /// </summary>
        string CurrencyId
        {
            get;
        }

        /// <summary>
        /// Gets the amount.
        /// </summary>
        int Amount
        {
            get;
        }
    }
}
