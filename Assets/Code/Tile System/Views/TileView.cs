using System.Collections.Generic;
using UnityEngine;

namespace Code.TileSystem
{
    public class TileView : MonoBehaviour
    {
        [SerializeField] private TileConfig _tileConfig;
        [SerializeField] private List<Dot> _dotSpawns; 
        public Renderer Renderer;
        public TileModel TileModel;
        private void Awake()
        {
            TileModel = new TileModel();
            TileModel.TileConfig = _tileConfig;
            TileModel.DotSpawns = _dotSpawns;
            TileModel.Init();
        }

    }
}