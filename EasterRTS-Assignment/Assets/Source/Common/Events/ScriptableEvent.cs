using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace EasterRts.Common.Events {

    [CreateAssetMenu(fileName = nameof(ScriptableEvent), menuName = "Common/Scriptable Event")]
    public class ScriptableEvent : ScriptableObject {

        private HashSet<EventListener> _listeners;
        private Queue<EventListener> _currentQueue;

        private bool Enabled => !(_listeners is null);
        private bool IsInDispatch => !(_currentQueue is null);

        private void OnEnable() {
            _listeners = new HashSet<EventListener>();
            _currentQueue = null;
        }

        private void OnDisable() {
            _listeners = null;
            if (IsInDispatch) {
                Debug.LogError("Event is disabled in the middle of dispatching!");
            }
            _currentQueue = null;
        }

        public void AddNonEmptyListener(EventListener listener) {
            if (!Enabled) {
                Debug.LogError($"Can't add listener when disabled!");
                return;
            }
            if (listener is null || listener.IsEmpty) {
                return;
            }
            if (_listeners.Add(listener) && IsInDispatch) {
                _currentQueue.Enqueue(listener);
            }
        }

        public void RemoveEmptyListener(EventListener listener) {
            if (!Enabled || listener is null || !listener.IsEmpty) {
                return;
            }
            _listeners.Remove(listener);
        }

        public void Dispatch() {
            if (!Enabled) {
                Debug.LogError($"Can't be dispatched when disabled!");
                return;
            }
            var queue = new Queue<EventListener>(_listeners);
            _currentQueue = queue;
            while (queue.Count > 0) {
                var listener = queue.Dequeue();
                listener.Message();
            }
            _currentQueue = null;
        }
    }
}
