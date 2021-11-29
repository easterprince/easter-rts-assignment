using System.Collections;
using System.Collections.Generic;

namespace EasterRts.Utilities.Collections {

    public class OrderedDictionary<TKey, TValue> : IReadOnlyDictionary<TKey, TValue> {

        // Fields.

        private readonly Dictionary<TKey, LinkedListNode<KeyValuePair<TKey, TValue>>> _mapping = new Dictionary<TKey, LinkedListNode<KeyValuePair<TKey, TValue>>>();
        private readonly LinkedList<KeyValuePair<TKey, TValue>> _order = new LinkedList<KeyValuePair<TKey, TValue>>();


        // Properties.

        public int Count => _mapping.Count;

        public bool IsReadOnly => false;

        public IEnumerable<TKey> Keys {
            get {
                foreach (var pair in _order) {
                    yield return pair.Key;
                }
            }
        }

        public IEnumerable<TValue> Values {
            get {
                foreach (var pair in _order) {
                    yield return pair.Value;
                }
            }
        }


        // Indexator.

        public TValue this[TKey key] {
            get => _mapping[key].Value.Value;
            set {
                var node = _mapping[key];
                node.Value = new KeyValuePair<TKey, TValue>(node.Value.Key, value);
            }
        }


        // Modification methods.

        public bool Add(KeyValuePair<TKey, TValue> pair) {
            if (_mapping.ContainsKey(pair.Key)) {
                return false;
            }
            var node = _order.AddLast(pair);
            _mapping[pair.Key] = node;
            return true;
        }

        public bool Add(TKey key, TValue value) => Add(new KeyValuePair<TKey, TValue>(key, value));

        public bool Remove(TKey key) {
            if (!_mapping.TryGetValue(key, out var node)) {
                return false;
            }
            _mapping.Remove(key);
            _order.Remove(node);
            return true;
        }

        public void Clear() {
            _mapping.Clear();
            _order.Clear();
        }


        // Reading methods.

        public bool ContainsKey(TKey key) => _mapping.ContainsKey(key);

        public bool TryGetValue(TKey key, out TValue value) {
            if (!_mapping.TryGetValue(key, out var node)) {
                value = default;
                return false;
            }
            value = node.Value.Value;
            return true;
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() {
            foreach (var pair in _order) {
                yield return pair;
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
