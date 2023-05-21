using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Code.TileSystem
{
    public class ResourcesLayoutUIView : MonoBehaviour
    {
        [SerializeField] private RectTransform _layoutRectTransform;
        [SerializeField] private List<ResourceView> _resources;

        public RectTransform LayoutRectTransform
        {
            get { return _layoutRectTransform; }
        }
        public List<ResourceView> Resources 
        {
            get { return _resources; }
        }

    }
}