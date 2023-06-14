using System.Collections.Generic;
using Views.BaseUnit.UI;
using Views.Outpost;

namespace Controllers.OutPost
{
    public class OutpostSpawner: IOnController, IOnStart
    {
        public List<OutPostUnitController> OutPostUnitControllers;
        private UnitUISpawnerTest _unitUISpawnerTest;

        public OutpostSpawner(UnitUISpawnerTest unitUISpawnerTest)
        {
            _unitUISpawnerTest = unitUISpawnerTest;
        }
        
        public void OnStart()
        {
            OutPostUnitControllers = new List<OutPostUnitController>();
        }

        public void SpawnLogic(OutpostUnitView unitView)
        {
            var index = OutPostUnitControllers.Count;
            OutPostUnitControllers.Add(new OutPostUnitController(index,unitView,_unitUISpawnerTest));
        }

    }
}