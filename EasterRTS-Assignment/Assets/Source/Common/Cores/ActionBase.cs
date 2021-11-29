namespace EasterRts.Common.Cores {

    public abstract class ActionBase<TCore>
        where TCore : CoreBase<TCore> {

        // Fields.

        private ActionId _id = ActionId.Undefined;
        public ActionId Id {
            get => _id;
            internal set => _id = value;
        }
    }

    public abstract class ActionBase<TCore, TArgument> : ActionBase<TCore>
        where TCore : CoreBase<TCore> {

        // Methods.

        public abstract void Call(TCore core, TArgument argument);
    }
}
