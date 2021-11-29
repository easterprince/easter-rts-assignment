using EasterRts.Utilities.Attributes;
using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace EasterRts.Common.Events {

    [Serializable]
    [OpenedBoxDrawer]
    public sealed class EventDispatcher {

        [SerializeField]
        [Required]
        private ScriptableEvent _dispatched;

        public void Dispatch() {
            if (_dispatched != null) {
                _dispatched.Dispatch();
            }
        }
    }
}
