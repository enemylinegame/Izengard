using EquipmentSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine; 
using UnityEngine.UI;
using TMPro;

public class itemHolderButton : MonoBehaviour
{
    [SerializeField] private Button ItemButton;
    [SerializeField] private ItemModel ModelInButton;
    [SerializeField] private TextMeshProUGUI CostText;
    [SerializeField] private TextMeshProUGUI NameItemText;
    
    
    public void SetModelInButton(ItemModel model)
    {
        ModelInButton = model;
        ChangeInfoButton(); 
    }
    public ItemModel GetModelInButton()
    {
        return ModelInButton;
    }
    public Button GetButton()
    {
        return ItemButton;
    }
    public void ChangeInfoButton()
    {
        if (ModelInButton!=null)
        { 
            ItemButton.image.sprite = ModelInButton.Icon;
            NameItemText.text = ModelInButton.Name;
            CostText.text = ModelInButton.CostInGold.Cost.ToString();
        }
        else
        {
            SetActiveButton(false);
        }
    }
    public void Awake()
    {
        ChangeInfoButton();
    }
    public void SetActiveButton(bool value)
    {
        gameObject.SetActive(value);
    }
    
    
}
