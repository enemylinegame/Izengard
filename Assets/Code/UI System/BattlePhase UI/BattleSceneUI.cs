using UnityEngine;

namespace UI
{
    public class BattleSceneUI : MonoBehaviour
    {
        [SerializeField]
        private EnemySettingsPanel _enemySettings;
        [SerializeField]
        private DefenderSettingsPanel _defenderSettings;

        public EnemySettingsPanel EnemySettings => _enemySettings;
        public DefenderSettingsPanel DefenderSettings => _defenderSettings;
    }
}
