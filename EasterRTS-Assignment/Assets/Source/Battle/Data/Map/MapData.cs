using EasterRts.Utilities.Collections;
using System;
using UnityEngine;

namespace EasterRts.Battle.Data.Map {

    [Serializable]
    public class MapData {

        [SerializeField]
        private Vector2Int _size;
        public Vector2Int Size {
            get => _size;
            set => _size = value;
        }

        [SerializeField]
        private int _siteSquareSize;
        public int SiteSquareSize {
            get => _siteSquareSize;
            set => _siteSquareSize = value;
        }

        [SerializeField]
        private Array2<MapSiteData> _sites = new Array2<MapSiteData>();
        public Array2<MapSiteData> Sites {
            get => _sites;
            set => _sites = value?.Copy() ?? new Array2<MapSiteData>();
        }
    }
}
