using System;
using BuildingSystem;
using ResourceSystem;
using UnityEngine;

namespace EquipmentSystem
{
    public class HireSystemView : MonoBehaviour
    {
        public event Action<bool> ActiveEquipUI;
        public event Action<itemHolderButton> equipRecruit;
        public event Action HiredRecruit;
        public BuyItemScreenView BuyItemScreen;

        public UnitView RecrutUnitView;
        public GameObject RecrutPrefab;
        public ItemsInObject items;
        public GoldCost TempCostInGold;
        public Transform PositionForInstantiateUnit;
        public EquipScreenController EquipController;

        public void EnableUI()
        {
            ActiveEquipUI?.Invoke(true);
        }
        public void DisableUI()
        {
            ActiveEquipUI?.Invoke(false);
        }

        public void EquipRecruit(itemHolderButton button)
        {
            equipRecruit?.Invoke(button);
        }
        
        public void HireRecruit()
        {
            HiredRecruit?.Invoke();
        }
    }
}