using EasterRts.Common;
using EasterRts.Common.Events;
using EasterRts.Utilities;
using UnityEngine;

namespace EasterRts.Battle.World {

    public class BattleCamera : CameraBase {

        // Serialized fields.

        [SerializeField]
        private EventListener _worldUpdateListener;

        [SerializeField]
        private Camera _cameraComponent;
        public Camera Camera => _cameraComponent;

        [SerializeField]
        private BattleCameraContainer _cameraContainer;

        [Header("Initial values")]
        [SerializeField]
        private float _initialDistance = 5f;

        [Header("Restrictions")]
        [SerializeField]
        private float _minDistance = 3f;

        [SerializeField]
        private float _maxDistance = 10f;

        [Header("Input settings")]
        [SerializeField]
        private float _motionSpeed = 5f;

        [SerializeField]
        private float _rotationSpeed = 2f;

        [SerializeField]
        private float _zoomingSpeed = 1f;


        // Non-serialized fields.

        private Vector3 _target;
        public Vector3 Target {
            get => _target;
            private set => _target = value;
        }

        private float _rotation;
        public float Rotation {
            get => _rotation;
            private set => _rotation = Floats.ClampPeriodically(value, 2 * Mathf.PI);
        }

        private float _distance;
        public float Distance {
            get => _distance;
            private set => _distance = Mathf.Clamp(value, _minDistance, _maxDistance);
        }


        // Life cycle.

        protected override void OnEnable() {
            base.OnEnable();
            _worldUpdateListener.AddHandler(OnWorldUpdate);
            _cameraContainer.Reference = this;
        }

        private void Start() {
            Target = Vector3.zero;
            Rotation = 0;
            Distance = _initialDistance;
        }

        private void OnWorldUpdate() {

            // Apply input.
            var resettingInput = GetResettingInput();
            if (resettingInput) {
                Target = Vector3.zero;
                Rotation = 0;
                Distance = _initialDistance;
            } else {
                var motionInput = GetMotionInput();
                var motion = _motionSpeed * Time.deltaTime * (GetRotationQuaternion() * new Vector3(motionInput.x, 0, motionInput.y));
                Target += motion;
                var rotationInput = GetRotationInput();
                var rotation = _rotationSpeed * Time.deltaTime * rotationInput;
                Rotation += rotation;
                var zoomingInput = GetZoomingInput();
                var zooming = _zoomingSpeed * Time.deltaTime * -zoomingInput;
                Distance += zooming;
            }

            // Move transform.
            transform.rotation = GetRotationQuaternion();
            transform.position = _target - _distance * _cameraComponent.transform.forward;
        }

        protected override void OnDisable() {
            base.OnDisable();
            _worldUpdateListener.RemoveHandler(OnWorldUpdate);
            _cameraContainer.Reference = null;
        }


        // Supporting methods.

        private Quaternion GetRotationQuaternion() {
            return Quaternion.AngleAxis(Mathf.Rad2Deg * Rotation, Vector3.up);
        }
    }
}
