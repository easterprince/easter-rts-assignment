using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace EasterRts.Utilities.Collections {
    
    public class ImmutableArray<TItem> : IReadOnlyList<TItem> {

        // Fields.

        private readonly TItem[] _items;
        

        // Constructor.

        public ImmutableArray() {
            _items = new TItem[0];
        }

        public ImmutableArray(TItem[] items) {
            _items = new TItem[items.Length];
            Array.Copy(items, _items, items.Length);
        }

        public ImmutableArray(IEnumerable<TItem> items) {
            _items = items.ToArray() ?? throw new ArgumentNullException(nameof(items));
        }


        // Properties.

        public int Count => _items.Length;


        // Indexator.

        public TItem this[int index] => _items[index];


        // Methods.

        public IEnumerator<TItem> GetEnumerator() {
            foreach (var item in _items) {
                yield return item;
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public void CopyTo(long sourceIndex, TItem[] destinationArray, long destinationIndex, long length) =>
            Array.Copy(_items, sourceIndex, destinationArray, destinationIndex, length);
    }
}
