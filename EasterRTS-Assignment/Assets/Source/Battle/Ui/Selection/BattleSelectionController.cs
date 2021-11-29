using EasterRts.Battle.World;
using EasterRts.Common.Containers;
using EasterRts.Common.Events;
using UnityEngine;
using UnityEngine.EventSystems;

namespace EasterRts.Battle.Ui.Selection {
    
    public class BattleSelectionController : MonoBehaviour, IPointerClickHandler {

        private const float _rayCastMaxDistance = 500;

        [SerializeField]
        private StateDispatcher<SelectionData> _selectionDispatcher;

        [SerializeField]
        private EventDispatcher<OrderData> _orderDispatcher;

        [SerializeField]
        private BattleCameraContainer _battleCameraContainer;

        public void OnPointerClick(PointerEventData eventData) {
            if (!_battleCameraContainer.Set) {
                return;
            }
            var camera = _battleCameraContainer.Reference.Camera;
            var ray = camera.ScreenPointToRay(eventData.position);
            switch (eventData.button) {
                case PointerEventData.InputButton.Left:

                    var hitHappened = Physics.Raycast(ray, hitInfo: out var hit, layerMask: WorldLayers.UnitSystemLayer, maxDistance: _rayCastMaxDistance);
                    UnitEntity unit = null;
                    if (hitHappened) {
                        unit = hit.collider.GetComponent<UnitEntity>();
                    }
                    var selectionData = new SelectionData(unit);
                    _selectionDispatcher.Dispatch(selectionData);
                    break;

                case PointerEventData.InputButton.Right:

                    hitHappened = Physics.Raycast(ray, hitInfo: out hit, layerMask: WorldLayers.UnitSystemLayer | WorldLayers.MapLayer, maxDistance: _rayCastMaxDistance);
                    if (!hitHappened) {
                        break;
                    }
                    OrderData orderData = null;
                    var mapSegment = hit.collider.GetComponent<MapSegmentEntity>();
                    if (mapSegment != null) {
                        orderData = new OrderData(hit.point);
                    }
                    unit = hit.collider.GetComponent<UnitEntity>();
                    if (unit != null) {
                        orderData = new OrderData(unit);
                    }
                    if (orderData != null) {
                        _orderDispatcher.Dispatch(orderData);
                    }
                    break;
            }
        }
    }
}
