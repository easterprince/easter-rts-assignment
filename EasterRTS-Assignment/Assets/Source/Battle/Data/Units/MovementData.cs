using EasterRts.Battle.Data.Map;
using EasterRts.Utilities;
using EasterRts.Utilities.FixedFloats;
using System;
using System.IO;
using UnityEngine;

namespace EasterRts.Battle.Data.Units {
    
    [Serializable]
    public class MovementData : IBinarySerializable {

        [SerializeField]
        private FixedSingle _speedLimit;
        public FixedSingle SpeedLimit {
            get => _speedLimit;
            set => _speedLimit = value;
        }

        [SerializeField]
        private MapLocationData _location;
        public MapLocationData Location {
            get => _location;
            set => _location = value;
        }

        [SerializeField]
        private FixedVector2 _velocity;
        public FixedVector2 Velocity {
            get => _velocity;
            set => _velocity = value;
        }

        [SerializeField]
        private MapLocationData _immediateDestination;
        public MapLocationData ImmediateDestination {
            get => _immediateDestination;
            set => _immediateDestination = value;
        }

        [SerializeField]
        private MapLocationData _followedFinalDestination;
        public MapLocationData FollowedFinalDestination {
            get => _followedFinalDestination;
            set => _followedFinalDestination = value;
        }

        [SerializeField]
        private MapLocationData _finalDestination;
        public MapLocationData FinalDestination {
            get => _finalDestination;
            set => _finalDestination = value;
        }

        public void ReadFrom(BinaryReader reader) {
            _speedLimit = reader.ReadFixedSingle();
            _location.ReadFrom(reader);
            _velocity = reader.ReadFixedVector2();
            _immediateDestination.ReadFrom(reader);
            _followedFinalDestination.ReadFrom(reader);
            _finalDestination.ReadFrom(reader);
        }

        public void WriteTo(BinaryWriter writer) {
            writer.Write(_speedLimit);
            _location.WriteTo(writer);
            writer.Write(_velocity);
            _immediateDestination.WriteTo(writer);
            _followedFinalDestination.WriteTo(writer);
            _finalDestination.WriteTo(writer);
        }
    }
}
