using System;

namespace EasterRts.Common.Cores {

    public struct TrustToken {

        // Static properties.

        public static TrustToken Invalid => new TrustToken();


        // Constructor.

        internal TrustToken(ModelBase model) {
            Model = model;
        }


        // Properties.

        internal ModelBase Model { get; }

        internal bool IsValid => Model != null;
    }
}
