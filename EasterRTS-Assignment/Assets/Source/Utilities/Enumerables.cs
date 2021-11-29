using System.Collections.Generic;
using UnityEngine;

namespace EasterRts.Utilities {
    
    public static class Enumerables {

        public static IEnumerable<Vector2Int> InRange2(Vector2Int limits) {
            var vector = Vector2Int.zero;
            for (vector.x = 0; vector.x < limits.x; ++vector.x) {
                for (vector.y = 0; vector.y < limits.y; ++vector.y) {
                    yield return vector;
                }
            }
        }

        public static IEnumerable<Vector2Int> InRange2(Vector2Int lower, Vector2Int upper) {
            var vector = Vector2Int.zero;
            for (vector.x = lower.x; vector.x < upper.x; ++vector.x) {
                for (vector.y = lower.y; vector.y < upper.y; ++vector.y) {
                    yield return vector;
                }
            }
        }

        public static IEnumerable<Vector2Int> InSegment2(Vector2Int lower, Vector2Int upper) {
            var vector = Vector2Int.zero;
            for (vector.x = lower.x; vector.x <= upper.x; ++vector.x) {
                for (vector.y = lower.y; vector.y <= upper.y; ++vector.y) {
                    yield return vector;
                }
            }
        }

        public static Vector2Int ClampInRange2(Vector2Int index, Vector2Int limits) {
            return new Vector2Int(
                x: Mathf.Clamp(index.x, 0, limits.x - 1),    
                y: Mathf.Clamp(index.y, 0, limits.y - 1)
            );
        }
    }
}
