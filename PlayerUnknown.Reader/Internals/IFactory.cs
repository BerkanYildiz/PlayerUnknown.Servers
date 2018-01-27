namespace PlayerUnknown.Reader.Internals
{
    using System;

    /// <summary>
    /// Define a factory for the library.
    /// </summary>
    /// <remarks>At the moment, the factories are just disposable.</remarks>
    public interface IFactory : IDisposable
    {
    }
}