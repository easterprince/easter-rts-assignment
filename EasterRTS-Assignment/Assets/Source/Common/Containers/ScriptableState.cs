using System.Collections.Generic;
using UnityEngine;

namespace EasterRts.Common.Containers {
    
    public abstract class ScriptableState<TState> : ScriptableObject
        where TState : new() {

        private TState _currentState = new TState();
        public TState CurrentState => _currentState;

        private HashSet<StateListener<TState>> _listeners;
        private Queue<StateListener<TState>> _currentQueue;

        protected bool IsInDispatch => !(_currentQueue is null);

        protected virtual void OnEnable() {
            _currentState = new TState();
            _listeners = new HashSet<StateListener<TState>>();
            _currentQueue = null;
        }

        protected virtual void OnDisable() {
            _listeners = null;
            if (IsInDispatch) {
                Debug.LogError("State is disabled in the middle of dispatching!");
            }
            _currentQueue = null;
        }

        public virtual void AddNonEmptyListener(StateListener<TState> listener) {
            if (listener is null || listener.IsEmpty) {
                return;
            }
            if (_listeners.Add(listener) && IsInDispatch) {
                _currentQueue.Enqueue(listener);
            }
        }

        public virtual void RemoveEmptyListener(StateListener<TState> listener) {
            if (listener is null || !listener.IsEmpty) {
                return;
            }
            _listeners.Remove(listener);
        }

        public virtual void Dispatch(TState state) {
            _currentState = state;
            if (_currentState == null) {
                _currentState = new TState();
            }
            var queue = new Queue<StateListener<TState>>(_listeners);
            _currentQueue = queue;
            while (queue.Count > 0) {
                var listener = queue.Dequeue();
                listener.Message(state);
            }
            _currentQueue = null;
        }
    }
}
