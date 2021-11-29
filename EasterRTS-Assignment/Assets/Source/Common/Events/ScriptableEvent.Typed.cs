using System;
using System.Collections.Generic;
using UnityEngine;

namespace EasterRts.Common.Events {
    
    public abstract class ScriptableEvent<TData> : ScriptableObject {

        private HashSet<EventListener<TData>> _listeners;
        private Queue<EventListener<TData>> _currentQueue;

        protected bool IsInDispatch => !(_currentQueue is null);

        protected virtual void OnEnable() {
            _listeners = new HashSet<EventListener<TData>>();
            _currentQueue = null;
        }

        protected virtual void OnDisable() {
            _listeners = null;
            if (IsInDispatch) {
                Debug.LogError("Event is disabled in the middle of dispatching!");
            }
            _currentQueue = null;
        }

        public virtual void AddNonEmptyListener(EventListener<TData> listener) {
            if (listener is null || listener.IsEmpty) {
                return;
            }
            if (_listeners.Add(listener) && IsInDispatch) {
                _currentQueue.Enqueue(listener);
            }
        }

        public virtual void RemoveEmptyListener(EventListener<TData> listener) {
            if (listener is null || !listener.IsEmpty) {
                return;
            }
            _listeners.Remove(listener);
        }

        public virtual void Dispatch(TData data) {
            var queue = new Queue<EventListener<TData>>(_listeners);
            _currentQueue = queue;
            while (queue.Count > 0) {
                var listener = queue.Dequeue();
                listener.Message(data);
            }
            _currentQueue = null;
        }
    }
}
