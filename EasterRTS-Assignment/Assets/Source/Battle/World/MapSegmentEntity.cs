using EasterRts.Battle.Cores.Map;
using EasterRts.Common;
using EasterRts.Utilities.FixedFloats;
using UnityEngine;

namespace EasterRts.Battle.World {

    public class MapSegmentEntity : DependentMonoBehaviour {

        [SerializeField]
        private Renderer _renderer;

        protected override void OnStart() {}

        public void SetData(MapSystem mapCore, Vector2 origin, float worldSize, int textureSize) {

            transform.position = new Vector3(origin.x, 0, origin.y);
            transform.localScale = new Vector3(worldSize, 1, worldSize);

            var texture = new Texture2D(textureSize, textureSize);
            for (int x = 0; x < textureSize; ++x) {
                for (int y = 0; y < textureSize; ++y) {
                    var pixelPosition = (FixedVector2) (origin + new Vector2(textureSize - x - 0.5f, textureSize - y - 0.5f) / textureSize * worldSize);
                    var site = mapCore.GetSiteByPosition(pixelPosition);
                    Color color;
                    if (!mapCore.Contains(pixelPosition) || (pixelPosition - site.GenerativePoint).SquaredMagnitude < FixedSingle.FromThousandths((int) (0.5 * 0.5 * 1000))) {
                        color = Color.black;
                    } else if (!site.Traversable) {
                        color = Color.gray;
                    } else if (site.ContainsOnBorder(pixelPosition)) {
                        color = Color.white;
                    } else {
                        color = GetColor(site.Index);
                    }
                    texture.SetPixel(x, y, color);
                }
            }
            texture.wrapMode = TextureWrapMode.Clamp;
            texture.filterMode = FilterMode.Point;
            texture.Apply();

            var properties = new MaterialPropertyBlock();
            properties.SetTexture("_MainTex", texture);
            _renderer.SetPropertyBlock(properties);
        }

        private Color GetColor(Vector2Int index) {
            var colorIndex = new Vector2Int(index.x % 2, index.y % 2);
            if (colorIndex.x == 0) {
                if (colorIndex.y == 0) {
                    return Color.red;
                } else {
                    return Color.blue;
                }
            } else {
                if (colorIndex.y == 0) {
                    return Color.yellow;
                } else {
                    return Color.green;
                }
            }
        }
    }
}
