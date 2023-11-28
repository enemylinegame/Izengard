using SpawnSystem;
using BattleSystem.Buildings.View;
using Tools.Navigation;
using UnityEngine;

namespace Code.SceneConfigs
{
    public class SceneObjectsHolder : MonoBehaviour
    {
        [field: SerializeField] public SpawnerView EnemySpawner { get; private set; }
        [field: SerializeField] public SpawnerView DefendersSpawner { get; private set; }
        [field: SerializeField] public WarBuildingView MainTower { get; private set; }
        [field: SerializeField] public NavigationSurfaceView GroundSurface { get; private set; }
    }
}