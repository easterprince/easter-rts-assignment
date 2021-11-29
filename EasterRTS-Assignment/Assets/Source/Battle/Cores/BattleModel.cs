using EasterRts.Battle.Data;
using EasterRts.Common.Cores;
using Sirenix.OdinInspector;

namespace EasterRts.Battle.Cores {

    public class BattleModel : ModelBase {

        // Fields.

        [ShowInInspector]
        private BattleCore _core;


        // Constructor.

        public BattleModel(BattleData data) : base(data) {
            _core = new BattleCore(data, Token);
            _core.Initialize(data);
        }


        // Properties.

        public BattleCore Core => _core;
    }
}
