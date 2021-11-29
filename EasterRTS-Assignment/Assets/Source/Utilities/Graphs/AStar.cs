using EasterRts.Utilities.FixedFloats;
using System;
using System.Collections.Generic;

namespace EasterRts.Utilities.Graphs {
    
    public static class AStar {

        private struct QueueEntry<TVertex, TArc> : IComparable<QueueEntry<TVertex, TArc>> {

            public FixedSingle estimatedFullDistance;
            public FixedSingle currentDistance;
            public int index;
            public TVertex vertex;
            public bool processed;
            public TVertex from;
            public TArc arc;

            public int CompareTo(QueueEntry<TVertex, TArc> other) {
                if (estimatedFullDistance != other.estimatedFullDistance) {
                    return estimatedFullDistance - other.estimatedFullDistance < 0 ? -1 : 1;
                }
                if (index != other.index) {
                    return index - other.index < 0 ? -1 : 1;
                }
                return 0;
            }
        }

        public static List<TArc> FindShortestPath<TVertex, TArc>(TVertex from, TVertex to)
            where TVertex : class, IPositionedGraphVertex<TVertex, TArc>
            where TArc : IGraphArc<TVertex> {

            if (from == null || to == null) {
                return null;
            }

            // Run vertices queue.
            var index = 0;
            var startingEntry = new QueueEntry<TVertex, TArc> {
                estimatedFullDistance = from.EstimateDistance(to),
                currentDistance = 0,
                index = index++,
                vertex = from,
                processed = false,
            };
            var vertexToEntry = new Dictionary<TVertex, QueueEntry<TVertex, TArc>>() {
                [from] = startingEntry
            };
            var queue = new SortedSet<QueueEntry<TVertex, TArc>> {
                startingEntry
            };
            while (queue.Count > 0) {
                var entry = queue.Min;
                queue.Remove(entry);
                entry.processed = true;
                vertexToEntry[entry.vertex] = entry;
                foreach (var arc in entry.vertex.Adjacent) {
                    if (arc == null || arc.To == null || !arc.Distance.IsValid) {
                        continue;
                    }
                    if (arc.Distance < 0) {
                        throw new InvalidOperationException($"Encountered negative arc distance - algorithm is not applicable!");
                    }

                    // Update entry for adjacent vertex.
                    var updateAdjacent = false;
                    var newDistance = entry.currentDistance + arc.Distance;
                    if (!vertexToEntry.TryGetValue(arc.To, out var adjacentEntry)) {
                        updateAdjacent = true;
                    } else if (!adjacentEntry.processed && newDistance < adjacentEntry.currentDistance) {
                        queue.Remove(adjacentEntry);
                        updateAdjacent = true;
                    }
                    if (updateAdjacent) {
                        adjacentEntry = new QueueEntry<TVertex, TArc> {
                            estimatedFullDistance = newDistance + arc.To.EstimateDistance(to),
                            currentDistance = newDistance,
                            index = index++,
                            vertex = arc.To,
                            processed = false,
                            from = entry.vertex,
                            arc = arc,
                        };
                        vertexToEntry[arc.To] = adjacentEntry;
                        queue.Add(adjacentEntry);
                    }
                }
            }

            // Restore shortest path.
            if (!vertexToEntry.ContainsKey(to)) {
                return null;
            }
            var path = new List<TArc>();
            var vertex = to;
            while (vertex != from) {
                var entry = vertexToEntry[vertex];
                path.Add(entry.arc);
                vertex = entry.from;
            }
            path.Reverse();
            return path;
        }
    }
}
