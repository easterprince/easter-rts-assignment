using System;
using UnityEngine;

namespace EasterRts.Utilities {

    [CreateAssetMenu(menuName = "Common/Global Settings")]
    public class GlobalSettings : ScriptableObject {

        private const int _unitsNumberLimit = 32000;

        private static GlobalSettings _instance;

        [SerializeField]
        private bool _enableAsynchronousExecution;
        public static bool EnableAsynchronousExecution => _instance._enableAsynchronousExecution;

        [SerializeField]
        private int _unitsNumber = -1;
        public static int AllowedUnitsNumber {
            get {
                var number = _instance._unitsNumber;
                if (number < 0) {
                    return _unitsNumberLimit;
                }
                return number;
            }
        }

        private void OnEnable() {
            if (_instance != null && _instance != this) {
                Debug.LogError($"Multiple {nameof(GlobalSettings)} instances are detected!");
                return;
            }
            _instance = this;
            if (!Application.isEditor) {
                var arguments = Environment.GetCommandLineArgs();
                for (int i = 1; i < arguments.Length; ++i) {
                    var argument = arguments[i];
                    var modifier = (i < arguments.Length - 1) ? arguments[i + 1] : null;
                    switch (argument) {
                        case "-sync":
                            _enableAsynchronousExecution = false;
                            break;
                        case "-async":
                            _enableAsynchronousExecution = true;
                            break;
                        case "-units":
                            if (modifier == null || !int.TryParse(modifier, out var unitsLimit)) {
                                break;
                            }
                            ++i;
                            _unitsNumber = unitsLimit;
                            break;
                    }
                }
            }
        }

        private void OnDisable() {
            if (_instance != null && _instance == this) {
                _instance = null;
            }
        }
    }
}
