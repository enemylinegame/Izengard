using ResourceSystem;

namespace Code.TileSystem.Interfaces
{
    public interface IbuildingCollectable
    {
        BuildingTypes BuildingType { get; set; }
        ResourceType ResourceType { get; set; }
    }
}