using ResourceSystem;
using UnityEngine;

namespace Code.BuildingSystem
{
    public interface ICollectable
    {
        ResourceType ResourceType { get; set; }
        BuildingTypes BuildingTypes { get; set; }
        MineralConfig MineralConfig { get; set; }
        GameObject Prefab { get; set; }
        Vector3 SpawnPosition { get; set; }
        float CollectTime { get; set; }
        Sprite Icon { get; set; }
        int MaxWorkers { get; set; }
        int WorkersCount { get; set; }
        int BuildingID { get; set; }
        string VisibleName { get; set; }
        string Name { get; set; }

        IWorkerPreparation WorkerPreparation { get; set; }

        void InitBuilding();
        void InitMineral(MineralConfig mineralConfig);
    }
}