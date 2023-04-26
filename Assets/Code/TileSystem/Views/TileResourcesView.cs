using UnityEngine;
using UnityEngine.UI;

namespace Code.TileSystem
{
    public class TileResourcesView : MonoBehaviour
    {
        [SerializeField] private ResourceView _kResource;
        [SerializeField] private ResourceView _dResource;

        public ResourceView KResource 
        {
            get { return _kResource; }
        }
        public ResourceView DResource
        {
            get { return _dResource; }
        }
    }
}