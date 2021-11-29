using EasterRts.Utilities;
using System;
using System.IO;

namespace EasterRts.Common.Cores {

    internal struct ActionCall<TCore, TArgument>
        where TCore : CoreBase {

        public TCore core;
        public ActionId actionId;
        public TArgument argument;
    }
}