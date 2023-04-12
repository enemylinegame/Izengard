using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace EquipmentSystem
{ 
    public class EquipScreenView : MonoBehaviour
    {
        [SerializeField] private Button HelmetButton;
        [SerializeField] private Button ShoulderButton;
        [SerializeField] private Button RightWeaponButton;
        [SerializeField] private Button BootsButton;
        [SerializeField] private Button AccessoryButton;
        [SerializeField] private Button ArmoryButton;
        [SerializeField] private Button LeftHandButton;
        [SerializeField] private Button GlovesSlotButton;

        [SerializeField] private TextMeshProUGUI NameCharacterText;  
        [SerializeField] private TextMeshProUGUI DamageText;
        [SerializeField] private TextMeshProUGUI SpeedText;
        [SerializeField] private TextMeshProUGUI RangeAModText;
        [SerializeField] private TextMeshProUGUI DefendsText;

        public void SetButtonIcon(ItemModel model)
        {
            if (model ==null)
            {
                return;
            }
            if (model is ArmorModel)
            {
                switch (((ArmorModel)model).ArmorSlotTypeID)
                {
                    case 1:
                        ArmoryButton.image.sprite=model.Icon;
                        break;
                    case 2:
                        GlovesSlotButton.image.sprite = model.Icon;
                        break;
                    case 3:
                        BootsButton.image.sprite = model.Icon;
                        break;
                    case 4:
                        HelmetButton.image.sprite = model.Icon;
                        break;
                    case 5:
                        ShoulderButton.image.sprite = model.Icon;
                        break;
                    default:
                        break;
                }
            }
            if (model is WeaponModel)
            {
                switch (((WeaponModel)model).WeaponGripTypeID)
                {
                    case 1:
                        RightWeaponButton.image.sprite = model.Icon;
                        break;
                    case 3:
                        LeftHandButton.image.sprite = model.Icon;
                        break;
                    case 2:
                        LeftHandButton.image.sprite = model.Icon;
                        RightWeaponButton.image.sprite = null;
                        break;                   
                    default:
                        break;
                }
            }            
        }
        public void ResetWeaponButton()
        {
            RightWeaponButton.image.sprite = null;
            AccessoryButton.image.sprite = null;
            LeftHandButton.image.sprite = null;
        }
        public void SetCharacterParameters(CurrentParameters parameters)
        {
            if (parameters!=null)
            { 
            DamageText.text = $"Damage: \n{parameters.CurrentDamage.Value}";
            SpeedText.text = $"Speed: \n{parameters.CurrentSpeed.Value}";
            RangeAModText.text = $"RangeA: \n{parameters.CurrentRangeAttackMod.Value}";
            DefendsText.text = $"Defends: \n{parameters.CurrentDefends.Value}";
            }
        }
        public void SetCharacterName(string name)
        {
            NameCharacterText.text = name;
        }

    }
}
