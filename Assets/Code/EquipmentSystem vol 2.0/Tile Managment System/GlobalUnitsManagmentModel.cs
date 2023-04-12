using EquipmentSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Global Units Managment Models ", menuName = "Units Configs/All units managment model", order = 1)]
public class GlobalUnitsManagmentModel : ScriptableObject
{
    public List<UnitView> AllPlayerFreeUnits => _allPlayerFreeUnits;
    public int CountOfFreeSpaceForUnitsAll => _currentFreeSpaceCount;

    private List<UnitView> _allPlayerFreeUnits;
    private List<UnitView> _allPlayerUnits;
    private int _currentFreeSpaceCount;
    private Transform _basePosition;

    /// <summary>
    /// Reset global model
    /// </summary>
    public void ResetModel()
    {
        _allPlayerFreeUnits = new List<UnitView>();
    }
    /// <summary>
    /// Get free unit by index
    /// </summary>
    /// <param name="index">index of free unit</param>
    /// <returns></returns>
    public UnitView GetFreeUnit(int index)
    {
        if (index < _allPlayerFreeUnits.Count && _allPlayerFreeUnits[index] != null)
        {
            UnitView tempUnit = _allPlayerFreeUnits[index];
            _allPlayerFreeUnits.RemoveAt(index);
            return tempUnit;
        }
        else
            throw new UnityException("Not correct index of unit");
    }
    /// <summary>
    /// Add unit from tile to hub
    /// </summary>
    /// <param name="unit"></param>
    public void AddUnitForFreeList(UnitView unit)
    {
        if (_currentFreeSpaceCount > 0)
        {
            _allPlayerFreeUnits.Add(unit);
            _currentFreeSpaceCount--;
        }
        else
        {
            Debug.Log("No free space for hire units");
        }

                
    }
}
