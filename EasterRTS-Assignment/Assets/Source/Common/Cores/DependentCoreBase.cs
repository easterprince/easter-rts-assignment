using System;

namespace EasterRts.Common.Cores {

    public abstract class DependentCoreBase<TSelf, TContext> : CoreBase<TSelf>
        where TSelf : DependentCoreBase<TSelf, TContext>
        where TContext : CoreBase<TContext> {

        // Constructors.

        protected DependentCoreBase(TContext context, CoreId id) : base(id, context?.Token ?? TrustToken.Invalid) {
            if (!(this is TSelf)) {
                throw new InvalidOperationException($"Constructed instance must be of type {typeof(TSelf).Name} (generic parameter {nameof(TSelf)})!");
            }
            Context = context;
        }


        // Properties.

        public TContext Context { get; private set; }
    }
}
