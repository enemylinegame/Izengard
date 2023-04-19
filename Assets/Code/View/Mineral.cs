using Code.TileSystem;
using ResourceSystem;
using ResourceSystem.SupportClases;
using UnityEngine;

public class Mineral : BaseBuildAndResources
{
    public ResourceMine ThisResourceMine => _thisResourceMine;
    private ResourceMine _thisResourceMine;

    public Mineral(MineralConfig mineralConfig)
    {

    }

    public void SetModelOfMine(ResourceMine mine)
    {
        _thisResourceMine=new ResourceMine(mine);
    }

    public void SetModelOfMine(MineralConfig mineralConfig)
    {
        _thisResourceMine = new ResourceMine(mineralConfig.NameOfMine, mineralConfig.ExtractionTime,
            mineralConfig.Icon, mineralConfig.CurrentMineValue,mineralConfig.Tier, mineralConfig.TypeOfMine);
    }
}