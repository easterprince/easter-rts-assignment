using EasterRts.Utilities;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace EasterRts.Common.Cores {

    public abstract class ModelBase {

        // Calls queue types.

        private abstract class CallsQueue {

            public abstract bool RunCalls();
        }

        private class CallsQueue<TCore, TArgument> : CallsQueue
            where TCore : CoreBase {

            // TODO: Need to replace queue with some other collection that can guarantee that
            // the dequeue'ing order will be the same for any possible enqueue'ing order.
            private ConcurrentQueue<ActionCall<TCore, TArgument>> _queue = new ConcurrentQueue<ActionCall<TCore, TArgument>>();

            public void Enqueue(ActionCall<TCore, TArgument> callData) => _queue.Enqueue(callData);

            public override bool RunCalls() {
                if (!_queue.TryDequeue(out var call)) {
                    return false;
                }
                do {
                    call.core?.CallAction(call.actionId, call.argument);
                } while (_queue.TryDequeue(out call));
                return true;
            }
        }


        // Fields.

        private CoreId _nextId;
        private readonly Dictionary<CoreId, CoreBase> _idToCore;
        private readonly ConcurrentDictionary<Type, CallsQueue> _actionTypeToCallsQueue;
        private Task[] _progressingTasks;
        private readonly IdToCore _idToCoreWrapper;


        // Constructor.

        protected ModelBase(ModelData data) {
            _nextId = data?.NextFreeId ?? CoreId.First;
            _idToCore = new Dictionary<CoreId, CoreBase>();
            _actionTypeToCallsQueue = new ConcurrentDictionary<Type, CallsQueue>();
            _progressingTasks = null;
            _idToCoreWrapper = new IdToCore(this);
        }


        // Properties.

        protected TrustToken Token => new TrustToken(this);
        protected IEnumerable<CoreBase> Cores => _idToCore.Values;
        protected internal IdToCore IdToCore => _idToCoreWrapper;


        // Core interaction methods.

        protected internal CoreBase GetCoreById(CoreId id) {
            if (!_idToCore.TryGetValue(id, out var core)) {
                return null;
            }
            return core;
        }

        internal CoreId RegisterCore(CoreBase core, CoreId preferredId) {
            CoreId id = preferredId;
            if (id.IsUndefined) {
                if (_nextId.IsUndefined) {
                    throw new InvalidOperationException("Can't generate more ids!");
                }
                id = _nextId;
            } else if (_idToCore.ContainsKey(id)) {
                throw new InvalidOperationException("Specified id is already in use!");
            }
            _nextId = CoreId.GetLatter(id.GetNext(), _nextId);
            _idToCore.Add(id, core);
            return id;
        }

        internal void ReleaseCore(CoreBase core) {
            _idToCore.Remove(core.Id);
        }

        internal void PlanAction<TCore, TAction, TArgument>(ActionCall<TCore, TArgument> callData, TrustToken callerToken)
            where TCore : CoreBase {

            if (callData.core == null || !callData.core.Enabled || callData.actionId.IsUndefined) {
                return;
            }
            if (callerToken.Model != this) {
                // TODO: Need to create separate queue for untrusted (i.e. user initiated) calls with invalid tokens.
            }
            if (!_actionTypeToCallsQueue.TryGetValue(typeof(TAction), out var abstractQueue)) {
                abstractQueue = new CallsQueue<TCore, TArgument>();
                abstractQueue = _actionTypeToCallsQueue.GetOrAdd(typeof(TAction), abstractQueue);
            }
            var queue = (CallsQueue<TCore, TArgument>) abstractQueue;
            queue.Enqueue(callData);
        }

        internal void RunCalls() {
            bool called;
            do {
                called = false;
                foreach (var queue in _actionTypeToCallsQueue.Values) {
                    called |= queue.RunCalls();
                }
            } while (called);
        }


        // Progression methods.

        public void StartProgressing() {

            if (_progressingTasks != null) {
                Debug.LogWarning("Progression has been started already!");
                return;
            }

            /*_progressingTasks = Cores
                .Select(core => Task.Run(() => core.Act()))
                .ToArray();*/
            _progressingTasks = RunProgressingTasks();
        }

        private Task[] RunProgressingTasks() {

            Task RunTask(List<CoreBase> cores) {
                cores = new List<CoreBase>(cores);
                return Task.Run(() => {
                    foreach (var core in cores) {
                        core.Act();
                    }
                });
            }

            var tasks = new List<Task>();
            var cores = new List<CoreBase>();
            var enumerator = Cores.GetEnumerator();
            while (true) {
                var quit = false;
                if (enumerator.MoveNext()) {
                    var core = enumerator.Current;
                    cores.Add(core);
                } else {
                    quit = true;
                }
                if (cores.Count > 0 && (quit || cores.Count >= 16)) {
                    tasks.Add(RunTask(cores));
                    cores.Clear();
                }
                if (quit) {
                    break;
                }
            }
            return tasks.ToArray();
        }

        public void FinishProgressing() {

            if (_progressingTasks == null) {
                Debug.LogWarning(
                    $"Should've called {nameof(StartProgressing)}() after previous {nameof(FinishProgressing)}() call - " +
                    $"progression is meant to be calculated asynchronously!"
                );
                StartProgressing();
            }
            Task.WaitAll(_progressingTasks);
            _progressingTasks = null;
            RunCalls();
        }

        public void ProgressSynchronously() {
            
            foreach (var core in Cores) {
                core.Act();
                RunCalls();
            } 
        }
    }
}
