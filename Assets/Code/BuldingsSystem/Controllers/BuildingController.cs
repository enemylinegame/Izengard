using Code.BuldingsSystem.ScriptableObjects;
using Code.TileSystem;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = System.Random;

namespace Code.BuildingSystem
{
    public class BuildingController
    {
        private BaseCenterText _centerText;
        public BuildingController(BaseCenterText centerText)
        {
            _centerText = centerText;
        }

        public Building StartBuilding(TileModel model, BuildingConfig config)
        {
            if (CheckDot(model))
            {
                var dot = CheckDot(model);
                var build = Object.Instantiate(config.BuildingPrefab.GetComponent<Building>(), dot.transform);
                build.Type = config.BuildingType;
                dot.IsActive = false;
                return build;
            }
            _centerText.NotificationUI("You have built maximum buildings", 1000);
            return null;
        }
        public void RemoveTypeDots(TileModel model, Building building)
        {
            if (model.DotSpawns.Exists(x => x.Building == building))
            {
                var dot = model.DotSpawns.Find(x => x.Building == building);
                dot.Building.Type = BuildingTypes.None;
                dot.IsActive = true;
            }
        }
        public Dot CheckDot(TileModel model)
        {
            var cleardots = model.DotSpawns.Find(x => x.IsActive);
            return cleardots;
        }
    }
}