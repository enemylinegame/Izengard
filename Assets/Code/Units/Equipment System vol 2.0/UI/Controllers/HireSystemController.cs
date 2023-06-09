using System;
using System.Collections.Generic;
using UnityEngine;
using ResourceSystem;
using Controllers.Pool;

namespace EquipmentSystem
{
    public class HireSystemController : IOnStart, IOnController, IDisposable

    {
    private HireSystemView _view;
    private BaseELFUnitFactory _factory;
    public GlobalStock _globalResStock;
    private Transform _whereToSpawn;
    private GeneratorLevelController _levelController;

    public HireSystemController(GlobalStock glStock, BuyItemScreenView buyView,
        EquipScreenController eqScreenController, HireSystemView view, GeneratorLevelController levelController)
    {
        view.BuyItemScreen = buyView;
        view.EquipController = eqScreenController;
        _globalResStock = glStock;
        _view = view;
        _levelController = levelController;
        _view.ActiveEquipUI += ActiveUI;
        _view.equipRecruit += EquipRecruit;
        _view.HiredRecruit += HireReqruit;


        buyView.gameObject.SetActive(false);
        eqScreenController.SetActiveEquipScreen(false);
        SortingAndFillModels();
    }


        public void SetActiveHireScreen()
    {
        _view.RecrutUnitView.gameObject.SetActive(true);
        _view.EquipController.SetUnit(_view.RecrutUnitView);
        _view.EquipController.SetActiveEquipScreen(true);
        _view.BuyItemScreen.SetActiveScreen(true);
        _view.BuyItemScreen.SetActiveHireScreen();


        if (_view.TempCostInGold == null)
        {
            SetTempCost();
        }
    }

    public void EquipRecruit(itemHolderButton button)
    {
        var tempModel = button.GetModelInButton();

        _view.EquipController.EquipCharacter(tempModel);
        ChangeCostBasket();
    }

    public void SetTempCost()
    {
        if (_view.RecrutUnitView != null)
        {
            float tempvalue = _view.RecrutUnitView.GetUnitInventory().CostEquipmentinGold;
            GoldCost tempCost =
                new GoldCost(
                    _view.RecrutUnitView.GetUnitInventory().ArmorSlotsController.BootsSlot.ItemModel.CostInGold,
                    tempvalue);
            _view.TempCostInGold = tempCost;
        }
        else
        {
            _view.TempCostInGold = null;
        }
    }

    public void ChangeCostBasket()
    {
        float tempvalue = _view.RecrutUnitView.GetUnitInventory().CostEquipmentinGold;
        _view.TempCostInGold.ChangeCost(tempvalue);
        _view.BuyItemScreen.SetCurrentCost(tempvalue.ToString());
    }

    public void ItemButtonsSubscribe()
    {

    }
    public void ActiveUI(bool IsActive)
    {
        Spawn(IsActive);
        _view.RecrutUnitView.gameObject.SetActive(IsActive);
        _view.EquipController.SetActiveEquipScreen(IsActive);

        _view.BuyItemScreen.SetActiveScreen(IsActive);
    }

    public void SortingAndFillModels()
    {
        List<ItemModel> bodyArmors = new List<ItemModel>();
        List<ItemModel> helmets = new List<ItemModel>();
        foreach (ArmorModel model in _view.items.ArmorsInObject)
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

        foreach (WeaponModel model in _view.items.WeaponsInObject)
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

        _view.BuyItemScreen.SetBodyArmorContent(bodyArmors);
        _view.BuyItemScreen.SetHelmetContent(helmets);
        _view.BuyItemScreen.SetRightWeaponContent(rightWeapons);
        _view.BuyItemScreen.SetLeftWeaponContent(leftWeapons);
        _view.BuyItemScreen.SetTwoHandtWeaponContent(twoHWeapons);
    }

    public void SetPositionForHire(Transform parent)
    {
        _view.PositionForInstantiateUnit = parent;
    }

    public void HireUnit()
    {

      /*  if (_globalResStock.PriceGoldFromGlobalStock(_view.TempCostInGold))
        {
            _view.EquipController.ChangeCameraPosition();
            //Object.Instantiate(_view.RecrutUnitView.gameObject, _view.PositionForInstantiateUnit);
            
            _view.EquipController.SetCameraOnUnit();

        }
        else
        {
            _view.BuyItemScreen.SetCurrentCost($"НУЖНО БОЛЬШЕ ЗОЛОТА!");
        }*/

    }
    public void OnStart()
    {
        _factory = new BaseELFUnitFactory(new GameObjectPoolController(20, _view.RecrutPrefab));
    }

    public void Dispose()
    {
        _view.ActiveEquipUI -= ActiveUI;
        _view.equipRecruit -= EquipRecruit;
        _view.HiredRecruit -= HireReqruit;
    }

    public void Spawn(bool IsActive)
    {
        if(IsActive == true)
        {
            if (_view.RecrutUnitView == null)
            {
                var recrutPrefab = _factory.CreateUnit(_levelController.PointSpawnUnits.position);
                _view.RecrutUnitView = recrutPrefab.GetComponent<UnitView>();
            }
            else _view.RecrutUnitView.gameObject.SetActive(true);
            
            _view.EquipController.SetUnit(_view.RecrutUnitView);
            SortingAndFillModels();
            _view.EquipController.SetCameraOnUnit();
        }
        else
        {
            _view.RecrutUnitView.gameObject.SetActive(false);
        }
    }

        private void HireReqruit() // Времено добавлено ВН
        {
            _factory.CreateUnit(_levelController.PointSpawnUnits.position);
        }
    }
}
