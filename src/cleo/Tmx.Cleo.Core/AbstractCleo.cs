using Tmx.Cleo.Core.Exceptions;

namespace Tmx.Cleo.Core
{
    /// <summary>
    /// This class contains foundational logic for CLEOs.
    /// </summary>
    public abstract class AbstractCleo<TCleo> : ICleo
        where TCleo : class
    {
        /// <inheritdoc cref="ICleo.GrabChain{TChainLink}"/>
        public TChain GrabChain<TChain>()
            where TChain : class
        {
            // Build the chain
            TChain result = ChainBuilder.BuildChain<TChain>(GrabAttachmentChainLink<TChain>(), this);
            return result;
        }

        /// <summary>
        /// This method must be implemented in a derived CLEO-class and return the final chain link,
        /// where <see cref="GrabChain{TChainLink}"/> can attach the build chain to.
        /// </summary>
        /// <typeparam name="TChain">
        /// This type defines the interface that the chain to be attached supportes.
        /// Thus the returned attachment chain link must be compatible with this.
        /// </typeparam>
        /// <returns>
        /// The reference to the attachment (which is the innermost) chain link wher the
        /// built chain can be attached to.
        /// </returns>
        /// <exception cref="ChainTypeNotSupportedException">
        /// Will be thrown, if the CLEO doesn't support a chain of type <typeparamref name="TChain"/>.
        /// </exception>
        protected abstract TChain GrabAttachmentChainLink<TChain>()
            where TChain : class;
    }
}
