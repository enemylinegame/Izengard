using System.Collections.Generic;
using UnityEngine;


namespace CombatSystem
{
    [CreateAssetMenu(fileName = nameof(DefendersSet), menuName = "Defender/" + nameof(DefendersSet))]
    public class DefendersSet : ScriptableObject
    {
        [SerializeField] private List<DefenderSettings> defendersSettingsList;
        public IReadOnlyList<DefenderSettings> Defenders => defendersSettingsList;
    }
}