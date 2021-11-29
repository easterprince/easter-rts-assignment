using EasterRts.Utilities.Attributes;
using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace EasterRts.Common.Events {

    [Serializable]
    [OpenedBoxDrawer]
    public sealed class EventListener<TData> {

        [SerializeField]
        [Required]
        private ScriptableEvent<TData> _listened;

        public bool IsEmpty => _handler is null;

        private Action<TData> _handler = null;

        public void AddHandler(Action<TData> handler) {
            bool wasEmpty = IsEmpty;
            _handler += handler;
            if (wasEmpty && !IsEmpty) {
                _listened.AddNonEmptyListener(this);
            }
        }

        public void RemoveHandler(Action<TData> handler) {
            bool wasEmpty = IsEmpty;
            _handler -= handler;
            if (!wasEmpty && IsEmpty) {
                _listened.RemoveEmptyListener(this);
            }
        }

        public void Message(TData data) {
            _handler?.Invoke(data);
        }
    }
}
