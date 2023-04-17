using Code.TileSystem;
using ResourceSystem;
using UnityEngine;

public class Mineral : BaseBuildAndResources
{
    public ResourceMine ThisResourceMine => _thisResourceMine;
    private ResourceMine _thisResourceMine;

    public Mineral(MineralConfig mineralConfig)
    {

    }

    //private void Awake()
    //{
    //    if (mineralConfig == null)
    //      {
    //           throw new Exception($"Not implement mine model in {gameObject}");
    //      }
    //}
public void SetModelOfMine(ResourceMine mine)
    {
        _thisResourceMine=new ResourceMine(mine);
    }

    public void SetModelOfMine(MineralConfig mineralConfig)
    {
        _thisResourceMine = new ResourceMine(mineralConfig.NameOfMine, mineralConfig.ExtractionTime,
                                new ResourceHolder(mineralConfig.ResourceHolderMine),
                                mineralConfig.Icon, mineralConfig.CurrentMineValue, (TierNumber)mineralConfig.Tier, (TypeOfMine)mineralConfig.TypeOfMine);
    }
}