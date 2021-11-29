using EasterRts.Battle.Data.Units;
using EasterRts.Common.Cores;
using EasterRts.Utilities.Collections;
using Sirenix.OdinInspector;
using System.Linq;

namespace EasterRts.Battle.Cores.Units {

    public class UnitSystem : DependentCoreBase<UnitSystem, BattleCore> {


        // Action fields.

        private static readonly UnitAddition _unitAddition = new UnitAddition();
        private class UnitAddition : ActionBase<UnitSystem, UnitData> {
            public override void Call(UnitSystem core, UnitData unitData) => core.AddUnit(unitData);
        }

        private static readonly UnitRemoval _unitRemoval = new UnitRemoval();
        private class UnitRemoval : ActionBase<UnitSystem, UnitCore> {
            public override void Call(UnitSystem core, UnitCore unit) => core.RemoveUnit(unit);
        }


        // Static constructor.

        static UnitSystem() {
            RegisterAction(_unitAddition);
            RegisterAction(_unitRemoval);
        }


        // Data fields.

        private readonly OrderedSet<UnitCore> _units;


        // Life cycle.

        internal UnitSystem(UnitSystemData data, BattleCore context) : base(context, CoreId.Undefined) {
            _units = data.Units
                .Select(unitData => new UnitCore(unitData, this))
                .ToOrderedSet();
        }

        internal void Initialize(UnitSystemData data) {
            foreach (var unitData in data.Units) {
                var unit = IdToCore.Get<UnitCore>(unitData.Id);
                unit.Initialize(unitData);
            }
        }


        // Properties.

        [ShowInInspector]
        [ReadOnly]
        public ImmutableArray<UnitCore> Units => _units.ToImmutableArray();


        // Progression methods.

        protected override void Act() {}


        // Action methods.

        internal void AddUnit(UnitData unitData) {
            if (unitData == null) {
                return;
            }
            var unit = new UnitCore(unitData, this);
            unit.Initialize(unitData);
            _units.Add(unit);
        }

        public void PlanUnitAddition(UnitData unitData, TrustToken callerToken) =>
            PlanAction(_unitAddition, unitData, callerToken);

        internal void RemoveUnit(UnitCore unit) {
            if (unit == null) {
                return;
            }
            if (_units.Remove(unit)) {
                unit.Disable();
            }
        }

        public void PlanUnitRemoval(UnitCore unitCore, TrustToken callerToken) =>
            PlanAction(_unitRemoval, unitCore, callerToken);
    }
}
