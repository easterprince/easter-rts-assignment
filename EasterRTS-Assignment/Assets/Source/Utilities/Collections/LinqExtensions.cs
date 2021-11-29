using System.Collections.Generic;
using UnityEngine;

namespace EasterRts.Utilities.Collections {
    
    public static class LinqExtensions {

        // Conversions.

        public static ImmutableArray<TItem> ToImmutableArray<TItem>(this IEnumerable<TItem> enumerable) =>
            new ImmutableArray<TItem>(enumerable);

        public static OrderedSet<TElement> ToOrderedSet<TElement>(this IEnumerable<TElement> enumerable) {
            return new OrderedSet<TElement>(enumerable);
        }

        public static Array2<TItem> ToArray2<TItem>(this IEnumerable<TItem> source, Vector2Int size) {
            return new Array2<TItem>(source, size);
        }
    }
}
