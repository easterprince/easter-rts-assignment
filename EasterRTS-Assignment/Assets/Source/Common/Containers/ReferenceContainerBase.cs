using UnityEngine;

namespace EasterRts.Common.Containers {

    public abstract class ReferenceContainerBase<TData> : ScriptableObject
        where TData : UnityEngine.Object {

        private bool _set = false;
        public bool Set => _set;

        private TData _reference;
        public TData Reference {
            get => _reference;
            set {
                var valueIsNull = value == null;
                if (_set && !valueIsNull) {
                    Debug.LogWarning("Reference is assigned while old one haven't been unset yet!", this);
                }
                _set = !valueIsNull;
                _reference = value;
            }
        }
    }
}
