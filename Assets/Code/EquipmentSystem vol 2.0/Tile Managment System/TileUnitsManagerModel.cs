using EquipmentSystem;
using UnityEngine;

namespace TileManagmentSystem
{ 

    [System.Serializable]
    public class TileUnitsManagerModel
    {
        public UnitView[] UnitsInThisTile => _unitsInThisTile;
        public Transform TileCentrPosition=> _tileCentrPosition;

        private int _currentUnitCount;
        private int _maxUnitCount;
        private TierNumber _tileTierNumber;
        private UnitView[] _unitsInThisTile;
        private Transform _tileCentrPosition;

        public TileUnitsManagerModel(int maxcountunit,TierNumber tirnumber,Transform tilecenterposition)
        {
            _currentUnitCount = 0;
            _maxUnitCount = maxcountunit;
            _tileTierNumber = tirnumber;
            _tileCentrPosition = tilecenterposition;
            _unitsInThisTile = new UnitView[_maxUnitCount];            
        }
        public TileUnitsManagerModel(TileUnitsManagerModel model,Transform tilecenterposition)
        {
            _maxUnitCount = model._maxUnitCount;
            _currentUnitCount = 0;
            _tileTierNumber = model._tileTierNumber;
            _unitsInThisTile = new UnitView[_maxUnitCount];
            _tileCentrPosition = tilecenterposition;
        }
        /// <summary>
        /// Mathod for adding unit in tile units array
        /// </summary>
        /// <param name="unit">unit for adding</param>
        /// <param name="index">index empty space in tile for unit</param>
        public void AddUnitInTile(UnitView unit,int index)
        {
            if (_unitsInThisTile[index]==null)
            {
                _unitsInThisTile[index] = unit;
                _currentUnitCount++;
                if (_currentUnitCount==_maxUnitCount)
                {
                    Debug.Log("This Tile is fool");
                }
            }
            else
            {
                throw new UnityException("This place in tile not free");
            }
        }
        /// <summary>
        /// Method for getting unit from tile
        /// </summary>
        /// <param name="index">index of unit</param>
        /// <returns></returns>
        public UnitView GetUnitFromTile(int index)
        {
            if (_unitsInThisTile[index]!=null)
            {
                _currentUnitCount--;
                UnitView tempUnit = _unitsInThisTile[index];
                _unitsInThisTile[index] = null;
                return tempUnit;
            }
            else
            {
                throw new UnityException("This place in tile not using by unit");
            }
        }
        /// <summary>
        /// Change max units count in tile. And create new units array
        /// </summary>
        /// <param name="maxCount">new max count of units in tile</param>
        public void ChangeUnitCount(int maxCount)
        {
            if (maxCount>_currentUnitCount)
            {
                _maxUnitCount = maxCount;
                int j = 0;
                UnitView[] tempUnitArray = new UnitView[_maxUnitCount];
                for (int i=0;i<_unitsInThisTile.Length;i++)
                {
                    if (_unitsInThisTile[i]!=null)
                    {
                        tempUnitArray[j] = _unitsInThisTile[i];
                        j++;
                    }
                }
                _unitsInThisTile = tempUnitArray;
            }
            else
            {
                throw new UnityException("Нельзя просто так взять и поставить максимальное число юнитов на тайле, меньше актуального!");
            }
        }
    }
}
