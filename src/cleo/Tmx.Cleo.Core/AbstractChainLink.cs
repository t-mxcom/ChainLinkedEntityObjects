namespace Tmx.Cleo.Core
{
    /// <summary>
    /// This class contains foundational logic for chain links.
    /// </summary>
    public abstract class AbstractChainLink<TChain> : IChainLink
        where TChain : class
    {
        /// <inheritdoc cref="IChainLink.Category"/>
        public ChainLinkCategory Category { get; protected set; }

        /// <inheritdoc cref="IChainLink.Name"/>
        public string Name { get; protected set; }

        /// <summary>
        /// Initializes the chain link.
        /// </summary>
        /// <param name="attachedTo">
        /// [in] Reference to the chain link this one is attached to.
        /// </param>
        protected AbstractChainLink(TChain attachedTo)
        {
            // store the reference to the following chain link
            AttachedTo = attachedTo;
        }

        /// <summary>
        /// Holds the reference to the chain link this one is attached to.
        /// </summary>
        /// <remarks>
        /// Incoming requests must be passed on to this chain link which
        /// may be the attachment chain link of the CLEO.
        /// </remarks>
        protected TChain AttachedTo { get; set; }
    }
}
