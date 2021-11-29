using System;

namespace EasterRts.Common.Cores {

    public abstract class RootCoreBase<TSelf> : CoreBase<TSelf>
        where TSelf : RootCoreBase<TSelf> {

        // Constructors.

        protected RootCoreBase(CoreId id, TrustToken token) : base(id, token) {
            if (!(this is TSelf)) {
                throw new InvalidOperationException($"Constructed instance must be of type {typeof(TSelf).Name} (generic parameter {nameof(TSelf)})!");
            }
        }
    }
}
