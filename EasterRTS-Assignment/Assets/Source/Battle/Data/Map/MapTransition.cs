using EasterRts.Utilities.FixedFloats;
using System;
using UnityEngine;

namespace EasterRts.Battle.Data.Map {
    
    [Serializable]
    public struct MapTransition {

        public Vector2Int neighbour;
        public FixedSingle distance;

        public MapTransition(Vector2Int neighbour, FixedSingle distance) {
            this.neighbour = neighbour;
            this.distance = distance;
        }
    }
}
