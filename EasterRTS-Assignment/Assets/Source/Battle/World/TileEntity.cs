using UnityEngine;

namespace EasterRts.Battle.World {

    public class TileEntity : MonoBehaviour {

        [SerializeField]
        private Renderer _renderer;

        public void SetColor(Color color) {
            var materialProperties = new MaterialPropertyBlock();
            materialProperties.SetColor("_Color", color);
            _renderer.SetPropertyBlock(materialProperties);
        }
    }
}
