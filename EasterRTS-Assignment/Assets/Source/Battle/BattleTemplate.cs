using EasterRts.Battle.Data;
using EasterRts.Battle.Generation;
using Sirenix.OdinInspector;
using UnityEngine;

namespace EasterRts.Battle {

    [CreateAssetMenu(fileName = "Battle Template", menuName = "Battle/Battle Template")]
    public class BattleTemplate : ScriptableObject {

        [SerializeField]
        [InlineProperty]
        [HideLabel]
        private BattleData _data;
        public BattleData Data => _data;

        [Button]
        private void GenerateMap() {
            var mapData = _data.Map;
            var generator = new MapGenerator {
                Size = mapData.Size,
                SegmentSize = mapData.SiteSquareSize,
            };
            mapData = generator.Generate(Random.Range(0, 200000));
            _data.Map = mapData;
        }

        [Button]
        private void GenerateUnits() {
            var generator = new UnitSystemGenerator {
                UnitsCount = _data.UnitSystem.Units.Count,
                Map = _data.Map,
            };
            var data = generator.Generate(Random.Range(0, 200000));
            _data.UnitSystem = data;
        }
    }
}