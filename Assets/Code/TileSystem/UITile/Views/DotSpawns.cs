using System.Collections.Generic;
using UnityEngine;

namespace Code.TileSystem
{
    public class DotSpawns : MonoBehaviour
    {
        [SerializeField] private List<GameObject> _dots;

        public List<GameObject> Dots => _dots;
    }
}