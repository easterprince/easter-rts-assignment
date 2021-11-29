using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace EasterRts.Utilities.Collections {
    
    [Serializable]
    public sealed class Array2<TItem> : IEnumerable<TItem>, ISerializationCallbackReceiver {

        // Nested types.

        public struct IndexSet : IEnumerable<Vector2Int> {

            private Vector2Int _size;
            public Vector2Int Size => _size;

            public IndexSet(Vector2Int size) {
                _size = size;
            }

            public bool Contains(Vector2Int index) =>
                index.x >= 0 && index.y >= 0 && index.x < _size.x && index.y < _size.y;

            public IEnumerator<Vector2Int> GetEnumerator() {
                Vector2Int index = Vector2Int.zero;
                for (index.x = 0; index.x < _size.x; index.x++) {
                    for (index.y = 0; index.y < _size.y; index.y++) {
                        yield return index;
                    }
                }
            }

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }


        // Fields.

        [SerializeField]
        [ReadOnly]
        private Vector2Int _size;

        [SerializeField]
        [ReadOnly]
        private TItem[] _items;


        // Properties.

        public Vector2Int Size => _size;
        public IndexSet Indexes => new IndexSet(_size);

        public TItem this[Vector2Int index] {
            get => this[index.x, index.y];
            set => this[index.x, index.y] = value;
        }

        public TItem this[int x, int y] {
            get => _items[x * _size.y + y];
            set => _items[x * _size.y + y] = value;
        }


        // Constructors.

        public Array2() {
            _size = Vector2Int.zero;
            _items = new TItem[0];
        }

        public Array2(Vector2Int size) {
            if (size.x < 0 || size.y < 0) {
                throw new ArgumentOutOfRangeException(nameof(size), size, "Components of size must be non-negative!");
            }
            _size = size;
            _items = new TItem[size.x * size.y];
        }

        public Array2(int sizeX, int sizeY) : this(new Vector2Int(sizeX, sizeY)) {} 

        public Array2(IEnumerable<TItem> items, Vector2Int size) {
            _size = size;
            _items = items.ToArray();
            if (_items.Length != _size.x * _size.y) {
                throw new ArgumentException(
                    $"Size must comply with items count, " +
                    $"i.e. product of {size}.x and {size}.y must be equal to {items}.Count()!"
                );
            }
        }

        public Array2(Array2<TItem> other) : this(other.Size) {
            if (other == null) {
                throw new ArgumentNullException(nameof(other));
            }
            other._items.CopyTo(_items, 0);
        }


        // Enumerating methods.

        public IEnumerator<TItem> GetEnumerator() {
            foreach (var index in Indexes) {
                yield return this[index];
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();


        // Serialization callbacks.

        void ISerializationCallbackReceiver.OnBeforeSerialize() { }

        void ISerializationCallbackReceiver.OnAfterDeserialize() {
            
            if (_size.x < 0 || _size.y < 0) {

                Debug.LogWarning(
                    $"Got invalid size ({_size}) after deserialization! " +
                    $"Make sure to fix serialized source."
                );
                _size = Vector2Int.zero;
                _items = new TItem[0];
            
            } else if (_items.Length != _size.x * _size.y) {

                Debug.LogWarning(
                    $"Got invalid internal array length ({_items.Length}) after deserialization: " +
                    $"it doesn't comply with size ({_size}, or {_size.x * _size.y} items)! " +
                    $"Make sure to fix serialized source."
                );
                var fixedItems = new TItem[_size.x * _size.y];
                Array.Copy(_items, fixedItems, Mathf.Min(_items.Length, fixedItems.Length));
                _items = fixedItems;
            }
        }


        // Other methods.

        public Array2<TItem> Copy() => new Array2<TItem>(this);
        public ReadOnlyArray2<TItem> ToReadOnlyArray2() => new ReadOnlyArray2<TItem>(this);
    }
}
