using EasterRts.Battle.Data.Map;
using EasterRts.Common.Cores;
using EasterRts.Utilities.Collections;
using EasterRts.Utilities.FixedFloats;
using EasterRts.Utilities.Graphs;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace EasterRts.Battle.Cores.Map {

    public class MapSite : DependentCoreBase<MapSite, MapSystem>, IPositionedGraphVertex<MapSite, MapSite.Transition> {

        // Nested types.

        public class Transition : IGraphArc<MapSite> {

            private readonly MapSite _neighbour;
            public MapSite Neigbour => _neighbour;

            private readonly FixedSingle _distance;
            public FixedSingle Distance => _distance;

            private readonly CachedFixedSegment2 _edge;
            public CachedFixedSegment2 Edge => _edge;

            MapSite IGraphArc<MapSite>.To => _neighbour;
            FixedSingle IGraphArc<MapSite>.Distance => _distance;

            public Transition(MapSite neighbour, FixedSingle distance, FixedSegment2 edge) {
                _neighbour = neighbour;
                _distance = distance;
                _edge = new CachedFixedSegment2(edge);
            }
        }


        // Constants.

        private static readonly FixedSingle _weakContainingDistance = FixedSingle.FromThousandths(1000);


        // Fields.

        [ShowInInspector]
        private Vector2Int _index;
        public Vector2Int Index => _index;

        private FixedVector2 _generativePoint;
        public FixedVector2 GenerativePoint => _generativePoint;

        [ShowInInspector]
        private bool _traversable;
        public bool Traversable => _traversable;

        private ImmutableArray<Transition> _transitions;
        public ImmutableArray<Transition> Transitions => _transitions;


        // Life cycle.

        internal MapSite(MapSiteData data, MapSystem map) : base(map, data.Id) {}

        internal void Initialize(MapData mapData, Vector2Int siteIndex, ReadOnlyArray2<MapSite> mapSites) {
            var data = mapData.Sites[siteIndex];
            _index = siteIndex;
            _generativePoint = (siteIndex + data.InternalPoint) * mapData.SiteSquareSize;
            _traversable = data.Traversable;
            _transitions = data.Transitions
                .Select(transition => new Transition(
                    neighbour: mapSites[transition.neighbour],
                    distance: transition.distance,
                    edge: transition.edge
                ))
                .ToImmutableArray();
        }


        // Graph vertex interface implementation.

        IEnumerable<Transition> IGraphVertex<MapSite, Transition>.Adjacent => _transitions;

        FixedSingle IPositionedGraphVertex<MapSite, Transition>.EstimateDistance(MapSite other) {
            return (_generativePoint - other._generativePoint).Magnitude;
        }


        // Informational methods.

        public bool Contains(FixedVector2 point) => Context.GetSiteByPosition(point) == this;
        public bool WeaklyContains(FixedVector2 point) => Contains(point) || ContainsOnBorder(point);

        public bool ContainsOnBorder(FixedVector2 point) {
            foreach (var transition in _transitions) {
                if (transition.Edge.GetDistance(point) <= _weakContainingDistance) {
                    return true;
                }
            }
            return false;
        }

        public List<Transition> FindMacroPath(MapSite to) =>
            AStar.FindShortestPath<MapSite, Transition>(this, to);


        // Progression methods.

        protected override void Act() {}
    }
}
