using Code.BuildingSystem;
using UnityEngine;
using Object = UnityEngine.Object;

public class BuildGenerator : IOnController
{
    //возможно переполнение массива при большом количестве зданий и ресурсов
    public GameObject[,] Buildings => _buildings;
    private GameObject[,] _buildings;
    private Building _flyingBuilding;
    //привязать к ширине тайла
    private float _offsetY = 0.1f;
    public BuildGenerator(GameConfig gameConfig)
    {
       
       _buildings = new GameObject[gameConfig.MapSizeX,gameConfig.MapSizeY];
       
    }
    public Building StartBuildingHouses(BuildingConfig config)
    {
        if (_flyingBuilding != null)
        {
            _flyingBuilding.gameObject.SetActive(false);
        }
        _flyingBuilding = Object.Instantiate(config.BuildingPrefab.GetComponent<Building>());
        return _flyingBuilding;
    }
}