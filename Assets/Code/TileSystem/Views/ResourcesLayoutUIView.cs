using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Code.TileSystem
{
    public class ResourcesLayoutUIView : MonoBehaviour
    {
        [SerializeField] private RectTransform _layoutRectTransform;
        [SerializeField] private List<ResourceView> _resources;

        [SerializeField] private Button _testAdd;

        public RectTransform LayoutRectTransform
        {
            get { return _layoutRectTransform; }
        }
        public List<ResourceView> Resources 
        {
            get { return _resources; }
        }
        
        public Button TestAdd
        {
            get { return _testAdd; }
        }
    }
}