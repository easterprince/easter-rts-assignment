using EasterRts.Battle.Data.Map;
using EasterRts.Common.Cores;
using EasterRts.Utilities;
using EasterRts.Utilities.FixedFloats;
using Sirenix.OdinInspector;
using System;

namespace EasterRts.Battle.Cores.Map {

    public struct MapLocation : IEquatable<MapLocation> {

        // Fields.

        [ShowInInspector]
        private readonly FixedVector2 _position;
        public FixedVector2 Position => _position;

        [ShowInInspector]
        private readonly MapSite _site;
        public MapSite Site => _site;


        // Constructor.

        public MapLocation(FixedVector2 position, MapSite site) {
            _position = position;
            _site = site;
        }

        public MapLocation(MapLocationData data, IdToCore idToCore) {
            _position = data.Position;
            _site = idToCore?.Get<MapSite>(data.SiteId);
        }


        // ToString().

        public override string ToString() => $"({_position.X}, {_position.Y}, site id = {_site.Id})";


        // Equality checking methods.

        public override bool Equals(object obj) {
            if (obj is MapLocation other) {
                return Equals(other);
            }
            return false;
        }

        public bool Equals(MapLocation other) {
            return _position == other._position && _site == other._site;
        }

        public static bool operator ==(MapLocation first, MapLocation second) => first.Equals(second);
        public static bool operator !=(MapLocation first, MapLocation second) => !first.Equals(second);

        public override int GetHashCode() {
            var hashCode = -540547671;
            hashCode = hashCode * -1521134295 + _position.GetHashCode();
            hashCode = hashCode * -1521134295 + _site.Id.GetHashCode();
            return hashCode;
        }
    }
}
