namespace PlayerUnknown.Reader.Internals
{
    /// <summary>
    /// Defines a element with a name.
    /// </summary>
    public interface INamedElement : IApplicableElement
    {
        /// <summary>
        /// The name of the element.
        /// </summary>
        string Name
        {
            get;
        }
    }
}