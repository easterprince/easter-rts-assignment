using EasterRts.Common.Cores;
using EasterRts.Utilities;
using System;
using System.IO;
using UnityEngine;

namespace EasterRts.Battle.Data.Units {

    [Serializable]
    public class UnitData : IBinarySerializable {

        [SerializeField]
        private CoreId _id;
        public CoreId Id {
            get => _id;
            set => _id = value;
        }

        [SerializeField]
        private UnitIntentionData _intention;
        public UnitIntentionData Intention {
            get => _intention;
            set => _intention = value;
        }

        [SerializeField]
        private MovementData _movement = new MovementData();
        public MovementData Movement {
            get => _movement;
            set => _movement = value;
        }

        public void ReadFrom(BinaryReader reader) {
            _id.ReadFrom(reader);
            _intention.ReadFrom(reader);
            _movement = new MovementData();
            _movement.ReadFrom(reader);
        }

        public void WriteTo(BinaryWriter writer) {
            _id.WriteTo(writer);
            _intention.WriteTo(writer);
            _movement.WriteTo(writer);
        }
    }
}
