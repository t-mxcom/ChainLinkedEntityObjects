using System;

namespace Tmx.Cleo.Core.Exceptions
{
    /// <summary>
    /// This exception can be thrown by a chain link, if any processing
    /// like validation or authorization failed.
    /// </summary>
    [Serializable]
    public class ChainLinkException : Exception
    {
    }
}
