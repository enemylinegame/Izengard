using UnityEngine;
using Wave;
using Wave.Settings;

namespace CombatSystem
{
    [CreateAssetMenu(fileName = nameof(BattlePhaseConfig), menuName = "Wave/" + nameof(BattlePhaseConfig), order = 0)]
    public class BattlePhaseConfig : ScriptableObject
    {
        [SerializeField] private WaveSettings _waveSettings;
        [SerializeField] private EnemySpawnSettings _enemySpawnSettings;
        [SerializeField] private EnemySet _enemySet;
        
        public EnemySet EnemySet => _enemySet;
        public WaveSettings WaveSettings => _waveSettings;
        public EnemySpawnSettings EnemySpawnSettings => _enemySpawnSettings;
    }
}