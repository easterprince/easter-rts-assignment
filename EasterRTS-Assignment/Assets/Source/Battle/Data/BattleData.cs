using EasterRts.Battle.Data.Map;
using EasterRts.Battle.Data.Units;
using EasterRts.Common.Cores;
using EasterRts.Utilities.FixedFloats;
using System;
using UnityEngine;

namespace EasterRts.Battle.Data {

    [Serializable]
    public class BattleData : ModelData {

        [SerializeField]
        private FixedSingle _startTime;
        public FixedSingle StartTime => _startTime;

        [SerializeField]
        private FixedSingle _timeDelta;
        public FixedSingle TimeDelta => _timeDelta;

        [SerializeField]
        private MapData _map = new MapData();
        public MapData Map {
            get => _map;
            set => _map = value ?? new MapData();
        }

        [SerializeField]
        private UnitSystemData _unitSystem = new UnitSystemData();
        public UnitSystemData UnitSystem {
            get => _unitSystem;
            set => _unitSystem = value ?? new UnitSystemData();
        }
    }
}
