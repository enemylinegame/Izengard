using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EquipmentSystem
{ 
    public class EquipScreenController : MonoBehaviour
    {
        [SerializeField] private EquipScreenView EqScreenView;
        [SerializeField] private UnitView CurrentUnit;
        [SerializeField] private Camera ScreenCamera;
        
        public EquipScreenController(EquipScreenView view,Camera camera)
        {
            CurrentUnit = null;
            EqScreenView = view;
            ScreenCamera = camera;
            EqScreenView.gameObject.SetActive(false);
        }
        public void SetCameraOnUnit()
        {
            ScreenCamera.gameObject.transform.SetParent(CurrentUnit.Get_hireCameraPosition());
            ScreenCamera.gameObject.transform.position = CurrentUnit.Get_hireCameraPosition().position;
            ScreenCamera.gameObject.transform.rotation = new Quaternion(0,0,0,0);
            ScreenCamera.gameObject.transform.rotation.SetLookRotation(CurrentUnit.transform.position);
        }
        public void ChangeCameraPosition()
        {
            ScreenCamera.gameObject.transform.SetParent(EqScreenView.transform);
            ScreenCamera.gameObject.transform.position = EqScreenView.transform.position;
        }
        public void SetUnit(UnitView newUnit)
        {
            if (CurrentUnit != newUnit)
            {
                CurrentUnit = newUnit;
                SetAllButtonIcon();
                SetCameraOnUnit();
                EqScreenView.SetCharacterParameters(newUnit.GetUnitInventory().InventoryModificationParam);
                EqScreenView.SetCharacterName(newUnit.GetNameOfCharacter());
            }
        }
        public void SetAllButtonIcon()
        {
            EqScreenView.ResetWeaponButton();
            Inventory tempInventory = CurrentUnit.GetUnitInventory();
            EqScreenView.SetButtonIcon(tempInventory.ArmorSlotsController.BodyArmorSlot.ItemModel);
            EqScreenView.SetButtonIcon(tempInventory.ArmorSlotsController.GlovesSlot.ItemModel);
            EqScreenView.SetButtonIcon(tempInventory.ArmorSlotsController.BootsSlot.ItemModel);
            EqScreenView.SetButtonIcon(tempInventory.ArmorSlotsController.HelmetSlot.ItemModel);
            EqScreenView.SetButtonIcon(tempInventory.ArmorSlotsController.ShouldersSlot.ItemModel);
            EqScreenView.SetButtonIcon(tempInventory.WeaponSlotController.RightHandSlot.ItemModel);
            EqScreenView.SetButtonIcon(tempInventory.WeaponSlotController.LeftHandSlot.ItemModel);
            EqScreenView.SetButtonIcon(tempInventory.WeaponSlotController.TwoHandSlot.ItemModel);
        }
        public void SetActiveEquipScreen(bool value)
        {
            if(value)
            {
                EqScreenView.gameObject.SetActive(true);
                ScreenCamera.gameObject.SetActive(true);
            }
            else
            {
                EqScreenView.gameObject.SetActive(false);
                ScreenCamera.gameObject.SetActive(false);
            }
        }
        public void EquipCharacter(ItemModel model)
        {            
            CurrentUnit.GetUnitInventory().ChangeItem(model);
            EqScreenView.SetCharacterParameters(CurrentUnit.GetUnitInventory().InventoryModificationParam);
            EqScreenView.SetButtonIcon(model);
        }
        public ItemModel UnequipCharacter(ItemModel model)
        {
            ItemModel tempModel=null;
            tempModel = CurrentUnit.GetUnitInventory().UnequipItem(model);
            EqScreenView.SetCharacterParameters(CurrentUnit.GetUnitInventory().InventoryModificationParam);
            return tempModel;
        }
        public void UnequipCharacter(itemHolderButton button)
        {
            ItemModel tempModel = button.GetModelInButton();
            CurrentUnit.GetUnitInventory().UnequipItem(tempModel);
            EqScreenView.SetCharacterParameters(CurrentUnit.GetUnitInventory().InventoryModificationParam);
            
        }
        public UnitView GetCurrentUnit()
        {
            return CurrentUnit;
        }

    }
}
