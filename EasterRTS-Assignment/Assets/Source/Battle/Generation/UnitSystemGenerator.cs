using EasterRts.Battle.Data.Map;
using EasterRts.Battle.Data.Units;
using EasterRts.Common.Cores;
using EasterRts.Utilities.FixedFloats;
using System.Collections.Generic;

namespace EasterRts.Battle.Generation {

    public class UnitSystemGenerator {

        public int UnitsCount { get; set; }
        public MapData Map { get; set; }

        public UnitSystemData Generate(int? seed = null) {

            var data = new UnitSystemData();
            var random = new System.Random(seed ?? 0);

            var units = new List<UnitData>();
            for (int i = 0; i < UnitsCount; ++i) {
                var position = new FixedVector2(
                    x: FixedSingle.FromThousandths(random.Next(0, 1000 * Map.Size.x)),
                    y: FixedSingle.FromThousandths(random.Next(0, 1000 * Map.Size.y))
                );
                var destination = new FixedVector2(
                    x: FixedSingle.FromThousandths(random.Next(0, 1000 * Map.Size.x)),
                    y: FixedSingle.FromThousandths(random.Next(0, 1000 * Map.Size.y))
                );
                var unit = new UnitData {
                    Id = new CoreId(i + 1),
                    Intention = UnitIntentionData.CreateMotion(destination),
                    Movement = new MovementData {
                        Location = new MapLocationData(position, CoreId.Undefined),
                        SpeedLimit = 5,
                    },
                };
                units.Add(unit);
            }

            data.Units = units;
            return data;
        }
    }
}
