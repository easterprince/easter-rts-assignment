using UnityEngine;

namespace EasterRts.Battle.World {
    
    public static class WorldLayers {

        private const string _unitSystemLayerName = "Unit System";
        public static int UnitSystemLayerIndex => LayerMask.NameToLayer(_unitSystemLayerName);
        public static int UnitSystemLayer => (1 << UnitSystemLayerIndex);

        private const string _mapLayerName = "Map";
        public static int MapLayerIndex => LayerMask.NameToLayer(_mapLayerName);
        public static int MapLayer => (1 << MapLayerIndex);
    }
}
