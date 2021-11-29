using EasterRts.Common.Cores;
using EasterRts.Utilities.FixedFloats;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace EasterRts.Battle.Data.Map {
    
    [Serializable]
    public class MapSiteData {

        [Serializable]
        public struct Transition {

            public Vector2Int neighbour;
            public FixedSingle distance;
            public FixedSegment2 edge;
        }

        [SerializeField]
        private CoreId _id;
        public CoreId Id {
            get => _id;
            set => _id = value;
        }

        [SerializeField]
        private FixedVector2 _internalPoint;
        public FixedVector2 InternalPoint {
            get => _internalPoint;
            set => _internalPoint = value;
        }

        [SerializeField]
        private bool _traversable;
        public bool Traversable {
            get => _traversable;
            set => _traversable = value;
        }

        [SerializeField]
        private List<Transition> _transitions = new List<Transition>();
        public List<Transition> Transitions {
            get => _transitions;
            set => _transitions = value?.ToList() ?? new List<Transition>();
        }

        public FixedVector2 GetPointPosition(Vector2Int siteIndex, FixedSingle segmentSize) =>
            (siteIndex + _internalPoint) * segmentSize;
    }
}
