using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EasterRts.Utilities.Collections {
    
    public struct ReadOnlyArray2<TItem> : IEnumerable<TItem>, IEquatable<ReadOnlyArray2<TItem>> {

        // Fields.

        private readonly Array2<TItem> _source;


        // Properties.

        public Vector2Int Size => _source.Size;
        public IEnumerable<Vector2Int> Indexes => _source.Indexes;
        public TItem this[Vector2Int index] => _source[index];
        public TItem this[int x, int y] => _source[x, y];


        // Constructors.

        public ReadOnlyArray2(Array2<TItem> source) {
            _source = source ?? throw new ArgumentNullException(nameof(source));
        }


        // Enumerating methods.

        public IEnumerator<TItem> GetEnumerator() => _source.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => _source.GetEnumerator();


        // Equality checking methods.

        public bool Equals(ReadOnlyArray2<TItem> other) => _source == other._source;

        public override bool Equals(object obj) {
            if (obj is ReadOnlyArray2<TItem> other) {
                return Equals(other);
            }
            return false;
        }

        public override int GetHashCode() => _source.GetHashCode();

        public static bool operator ==(ReadOnlyArray2<TItem> first, ReadOnlyArray2<TItem> second) => first.Equals(second);
        public static bool operator !=(ReadOnlyArray2<TItem> first, ReadOnlyArray2<TItem> second) => !first.Equals(second);
    }
}
