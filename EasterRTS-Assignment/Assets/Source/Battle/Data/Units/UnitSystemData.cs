using System;
using System.Collections.Generic;
using UnityEngine;

namespace EasterRts.Battle.Data.Units {

    [Serializable]
    public class UnitSystemData {

        [SerializeField]
        private List<UnitData> _units = new List<UnitData>();
        public List<UnitData> Units {
            get => _units;
            set => _units = value ?? new List<UnitData>();
        }
    }
}
