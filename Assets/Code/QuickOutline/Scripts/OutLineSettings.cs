using UnityEngine;

namespace Code.QuickOutline.Scripts
{
    [CreateAssetMenu(fileName = nameof(OutLineSettings), menuName = "Tile System/" + nameof(OutLineSettings))]
    public class OutLineSettings : ScriptableObject
    {
        [field: SerializeField] public Color OutLineColor { get; set; }
        [field: SerializeField, Range(0f, 10f)] public float OutlineWidth { get; set; }
        [Header("Materials")]
        [field: SerializeField] public Material OutlineMaskMaterial;
        [field: SerializeField] public Material OutlineFillMaterial;
    }
}