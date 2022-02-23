using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tmx.Cleo.Core
{
    public static class ChainBuilder
    {
        private class ChainLinkRegistryItem
        {
            private readonly Tuple<Type, Type, Type> tuple;

            public Type ChainType => tuple.Item1;
            public Type CleoType => tuple.Item2;
            public Type ChainLinkType => tuple.Item3;

            public ChainLinkRegistryItem(
                Type chainType,
                Type cleoType,
                Type chainLinkType)
            {
                tuple = new Tuple<Type, Type, Type>(chainType, cleoType, chainLinkType);
            }
        }

        private static readonly IList<ChainLinkRegistryItem> chainLinkRegistry =
            new List<ChainLinkRegistryItem>();

        public static void RegisterChainLink<TChain, TCleo, TChainLink>()
        {
            chainLinkRegistry.Add(
                new ChainLinkRegistryItem(typeof(TChain), typeof(TCleo), typeof(TChainLink)));
        }

        public static TChain BuildChain<TChain>(TChain attachmentChainLink, ICleo cleo)
            where TChain : class
        {
            IEnumerable<ChainLinkRegistryItem> chainLinks =
                chainLinkRegistry
                    .Where(clri => (clri.ChainType == typeof(TChain)) && ((clri.CleoType == null) || (clri.CleoType == cleo?.GetType())));

            TChain result = attachmentChainLink;
            foreach (ChainLinkRegistryItem chainLink in chainLinks)
            {
                result = (TChain) Activator.CreateInstance(
                    chainLink.ChainLinkType,
                    new object[] { result });
            }

            return result;
        }
    }
}
