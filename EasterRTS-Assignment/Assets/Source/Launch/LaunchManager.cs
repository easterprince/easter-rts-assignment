using EasterRts.Battle;
using EasterRts.Battle.Cores;
using EasterRts.Common;
using UnityEngine;

namespace EasterRts.Launch {

    public class LaunchManager : SceneManagerBase {

        [SerializeField]
        private BattleLauncher _battleLauncher;

        [SerializeField]
        private BattleTemplate _battleTemplate;

        private void Start() {
            Debug.Log("Launched!");
            var model = new BattleModel(_battleTemplate.Data);
            var launchData = new BattleContext(model);
            _battleLauncher.StartLoading(launchData);
            _battleLauncher.FinishLoading();
        }
    }
}
