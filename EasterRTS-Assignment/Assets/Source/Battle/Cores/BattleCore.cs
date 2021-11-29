using EasterRts.Battle.Cores.Map;
using EasterRts.Battle.Cores.Units;
using EasterRts.Battle.Data;
using EasterRts.Common.Cores;
using EasterRts.Utilities;
using EasterRts.Utilities.FixedFloats;
using Sirenix.OdinInspector;

namespace EasterRts.Battle.Cores {

    public class BattleCore : RootCoreBase<BattleCore> {

        // Actions.

        private static Progression _progressionAction;
        private class Progression : ActionBase<BattleCore, EmptyStruct> {
            public override void Call(BattleCore core, EmptyStruct argument) => core.Progress();
        }

        static BattleCore() {
            RegisterAction(_progressionAction = new Progression());
        }


        // Data fields.

        [ShowInInspector]
        [ReadOnly]
        private FixedSingle _time;
        public FixedSingle Time => _time;

        [ShowInInspector]
        [ReadOnly]
        private FixedSingle _timeDelta;
        public FixedSingle TimeDelta => _timeDelta;


        // Dependent cores fields.

        [ShowInInspector]
        [ReadOnly]
        private readonly MapSystem _map;
        public MapSystem Map => _map;

        [ShowInInspector]
        [ReadOnly]
        private readonly UnitSystem _unitSystem;
        public UnitSystem UnitSystem => _unitSystem;


        // Constructor.

        internal BattleCore(BattleData data, TrustToken token) : base(CoreId.Undefined, token) {
            _map = new MapSystem(data.Map, this);
            _unitSystem = new UnitSystem(data.UnitSystem, this);
        }

        internal void Initialize(BattleData data) {

            _time = data.StartTime;
            _timeDelta = data.TimeDelta;

            _map.Initialize(data.Map);
            _unitSystem.Initialize(data.UnitSystem);
        }


        // Progression methods.

        protected override void Act() {
            PlanAction(_progressionAction, new EmptyStruct(), Token);
        }


        // Action methods.

        private void Progress() {
            _time += _timeDelta;
        }
    }
}
