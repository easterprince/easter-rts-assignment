using UnityEngine;

namespace EasterRts.Common {
    
    public abstract class DependentMonoBehaviour : MonoBehaviour {
         
        // Fields.

        private bool _started = false;


        // Life cycle.

        public void Start() {
            if (_started) {
                return;
            }
            _started = true;
            OnStart();
        }

        protected abstract void OnStart();
    }
}
