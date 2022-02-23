using Tmx.Cleo.Core.Exceptions;

namespace Tmx.Cleo.Core
{
    /// <summary>
    /// This interface defines all members, that a CLEO has to provide to the public.
    /// </summary>
    public interface ICleo
    {
        /// <summary>
        /// Grabs the chain that supports the type <typeparamref name="TChain"/>.
        /// </summary>
        /// <remarks>
        /// Internally, this method detects, if the CLEO supports a chain of the given type and if yes,
        /// builds the chain with all registered chain links and finally attaches it to the CLEO instance.
        /// </remarks>
        /// <typeparam name="TChain">
        /// This type defines the interface that the grabbed chain must support.
        /// It can be an `interface` or `class`.
        /// </typeparam>
        /// <returns>
        /// The reference to the outermost chain link of the built chain, which can be used by the caller to
        /// perform the supported action.
        /// </returns>
        /// <exception cref="ChainTypeNotSupportedException">
        /// Will be thrown, if the CLEO doesn't support a chain of type <typeparamref name="TChain"/>.
        /// </exception>
        TChain GrabChain<TChain>()
            where TChain: class;
    }
}
