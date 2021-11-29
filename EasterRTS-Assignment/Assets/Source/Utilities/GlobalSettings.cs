using System;
using UnityEngine;

namespace EasterRts.Utilities {

    [CreateAssetMenu(menuName = "Common/Global Settings")]
    public class GlobalSettings : ScriptableObject {

        private static GlobalSettings _instance;

        [SerializeField]
        private bool _enableAsynchronousExecution;
        public static bool EnableAsynchronousExecution => _instance._enableAsynchronousExecution;

        private void OnEnable() {
            if (_instance != null && _instance != this) {
                Debug.LogError($"Multiple {nameof(GlobalSettings)} instances are detected!");
                return;
            }
            _instance = this;
            if (!Application.isEditor) {
                var arguments = Environment.GetCommandLineArgs();
                foreach (var argument in arguments) {
                    switch (argument) {
                        case "-sync":
                            _enableAsynchronousExecution = false;
                            break;
                        case "-async":
                            _enableAsynchronousExecution = true;
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
