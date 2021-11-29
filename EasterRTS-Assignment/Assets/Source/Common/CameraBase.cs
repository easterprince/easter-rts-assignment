using EasterRts.Common.Input;
using UnityEngine;

namespace EasterRts.Common {

    public abstract class CameraBase : MonoBehaviour {

        // Fields.

        private InputControls _inputControls;


        // Life cycle.

        protected virtual void Awake() {
            _inputControls = new InputControls();
        }

        protected virtual void OnEnable() {
            _inputControls.Camera.Enable();
        }

        protected virtual void OnDisable() {
            _inputControls.Camera.Disable();
        }


        // Camera input reading.

        protected Vector2 GetMotionInput() {
            return _inputControls.Camera.Move.ReadValue<Vector2>();
        }

        protected float GetRotationInput() {
            return _inputControls.Camera.Rotate.ReadValue<float>();
        }

        protected float GetZoomingInput() {
            return _inputControls.Camera.Zoom.ReadValue<Vector2>().y;
        }

        protected bool GetResettingInput() {
            return _inputControls.Camera.Reset.ReadValue<float>() != 0;
        }
    }
}
