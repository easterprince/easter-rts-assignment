using EasterRts.Utilities.Attributes;
using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace EasterRts.Common.Containers {

    [Serializable]
    [OpenedBoxDrawer]
    public sealed class StateDispatcher<TState>
        where TState : new() {

        [SerializeField]
        [Required]
        private ScriptableState<TState> _dispatched;

        public void Dispatch(TState state) {
            if (_dispatched != null) {
                _dispatched.Dispatch(state);
            }
        }
    }
}
