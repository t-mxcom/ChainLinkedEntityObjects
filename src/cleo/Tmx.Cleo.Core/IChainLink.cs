namespace Tmx.Cleo.Core
{
    /// <summary>
    /// This interface defines all members, that chain links have to provide to the public.
    /// </summary>
    public interface IChainLink
    {
        /// <summary>
        /// Gets the <see cref="ChainLinkCategory"/> of the chain link.
        /// </summary>
        ChainLinkCategory Category { get; }

        /// <summary>
        /// Gets the name of the chain link.
        /// </summary>
        string Name { get; }
    }
}
