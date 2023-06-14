using System.Collections.Generic;
using UnityEngine;

namespace Code.TileSystem
{
    public class ResourcesLayoutUIView : MonoBehaviour
    {
        [field: SerializeField] public RectTransform LayoutRectTransform { get; private set; }
        [field: SerializeField] public List<ResourceView> Resources { get; set; }
    }
}