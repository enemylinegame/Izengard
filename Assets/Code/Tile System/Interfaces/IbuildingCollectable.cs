using ResourceSystem;

namespace Code.TileSystem
{
    public interface IbuildingCollectable
    {
        BuildingTypes BuildingType { get; set; }
        ResourceType ResourceType { get; set; }
    }
}