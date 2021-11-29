using EasterRts.Utilities.Attributes;
using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace EasterRts.Common.Events {

    [Serializable]
    [OpenedBoxDrawer]
    public sealed class EventDispatcher<TData> {

        [SerializeField]
        [Required]
        private ScriptableEvent<TData> _dispatched;

        public void Dispatch(TData data) {
            if (_dispatched != null) {
                _dispatched.Dispatch(data);
            }
        }
    }
}
