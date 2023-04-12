using System;
using UnityEngine;
using Views.BaseUnit;
using Views.BaseUnit.UI;
using Views.Outpost;
using BuildingSystem;


namespace Controllers.OutPost
{
    public class OutPostUnitController: IOnController, IDisposable
    {
        private int index;
        private int _currentCountOfNPC = 0;
        public UnitUISpawnerTest UiSpawnerTest;
        public OutpostUnitView OutpostUnitView;
        public Action<Vector3> Transaction;

        public OutPostUnitController(int index,OutpostUnitView outpostUnitView,UnitUISpawnerTest uiSpawnerTest)
        {
            OutpostUnitView = outpostUnitView;
            OutpostUnitView.IndexInArray = index;
            OutpostUnitView.UnitInZone += OutpostViewDetection;
            UiSpawnerTest = uiSpawnerTest;
            UiSpawnerTest.spawnUnit += BuyAUnit;
        }

        public void Dispose()
        {
            OutpostUnitView.UnitInZone -= OutpostViewDetection;
            UiSpawnerTest.spawnUnit -= BuyAUnit;
        }
        /// <summary>
        /// задаем значение куда сначала пойдет
        /// </summary>
        /// <param name="outPostUnitController">контроллер</param>
        private void BuyAUnit(OutPostUnitController outPostUnitController)
        {
            // if (this != outPostUnitController)
            // {
            //     return;
            // }
            if (OutpostUnitView.OutpostParametersData.GetMaxCountOfNPC() >
                _currentCountOfNPC)
            {
                _currentCountOfNPC++;
                Transaction?.Invoke(OutpostUnitView.gameObject.transform.position);
            }
        }

        /// <summary>
        /// в каком радиусе они буду спавниться. Это сделано для того чтобы, предотвратить моделку в моделке.
        /// </summary>
        /// <param name="radius"></param>
        /// <param name="center"></param>
        /// <returns></returns>
        private Vector2 CalculatePositionOfPlacementCircle(/*float maxCount, int currentNumberOfPoint,
            */ double radius, Vector3 center)
        {
            float x = (float)(Math.Cos(2 * Math.PI/* * currentNumberOfPoint / maxCount*/) * radius + center.x);
            float z = (float)(Math.Sin(2 * Math.PI/* * currentNumberOfPoint / maxCount*/) * radius + center.z);
            return new Vector2(x, z);
        }

        private int counter = 0;
        private void OutpostViewDetection(UnitMovement unitMovement)
        {
            OutpostUnitView.GetColliderParameters(out Vector3 center, out Vector3 size);
            var positionInZone = CalculatePositionOfPlacementCircle(/*OutpostUnitView.OutpostParametersData.GetMaxCountOfNPC(),
                counter,*/size.x,center);
            counter++;
            unitMovement.EnterWorkZone.Invoke(positionInZone);
        }

    }
}