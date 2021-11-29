using EasterRts.Battle.Cores.Units;
using EasterRts.Common.Events;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace EasterRts.Battle.World {

    public class UnitSystemEntity : MonoBehaviour {

        // Configurable fields.

        [SerializeField]
        private EventListener _worldUpdateListener;

        [SerializeField]
        private BattleLauncher _battleLauncher;

        [SerializeField]
        private UnitEntity _unitPrefab;


        // Internal fields.

        [ShowInInspector]
        [ReadOnly]
        private UnitSystem _core;

        private Dictionary<UnitCore, UnitEntity> _unitCoreToEntity;


        // Life cycle.

        protected void OnEnable() {
            _worldUpdateListener.AddHandler(OnWorldUpdate);
        }

        protected void Start() {
            var battleCore = _battleLauncher.Data.BattleCore;
            _core = battleCore.UnitSystem;
            _unitCoreToEntity = new Dictionary<UnitCore, UnitEntity>();
            foreach (var unitCore in _core.Units) {
                var unitEntity = CreateUnit(unitCore);
                _unitCoreToEntity[unitCore] = unitEntity;
            }
        }

        protected void OnWorldUpdate() {
            var newUnitCoreToEntity = new Dictionary<UnitCore, UnitEntity>();
            foreach (var unitCore in _core.Units) {
                if (!_unitCoreToEntity.TryGetValue(unitCore, out var unitEntity)) {
                    unitEntity = CreateUnit(unitCore);
                }
                newUnitCoreToEntity[unitCore] = unitEntity;
            }
            foreach (var unitCoreAndEntity in _unitCoreToEntity) {
                if (!newUnitCoreToEntity.ContainsKey(unitCoreAndEntity.Key)) {
                    Destroy(unitCoreAndEntity.Value.gameObject);
                }
            }
            _unitCoreToEntity = newUnitCoreToEntity;
        }

        protected void OnDisable() {
            _worldUpdateListener.RemoveHandler(OnWorldUpdate);
        }


        // Supporting methods.

        private UnitEntity CreateUnit(UnitCore unitCore) {
            var unitEntity = Instantiate(_unitPrefab, transform);
            unitEntity.name = $"Unit ({unitCore.Id})";
            unitEntity.Start();
            unitEntity.SetCore(unitCore);
            return unitEntity;
        }
    }
}
