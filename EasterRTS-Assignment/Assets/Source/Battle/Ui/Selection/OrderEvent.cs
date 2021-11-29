using EasterRts.Common.Events;
using UnityEngine;

namespace EasterRts.Battle.Ui.Selection {
    
    [CreateAssetMenu(menuName = BattleUnityNames.createMenuEventsDirectory + "Order")]
    public class OrderEvent : ScriptableEvent<OrderData> {}
}
