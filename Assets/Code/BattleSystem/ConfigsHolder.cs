using Code.QuickOutline.Scripts;
using Code.Scriptable;
using Code.TileSystem;
using Code.TowerShot;
using CombatSystem;
using Core;
using ResourceMarket;
using ResourceSystem;
using UnityEngine;
using Wave;

namespace Code.Game
{
    [CreateAssetMenu(fileName = nameof(ConfigsHolder), menuName = "GameConfigs/" + nameof(ConfigsHolder))]
    public class ConfigsHolder : ScriptableObject
    {
        [field: Header("Global Configs")]
        [field: SerializeField] public GameConfig GameConfig { get; set; }
        [field: SerializeField] public GlobalTileSettings GlobalTileSettings { get; private set; }
        [field: SerializeField] public PrefabsHolder PrefabsHolder { get; set; }
        
        [field: Space(10), Header("UI")]
        [field: SerializeField] public UIElementsConfig UIElementsConfig { get; private set; }
        [field: Space(10), Header("Buildings")]
        [field: SerializeField] public ScriptableObject MainTowerConfig { get; private set; }
        [field: SerializeField] public OutLineSettings OutLineSettings { get; private set; }
        [field: SerializeField] public TowerShotConfig TowerShotConfig { get; private set; }
        
        [field: Space(10), Header("Wave")]
        [field: SerializeField] public BattlePhaseConfig BattlePhaseConfig { get; private set; }
        [field: SerializeField] public PhasesSettings PhasesSettings { get; private set; }
        [field: Space(10), Header("Resources")]
        [field: SerializeField] public PrescriptionsStorage PrescriptionsStorage { get; private set; }
        [field: SerializeField] public GlobalMineralsList GlobalMineralsList { get; private set; }
        [field: SerializeField] public MarketDataConfig MarketDataConfig { get; private set; }
        [field: SerializeField] public GlobalResourceData GlobalResourceData { get; private set; }
        [field: Space(10), Header("Units")]
        [field: SerializeField] public DefendersSet DefendersSet { get; private set; }
        
        [field: SerializeField] public WorkersTeamConfig WorkersTeamConfig { get; private set; }
        
        
        
        
    }
}