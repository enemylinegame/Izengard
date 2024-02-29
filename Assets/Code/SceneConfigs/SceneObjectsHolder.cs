using BattleSystem.MainTower;
using SpawnSystem;
using Tools.Navigation;
using UI;
using UnityEngine;

namespace Code.SceneConfigs
{
    public class SceneObjectsHolder : MonoBehaviour
    {
        [field: SerializeField] public SpawnerView EnemySpawner { get; private set; }
        [field: SerializeField] public SpawnerView DefendersSpawner { get; private set; }
        [field: SerializeField] public MainTowerView MainTower { get; private set; }
        [field: SerializeField] public NavigationSurfaceView GroundSurface { get; private set; }
        [field: SerializeField] public BattleSceneUI BattleUI { get; private set; }
        [field: SerializeField] public MainTowerUI MainTowerUI { get; private set; }
    }
}