using EasterRts.Battle.Cores.Map;
using EasterRts.Utilities;
using EasterRts.Utilities.Collections;
using Sirenix.OdinInspector;
using UnityEngine;

namespace EasterRts.Battle.World {

    public class MapEntity : MonoBehaviour {

        // Fields.

        [SerializeField]
        private BattleLauncher _battleLauncher;

        [SerializeField]
        private MapSegmentEntity _segmentPrefab;

        [SerializeField]
        private float _segmentWorldSize = 1;

        [SerializeField]
        private int _segmentTextureSize = 64;

        [ShowInInspector]
        [ReadOnly]
        private MapSystem _core;

        private Array2<MapSegmentEntity> _segments;


        // Life cycle.

        protected void Start() {
            var battleCore = _battleLauncher.Data.BattleCore;
            _core = battleCore.Map;
            var segmentsArraySize = Vector2Int.CeilToInt((Vector2) _core.Size / _segmentWorldSize - Mathf.Epsilon * Vector2.one);
            _segments = new Array2<MapSegmentEntity>(segmentsArraySize);
            foreach (var index in Enumerables.InRange2(_segments.Size)) {
                var segment = Instantiate(_segmentPrefab, transform);
                var origin = (Vector2) index * _segmentWorldSize;
                segment.Start();
                segment.SetData(_core, origin, _segmentWorldSize, _segmentTextureSize);
                segment.name = $"Segment {index}";
                _segments[index] = segment;
            }
        }
    }
}
