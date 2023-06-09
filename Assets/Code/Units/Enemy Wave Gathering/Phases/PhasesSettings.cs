using UnityEngine;


namespace Wave
{
    [CreateAssetMenu(fileName = nameof(PhasesSettings), menuName = "Wave/" + nameof(PhasesSettings))]
    public sealed class PhasesSettings : ScriptableObject
    {
        [field: SerializeField] public float PeacefulPhaseDuration { get; private set; }
        [field: SerializeField] public float PreparatoryPhaseDuration { get; private set; }
    }
}