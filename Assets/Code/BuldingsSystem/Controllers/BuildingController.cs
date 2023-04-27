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

        public Building StartBuilding(TileView view, BuildingConfig config)
        {
            if (CheckDot(view))
            {
                var build = Object.Instantiate(config.BuildingPrefab.GetComponent<Building>(), CheckDot(view).transform);
                build.Type = config.BuildingType;
                return build;
            }
            _centerText.NotificationUI("You have built maximum buildings", 1000);
            return null;
        }
        public void RemoveTypeDots(TileView view, Building building)
        {
            if (view.DotSpawns.Exists(x => x.Types == building.Type))
            {
                var dot = view.DotSpawns.Find(x => x.Types == building.Type);
                dot.Types = BuildingTypes.None;
                dot.IsActive = true;
            }
        }

        public void ADDListMinerals(TileView view)
        {
            if (view.FloodedMinerals.Count < 2)
            {
                foreach (var dot in view.DotSpawns)
                {
                    if (dot.Mineral)
                    {
                        view.FloodedMinerals.Add(dot.Mineral);
                    }
                }
            }
            
        }
        private GameObject CheckDot(TileView view)
        {
            foreach (var dot in view.DotSpawns)
            {
                if (dot.IsActive == true)
                {
                    return dot.gameObject;
                }
            }
            return null;
        }
    }
}