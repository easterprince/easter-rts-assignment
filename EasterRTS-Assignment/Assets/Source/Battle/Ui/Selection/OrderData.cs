using EasterRts.Battle.World;
using UnityEngine;

namespace EasterRts.Battle.Ui.Selection {
    
    public class OrderData {

        private readonly OrderKind _kind;
        public OrderKind Kind => _kind;

        private readonly Vector3? _positionTarget;
        public Vector3? PositionTarget => _positionTarget;

        private readonly UnitEntity _unitTarget;
        public UnitEntity UnitTarget => _unitTarget;

        public OrderData(UnitEntity unitTarget) {
            _unitTarget = unitTarget;
            _kind = OrderKind.UnitTargeted;
        }

        public OrderData(Vector3 positionTarget) {
            _positionTarget = positionTarget;
            _kind = OrderKind.PositionTargeted;
        }
    }
}
