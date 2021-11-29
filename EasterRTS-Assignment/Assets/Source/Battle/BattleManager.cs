using EasterRts.Battle.Cores;
using EasterRts.Common;
using EasterRts.Common.Events;
using EasterRts.Utilities;
using EasterRts.Utilities.FixedFloats;
using Sirenix.OdinInspector;
using System.Linq;
using UnityEngine;

namespace EasterRts.Battle {

    public class BattleManager : SceneManagerBase {

        // Configurable fields.

        [SerializeField]
        private BattleLauncher _launcher;

        [SerializeField]
        private EventDispatcher _uiUpdateDispatcher;

        [SerializeField]
        private EventDispatcher _worldUpdateDispatcher;


        // Internal fields.

        [ShowInInspector]
        [ReadOnly]
        private BattleModel _model;

        [ShowInInspector]
        [ReadOnly]
        private float _lastModelUpdateTime;
        public float LastModelUpdateTime => _lastModelUpdateTime;

        private float _coreUpdateTimeAccumulated;


        // Life cycle.

        private void Start() {
            if (_launcher.Data == null) {
                Debug.Log("No scene data provided!");
            } else {
                Debug.Log("Scene data is provided.");
                _model = _launcher.Data.RetrieveBattleModel(this);
            }
            if (GlobalSettings.EnableAsynchronousExecution) {
                _model.StartProgressing();
            }
            _coreUpdateTimeAccumulated = 0;
            _lastModelUpdateTime = Time.time;
        }

        private void Update() {

            // Update core.
            _coreUpdateTimeAccumulated += Time.deltaTime;
            if (_model.Core.TimeDelta > 0) {
                while (_coreUpdateTimeAccumulated >= (float) _model.Core.TimeDelta) {

                    _coreUpdateTimeAccumulated -= (float) _model.Core.TimeDelta;
                    if (GlobalSettings.EnableAsynchronousExecution) {
                        _model.FinishProgressing();
                        _model.StartProgressing();
                    } else {
                        _model.ProgressSynchronously();
                    }
                    _lastModelUpdateTime = Time.time;
                }
            } else {
                Debug.LogError($"Battle core has invalid time delta ({_model.Core.TimeDelta}). Can't update core!");
            }

            // Update world.
            _worldUpdateDispatcher.Dispatch();

            // Update UI.
            _uiUpdateDispatcher.Dispatch();
        }
    }
}
