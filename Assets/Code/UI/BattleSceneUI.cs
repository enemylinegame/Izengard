using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class BattleSceneUI : MonoBehaviour
    {
        [field: SerializeField] public Button EnemyWaveStartButton { get; private set; }
        [field: SerializeField] public Button EnemyWaveStopButton { get; private set; }
        [field: SerializeField] public Button DefenderSpawnButton { get; private set; }
    }
}
