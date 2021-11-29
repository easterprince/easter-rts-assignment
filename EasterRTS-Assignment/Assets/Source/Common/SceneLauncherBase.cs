using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace EasterRts.Common {

    public abstract class SceneLauncherBase<TData> : ScriptableObject
        where TData : class {

        [SerializeField]
        private string _sceneName;

        [NonSerialized]
        private bool _transitioning = false;

        [NonSerialized]
        private TData _data = null;
        public TData Data => _data;

        public void StartLoading(TData data = null) {
            if (_transitioning) {
                Debug.LogError("Scene is already loading!");
                return;
            }
            _transitioning = true;
            _data = data;
            Debug.Log($"Started loading \"{_sceneName}\" scene.");
        }

        public void FinishLoading() {
            if (!_transitioning) {
                Debug.LogError("Scene is not loading!");
                return;
            }
            _transitioning = false;
            SceneManager.LoadScene(_sceneName);
            Debug.Log($"Loaded \"{_sceneName}\" scene successfully.");
        }
    }
}
