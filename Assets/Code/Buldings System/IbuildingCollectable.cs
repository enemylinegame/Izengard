using ResourceSystem;

namespace BuildingSystem
{
    public interface IbuildingCollectable
    {
        BuildingTypes BuildingType { get; set; }
        ResourceType ResourceType { get; set; }
    }
}