using EasterRts.Utilities.Attributes;
using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace EasterRts.Common.Events {

    [Serializable]
    [OpenedBoxDrawer]
    public sealed class EventListener {

        [SerializeField]
        [Required]
        private ScriptableEvent _listened;

        public bool IsEmpty => _handler is null;

        private Action _handler = null;

        public void AddHandler(Action handler) {
            bool wasEmpty = IsEmpty;
            _handler += handler;
            if (wasEmpty && !IsEmpty) {
                _listened.AddNonEmptyListener(this);
            }
        }

        public void RemoveHandler(Action handler) {
            bool wasEmpty = IsEmpty;
            _handler -= handler;
            if (!wasEmpty && IsEmpty) {
                _listened.RemoveEmptyListener(this);
            }
        }

        public void Message() {
            _handler?.Invoke();
        }
    }
}
