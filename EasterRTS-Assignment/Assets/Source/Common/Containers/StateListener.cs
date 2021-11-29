using EasterRts.Utilities.Attributes;
using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace EasterRts.Common.Containers {

    [Serializable]
    [OpenedBoxDrawer]
    public sealed class StateListener<TState>
        where TState : new() {

        [SerializeField]
        [Required]
        private ScriptableState<TState> _listened;

        public bool IsEmpty => _handler is null;

        private Action<TState> _handler = null;

        public TState GetCurrentState() {
            if (_listened == null) {
                return new TState();
            }
            return _listened.CurrentState;
        }

        public void AddHandler(Action<TState> handler) {
            bool wasEmpty = IsEmpty;
            _handler += handler;
            if (wasEmpty && !IsEmpty) {
                _listened.AddNonEmptyListener(this);
            }
        }

        public void RemoveHandler(Action<TState> handler) {
            bool wasEmpty = IsEmpty;
            _handler -= handler;
            if (!wasEmpty && IsEmpty) {
                _listened.RemoveEmptyListener(this);
            }
        }

        public void Message(TState state) {
            _handler?.Invoke(state);
        }
    }
}
