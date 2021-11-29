using EasterRts.Battle.Data.Map;
using EasterRts.Common.Cores;
using EasterRts.Utilities;
using EasterRts.Utilities.Collections;
using EasterRts.Utilities.FixedFloats;
using Sirenix.OdinInspector;
using UnityEngine;

namespace EasterRts.Battle.Cores.Map {

    public class MapSystem : DependentCoreBase<MapSystem, BattleCore> {

        // Constants.

        private const int _siteSearchRadius = 1;


        // Fields.

        [ShowInInspector]
        [ReadOnly]
        private Vector2Int _size;
        public Vector2Int Size => _size;

        [ShowInInspector]
        [ReadOnly]
        private int _siteSquareSize;
        public int SiteSquareSize => _siteSquareSize;

        private Array2<MapSite> _sites;
        public ReadOnlyArray2<MapSite> Sites => _sites.ToReadOnlyArray2();


        // Life cycle.

        internal MapSystem(MapData data, BattleCore context) : base(context, CoreId.Undefined) {
            _sites = new Array2<MapSite>(data.Sites.Size);
            foreach (var index in _sites.Indexes) {
                _sites[index] = new MapSite(data.Sites[index], this);
            }
        }

        internal void Initialize(MapData data) {
            _size = data.Size;
            foreach (var index in _sites.Indexes) {
                _sites[index].Initialize(data, index, _sites.ToReadOnlyArray2());
            }
            _siteSquareSize = data.SiteSquareSize;
        }


        // Informational methods.

        // TODO: Everything should use MapLocation, not positions as FixedVector2.
        // Need to force all MapCore methods input MapLocation instead of FixedVector2.
        public MapLocation GetLocation(FixedVector2 position) =>
            new MapLocation(position, GetSiteByPosition(position));

        public MapSite GetSiteByIndex(Vector2Int index) {
            index = Enumerables.ClampInRange2(index, _sites.Size);
            return _sites[index];
        }

        public MapSite GetSiteByPosition(FixedVector2 position) {
            if (!position.IsValid) {
                return null;
            }
            var index = GetIndex(position);
            var nearestSite = _sites[index];
            var nearestSquaredDistance = (position - nearestSite.GenerativePoint).SquaredMagnitude;
            var adjacentIndexLowerBound = Enumerables.ClampInRange2(index - new Vector2Int(_siteSearchRadius, _siteSearchRadius), _sites.Size);
            var adjacentIndexUpperBound = Enumerables.ClampInRange2(index + new Vector2Int(_siteSearchRadius, _siteSearchRadius), _sites.Size);
            foreach (var adjacentIndex in Enumerables.InSegment2(adjacentIndexLowerBound, adjacentIndexUpperBound)) {
                if (adjacentIndex == index) {
                    continue;
                }
                var adjacentSite = _sites[adjacentIndex];
                var adjacentSquaredDistance = (position - adjacentSite.GenerativePoint).SquaredMagnitude;
                if (adjacentSquaredDistance < nearestSquaredDistance) {
                    nearestSite = adjacentSite;
                    nearestSquaredDistance = adjacentSquaredDistance;
                }
            }
            return nearestSite;
        }

        public Vector2Int GetIndex(FixedVector2 position) {
            var index = (Vector2Int) (position / _siteSquareSize);
            index = Enumerables.ClampInRange2(index, _sites.Size);
            return index;
        }

        public bool Contains(FixedVector2 position) =>
            position.X >= 0 && position.X <= _size.x && position.Y >= 0 && position.Y <= _size.y;


        // Progression methods.

        protected override void Act() { }
    }
}
