using EasterRts.Utilities;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;

namespace EasterRts.Common.Cores {

    public abstract class CoreBase {

        // Fields.

        [ShowInInspector]
        private CoreId _id;

        private ModelBase _model;


        // Constructor.

        private protected CoreBase(CoreId id, TrustToken trustToken) {
            if (!trustToken.IsValid) {
                throw new ArgumentException("Trust token must be valid!", nameof(trustToken));
            }
            Model = trustToken.Model;
            Id = Model.RegisterCore(this, id);
        }


        // Properties.

        public CoreId Id {
            get => _id;
            private set => _id = value;
        }
        
        internal protected ModelBase Model {
            get => _model;
            private set => _model = value;
        }

        public bool Enabled => !Id.IsUndefined;
        protected internal TrustToken Token => new TrustToken(Model);
        protected IdToCore IdToCore => Model.IdToCore;


        // Life cycle methods.

        protected void DisableSelf() {
            if (Enabled) {
                Model.ReleaseCore(this);
                Model = null;
                Id = CoreId.Undefined;
            }
        }


        // Progression methods.

        internal protected abstract void Act();

        internal abstract void CallAction<TArgument>(ActionId id, TArgument callArgument);
    }

    public abstract class CoreBase<TSelf> : CoreBase
        where TSelf : CoreBase<TSelf> {

        // Static fields.

        private static readonly List<ActionBase<TSelf>> _actions = new List<ActionBase<TSelf>>();


        // Constructor.

        private protected CoreBase(CoreId id, TrustToken token) : base(id, token) {
            if (!(this is TSelf)) {
                throw new InvalidOperationException($"Constructed instance must be of type {typeof(TSelf).Name} (generic parameter {nameof(TSelf)})!");
            }
        }

        // Actions pipeline methods.

        protected static void RegisterAction(ActionBase<TSelf> action) {
            _actions.Add(action);
            action.Id = new ActionId(_actions.Count - 1);
        }

        protected void PlanAction<TAction, TArgument>(TAction action, TArgument data, TrustToken callerToken)
            where TAction : ActionBase<TSelf, TArgument> {

            if (!Enabled) {
                return;
            }
            var callData = new ActionCall<TSelf, TArgument> {
                core = this as TSelf,
                actionId = action.Id,
                argument = data,
            };
            Model?.PlanAction<TSelf, TAction, TArgument>(callData, callerToken);
        }

        internal override void CallAction<TArgument>(ActionId id, TArgument argument) {

            var index = id.Value;
            if (index < 0 || index >= _actions.Count) {
                return;
            }
            var action = (ActionBase<TSelf, TArgument>) _actions[index];
            action.Call(this as TSelf, argument);
        }
    }
}