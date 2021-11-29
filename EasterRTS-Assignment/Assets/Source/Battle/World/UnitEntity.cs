using EasterRts.Battle.Cores.Units;
using EasterRts.Common;
using EasterRts.Common.Events;
using Sirenix.OdinInspector;
using UnityEngine;

namespace EasterRts.Battle.World {

    public class UnitEntity : DependentMonoBehaviour {

        // Fields.

        [SerializeField]
        private EventListener _worldUpdateListener;

        [SerializeField]
        private BattleLauncher _battleLauncher;

        private BattleContext _battleContext;

        [ShowInInspector]
        [ReadOnly]
        private UnitCore _core;
        public UnitCore Core => _core;


        // Methods.

        public void SetCore(UnitCore core) {
            _core = core;
        }


        // Life cycle.

        protected override void OnStart() {
            _battleContext = _battleLauncher.Data;
        }

        protected virtual void OnEnable() {
            _worldUpdateListener.AddHandler(OnWorldUpdate);
        }

        protected virtual void OnWorldUpdate() {
            var timeDelta = Time.time - _battleContext.LastModelUpdateTime;
            var currentPosition = _core.Movement.Location.Position + (Vector2) _core.Movement.Velocity * timeDelta;
            transform.position = new Vector3(currentPosition.x, 0, currentPosition.y);
        }

        protected virtual void OnDisable() {
            _worldUpdateListener.RemoveHandler(OnWorldUpdate);
        }
    }
}
