using System;

namespace Tmx.Cleo.Core.Exceptions
{
    /// <summary>
    /// This exception will be thrown by CLEOs, if the requested chain type
    /// is not supported.
    /// </summary>
    [Serializable]
    public class ChainTypeNotSupportedException: Exception
    {
    }
}
