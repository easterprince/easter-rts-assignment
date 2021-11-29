using EasterRts.Battle.Cores.Units;
using EasterRts.Battle.Data.Units;
using EasterRts.Battle.Ui.Selection;
using EasterRts.Common.Containers;
using EasterRts.Common.Cores;
using EasterRts.Common.Events;
using EasterRts.Utilities.FixedFloats;
using Sirenix.OdinInspector;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace EasterRts.Battle.Ui {

    public class UnitView : UiBehaviour {

        // Fields.

        [SerializeField]
        private StateListener<SelectionData> _selectionListener;

        [SerializeField]
        private EventListener<OrderData> _orderListener;

        [Required]
        [SerializeField]
        private Text _text;


        // Life cycle.

        protected override void OnEnable() {
            base.OnEnable();
            _orderListener.AddHandler(OnOrder);
        }

        protected override void OnUiUpdate() {
            var message = new StringBuilder();
            var unit = _selectionListener.GetCurrentState()?.Unit;
            if (unit == null) {
                message.AppendLine("Nothing is selected.");
            } else {
                var core = unit.Core;
                message.AppendLine($@"Unit ""{unit.name}"" selected.");
                message.AppendLine($@"- Position: {core.Movement.Location.Position}.");
                message.AppendLine($@"- Destination: {core.Movement.Destination}.");
                message.AppendLine($@"- Intention: {core.Intention}.");
            }
            _text.text = message.ToString();
        }

        protected override void OnDisable() {
            base.OnDisable();
            _orderListener.RemoveHandler(OnOrder);
        }


        // Listener handlers.

        private void OnOrder(OrderData orderData) {
            var selectedUnit = _selectionListener.GetCurrentState().Unit;
            if (selectedUnit == null) {
                return;
            }
            switch (orderData.Kind) {
                case OrderKind.PositionTargeted:
                    var targetedPosition = (Vector3) orderData.PositionTarget;
                    var destination = (FixedVector2) new Vector2(targetedPosition.x, targetedPosition.z);
                    selectedUnit.Core.PlanIntentionSetting(UnitIntention.CreateMotion(destination), default);
                    break;
                case OrderKind.UnitTargeted:
                    var targetedUnit = orderData.UnitTarget?.Core ?? null;
                    selectedUnit.Core.PlanIntentionSetting(UnitIntention.CreateFollowing(targetedUnit), default);
                    break;
            }
        }
    }
}
