using EasterRts.Battle.Cores;

namespace EasterRts.Battle {
    
    public class BattleContext {

        private readonly BattleCore _battleCore;
        public BattleCore BattleCore => _battleCore;

        private BattleManager _manager;
        private BattleModel _battleModel;

        public float LastModelUpdateTime => _manager.LastModelUpdateTime;

        public BattleContext(BattleModel battleModel) {
            _battleCore = battleModel.Core;
            _battleModel = battleModel;
        }

        public BattleModel RetrieveBattleModel(BattleManager manager) {
            _manager = manager;
            var model = _battleModel;
            _battleModel = null;
            return model;
        }
    }
}
