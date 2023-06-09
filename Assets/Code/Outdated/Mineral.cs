using Code.TileSystem;
using ResourceSystem;
using ResourceSystem.SupportClases;
using UnityEngine;


//TODO : Mines controller
public class Mineral : MonoBehaviour
{
    public ResourceMine ThisResourceMine => _thisResourceMine;

    public MineralConfig MineralConfig=>_mineralConfig;
    private ResourceMine _thisResourceMine;
    private MineralConfig _mineralConfig;

    private ResourceHolder _resourceHolder;

    public Mineral(MineralConfig mineralConfig)
    {
        LoadMineralModel(mineralConfig);
        _resourceHolder = new ResourceHolder(mineralConfig.ResourceType, mineralConfig.CurrentMineValue,mineralConfig.CurrentMineValue);
    }
    public void LoadMineralModel(MineralConfig mineralConfig) => _mineralConfig = mineralConfig;

    public void SetModelOfMine(MineralConfig mineralConfig)
    {
        _thisResourceMine = new ResourceMine(mineralConfig.NameOfMine, mineralConfig.ExtractionTime,
            mineralConfig.Icon, mineralConfig.CurrentMineValue,mineralConfig.Tier, mineralConfig.ResourceType);
    }
}