using EasterRts.Common.Events;
using UnityEngine;

namespace EasterRts.Battle.Ui {
    
    public abstract class UiBehaviour : MonoBehaviour {

        [SerializeField]
        private EventListener _uiUpdateListener;

        protected virtual void OnEnable() {
            _uiUpdateListener.AddHandler(OnUiUpdate);
        }

        protected virtual void OnDisable() {
            _uiUpdateListener.RemoveHandler(OnUiUpdate);
        }

        protected abstract void OnUiUpdate();
    }
}
