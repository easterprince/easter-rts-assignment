using EasterRts.Common.Cores;
using EasterRts.Utilities;
using EasterRts.Utilities.Attributes;
using EasterRts.Utilities.FixedFloats;
using System;
using System.IO;
using UnityEngine;

namespace EasterRts.Battle.Data.Map {
    
    [Serializable]
    [OpenedBox]
    public struct MapLocationData : IEquatable<MapLocationData>, IBinarySerializable {

        // Fields.

        [SerializeField]
        private FixedVector2 _position;
        public FixedVector2 Position => _position;

        [SerializeField]
        private CoreId _siteId;
        public CoreId SiteId => _siteId;


        // Constructor.

        public MapLocationData(FixedVector2 position, CoreId siteId) {
            _position = position;
            _siteId = siteId;
        }


        // Equality checking methods.

        public override bool Equals(object obj) {
            if (obj is MapLocationData other) {
                return Equals(other);
            }
            return false;
        }

        public bool Equals(MapLocationData other) {
            return _position == other._position && _siteId == other._siteId;
        }

        public static bool operator ==(MapLocationData first, MapLocationData second) => first.Equals(second);
        public static bool operator !=(MapLocationData first, MapLocationData second) => !first.Equals(second);

        public override int GetHashCode() {
            var hashCode = -540547671;
            hashCode = hashCode * -1521134295 + _position.GetHashCode();
            hashCode = hashCode * -1521134295 + _siteId.GetHashCode();
            return hashCode;
        }


        // Serialization.

        public void ReadFrom(BinaryReader reader) {
            _position = reader.ReadFixedVector2();
            _siteId.ReadFrom(reader);
        }

        public void WriteTo(BinaryWriter writer) {
            writer.Write(_position);
            _siteId.WriteTo(writer);
        }
    }
}
