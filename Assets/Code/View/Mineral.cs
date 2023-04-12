using ResourceSystem;

public class Mineral : BaseBuildAndResources
{
    public ResourceMine ThisResourceMine => _thisResourceMine;
    private ResourceMine _thisResourceMine;


    public void SetModelOfMine(MineralConfig mineralConfig)
    {
        _thisResourceMine = new ResourceMine(mineralConfig.NameOfMine, mineralConfig.ExtractionTime,
                                new ResourceHolder(mineralConfig.ResourceHolderMine),
                                mineralConfig.Icon, mineralConfig.CurrentMineValue, mineralConfig.Tier, mineralConfig.TypeOfMine);
    }
}