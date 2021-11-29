using System;
using UnityEngine;

namespace EasterRts.Common.Cores {
    
    [Serializable]
    public class ModelData {

        [SerializeField]
        private CoreId _nextFreeId;
        public CoreId NextFreeId => _nextFreeId;
    }
}
