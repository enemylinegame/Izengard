using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ResurseSystem;
using BuildingSystem;
using UnityEngine.UI;
using TMPro;

namespace EquipmentSystem
{ 
    public class HireSystemController : MonoBehaviour
    {

        [SerializeField] private BuyItemScreenView _BuyItemScreen;

        [SerializeField] private UnitView _RecrutUnitView;
        [SerializeField] private GameObject _RecrutPrefab;

        [SerializeField] private GlobalResurseStock _GlobalResStock;

        [SerializeField] private ItemsInObject _items;

        [SerializeField] private GoldCost _TempCostInGold;

        [SerializeField] private Transform _PositionForInstantiateUnit;

        [SerializeField] private EquipScreenController _EquipController;

        public HireSystemController(UnitView unitView, GlobalResurseStock glStock, BuyItemScreenView buyView, EquipScreenController eqScreenController)
        {
            _RecrutUnitView = unitView;
            _GlobalResStock = glStock;
            _BuyItemScreen = buyView;
            _EquipController = eqScreenController;            
            buyView.gameObject.SetActive(false);
            eqScreenController.SetActiveEquipScreen(false);
            SortingAndFillModels();
        }
        public void SetActiveHireScreen()
        {
            _RecrutUnitView.gameObject.SetActive(true);
            _EquipController.SetUnit(_RecrutUnitView);
            _EquipController.SetActiveEquipScreen(true);
            _BuyItemScreen.SetActiveScreen(true);
            _BuyItemScreen.SetActiveHireScreen();


            if (_TempCostInGold==null)
            {
                SetTempCost();
            }
        }

        public void EquipRecruit(itemHolderButton button)
        {
            var tempModel = button.GetModelInButton();
            
            _EquipController.EquipCharacter(tempModel);
            ChangeCostBasket();
        }
        public void SetTempCost()
        {
            if (_RecrutUnitView != null)
            {
                float tempvalue = _RecrutUnitView.GetUnitInventory().CostEquipmentinGold;
                GoldCost tempCost = new GoldCost (_RecrutUnitView.GetUnitInventory().ArmorSlotsController.BootsSlot.ItemModel.CostInGold,tempvalue);
                _TempCostInGold=tempCost;
            }
            else
            { 
                _TempCostInGold = null;
            }            
        }
        public void ChangeCostBasket()
        {
            float tempvalue = _RecrutUnitView.GetUnitInventory().CostEquipmentinGold;
            _TempCostInGold.ChangeCost(tempvalue);
            _BuyItemScreen.SetCurrentCost(tempvalue.ToString());
        }
        public void ItemButtonsSubscribe()
        {

        }
        public void SortingAndFillModels()
        {
            List<ItemModel> bodyArmors = new List<ItemModel>();
            List<ItemModel> helmets = new List<ItemModel>();
            foreach (ArmorModel model in _items.ArmorsInObject)
            {
                switch (model.ArmorSlotTypeID)
                {
                    case 1:
                        bodyArmors.Add(model);
                        break;
                    case 4:
                        helmets.Add(model);
                        break;
                    default:
                        break;
                }
            }
            List<ItemModel> rightWeapons = new List<ItemModel>();
            List<ItemModel> leftWeapons = new List<ItemModel>();
            List<ItemModel> twoHWeapons = new List<ItemModel>();
            
            foreach (WeaponModel model in _items.WeaponsInObject)
            {
                switch (model.WeaponGripTypeID)
                {
                    case 1:
                        rightWeapons.Add(model);
                        break;
                    case 3:
                        leftWeapons.Add(model);
                        break;
                    case 2:
                        twoHWeapons.Add(model);
                        break;
                    default:
                        break;
                }
            }
            _BuyItemScreen.SetBodyArmorContent(bodyArmors);
            _BuyItemScreen.SetHelmetContent(helmets);
            _BuyItemScreen.SetRightWeaponContent(rightWeapons);
            _BuyItemScreen.SetLeftWeaponContent(leftWeapons);
            _BuyItemScreen.SetTwoHandtWeaponContent(twoHWeapons);
        }
        public void SetPositionForHire(Transform parent)
        {
            _PositionForInstantiateUnit = parent;
        }
        public void HireUnit()
        {
            
            if (_GlobalResStock.PriceGoldFromGlobalStock(_TempCostInGold))
            {
                _EquipController.ChangeCameraPosition();
                Instantiate(_RecrutUnitView.gameObject, _PositionForInstantiateUnit);
                _EquipController.SetCameraOnUnit();

            }
            else
            {
                _BuyItemScreen.SetCurrentCost($"Õ”∆ÕŒ ¡ŒÀ‹ÿ≈ «ŒÀŒ“¿!");
            }
            
        }
        public void Cancel()
        {
            _RecrutUnitView.gameObject.SetActive(false);
            _EquipController.SetActiveEquipScreen(false);

            _BuyItemScreen.SetActiveScreen(false);

        }
        public void Awake()
        {
            _RecrutPrefab = Instantiate(_RecrutPrefab,gameObject.transform);
            _RecrutUnitView = _RecrutPrefab.GetComponent<UnitView>();
            _EquipController.SetUnit(_RecrutUnitView);
            SortingAndFillModels();
            _EquipController.SetCameraOnUnit();
        }


    }
}
