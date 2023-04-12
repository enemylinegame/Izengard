using UnityEngine;


namespace Wave.Settings
{
    [CreateAssetMenu(fileName = nameof(WaveSettings), menuName = "Wave/" + nameof(WaveSettings))]
    public class WaveSettings : ScriptableObject
    {
        [field: SerializeField] public float WaveBaseCost { get; private set; } = 75;
        [field: SerializeField] public float Ñoefficient { get; private set; } = 1.04f;
        [field: SerializeField] public float CoefficientDowntime { get; private set; } = 0.2f;
        [field: SerializeField] public float CoefficientPortal { get; private set; } = 0.5f;
    }
}