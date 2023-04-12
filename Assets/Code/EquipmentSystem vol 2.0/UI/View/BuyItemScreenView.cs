using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using EquipmentSystem;
using TMPro;
using ResourceSystem;

public class BuyItemScreenView : MonoBehaviour
{
    [SerializeField] private Button RightWeaponContentButton;
    [SerializeField] private Button LeftWeaponContentButtons;
    [SerializeField] private Button TwoHandWeaponContentButtons;
    [SerializeField] private Button BodyArmorContentButtons;
    [SerializeField] private Button HelmetContentButtons;
    [SerializeField] private Button ResursesContentButton;

    [SerializeField] private List<itemHolderButton> RightWeaponButtons;
    [SerializeField] private List<itemHolderButton> LeftWeaponButtons;
    [SerializeField] private List<itemHolderButton> TwoHandWeaponButtons;
    [SerializeField] private List<itemHolderButton> BodyArmorButtons;
    [SerializeField] private List<itemHolderButton> HelmetButtons;
    [SerializeField] private List<ResurseHolderButton> ResurseButtons;

    [SerializeField] private GameObject RightWeaponContent;
    [SerializeField] private GameObject LeftWeaponContent;
    [SerializeField] private GameObject TwoHandWeaponContent;
    [SerializeField] private GameObject BodyArmorContent;
    [SerializeField] private GameObject HelmetContent;
    [SerializeField] private GameObject ResurseContent;

    [SerializeField] private Button HireButton;
    [SerializeField] private Button CancelButton;

    [SerializeField] private TextMeshProUGUI currentCostTxt;

    #region Назначение кнопок предметов
    public void SetRightWeaponContent (List<ItemModel> models)
    {
        UnActiveButtons(RightWeaponButtons);
        if (RightWeaponButtons.Count >= models.Count)
        { 
            for (int i=0; i < models.Count; i++)
            {
                if (RightWeaponButtons[i].GetModelInButton()!=models[i])
                { 
                    RightWeaponButtons[i].SetModelInButton(models[i]);
                }
                RightWeaponButtons[i].SetActiveButton(true);
            }
        }
    }
    public void SetLeftWeaponContent(List<ItemModel> models)
    {
        UnActiveButtons(LeftWeaponButtons);
        if (LeftWeaponButtons.Count >= models.Count)
        {
            for (int i = 0; i < models.Count; i++)
            {
                if (LeftWeaponButtons[i].GetModelInButton() != models[i])
                {
                    LeftWeaponButtons[i].SetModelInButton(models[i]);
                }
                LeftWeaponButtons[i].SetActiveButton(true);
            }
        }
    }
    public void SetTwoHandtWeaponContent(List<ItemModel> models)
    {
        UnActiveButtons(TwoHandWeaponButtons);
        if (TwoHandWeaponButtons.Count >= models.Count)
        {
            for (int i = 0; i < models.Count; i++)
            {
                if (TwoHandWeaponButtons[i].GetModelInButton() != models[i])
                {
                    TwoHandWeaponButtons[i].SetModelInButton(models[i]);
                }
                TwoHandWeaponButtons[i].SetActiveButton(true);
            }
        }
    }
    public void SetBodyArmorContent(List<ItemModel> models)
    {
        UnActiveButtons(BodyArmorButtons);
        if (BodyArmorButtons.Count >= models.Count)
        {
            for (int i = 0; i < models.Count; i++)
            {
                if (BodyArmorButtons[i].GetModelInButton() != models[i])
                {
                    BodyArmorButtons[i].SetModelInButton(models[i]);
                }
                BodyArmorButtons[i].SetActiveButton(true);
            }
        }
    }
    public void SetHelmetContent(List<ItemModel> models)
    {
        UnActiveButtons(HelmetButtons);
        if (HelmetButtons.Count >= models.Count)
        {
            for (int i = 0; i < models.Count; i++)
            {
                if (HelmetButtons[i].GetModelInButton() != models[i])
                {
                    HelmetButtons[i].SetModelInButton(models[i]);
                }
                HelmetButtons[i].SetActiveButton(true);
            }
        }
    }


    #endregion

    #region Изменение состояния видимости контента предметов
    public void SetActiveRightWeaponContent()
    {
        UnActiveContent();
        RightWeaponContent.SetActive(true);

    }
    public void SetActiveLeftWeaponContent()
    {
        UnActiveContent();
        LeftWeaponContent.SetActive(true);

    }
    public void SetActiveTwoHandWeaponContent()
    {
        UnActiveContent();
        TwoHandWeaponContent.SetActive(true);

    }
    public void SetActiveBodyArmorContent()
    {
        UnActiveContent();
        BodyArmorContent.SetActive(true);

    }
    public void SetActiveHelmetContent()
    {
        UnActiveContent();
        HelmetContent.SetActive(true);

    }
    #endregion
    public void UnActiveButtons(List<itemHolderButton> holderButtons)
    {
        foreach(itemHolderButton button in holderButtons)
        {
            button.SetActiveButton(false);
        }
    }
    public void UnActiveButtons(List<ResurseHolderButton> holderButtons)
    {
        foreach (ResurseHolderButton button in holderButtons)
        {
            button.SetActiveButton(false);
        }
    }

    public void SetActiveScreen(bool value)
    {
        gameObject.SetActive(value);
    }
    public void SetActiveHireScreen()
    {
        ResursesContentButton.gameObject.SetActive(false);
        RightWeaponContentButton.gameObject.SetActive(true);
        LeftWeaponContentButtons.gameObject.SetActive(true);
        TwoHandWeaponContentButtons.gameObject.SetActive(true);
        BodyArmorContentButtons.gameObject.SetActive(true);
        HelmetContentButtons.gameObject.SetActive(true);

    }
    
    public void UnActiveContent()
    {
        RightWeaponContent.SetActive(false);
        LeftWeaponContent.SetActive(false);
        TwoHandWeaponContent.SetActive(false);
        BodyArmorContent.SetActive(false);
        HelmetContent.SetActive(false);
        ResurseContent.SetActive(false);
        
    }
    public void SetCurrentCost(string cost)
    {
        currentCostTxt.text = cost;
    }
    public void DisableAllWeaponTypeButtons()
    {
        RightWeaponContentButton.gameObject.SetActive(false);
        LeftWeaponContentButtons.gameObject.SetActive(false);
        TwoHandWeaponContentButtons.gameObject.SetActive(false);
        BodyArmorContentButtons.gameObject.SetActive(false);
        HelmetContentButtons.gameObject.SetActive(false);
    }
    public void SetActiveResurseMarketSpace()
    {
        SetActiveScreen(true);
        UnActiveContent();
        ResurseContent.SetActive(true);
        DisableAllWeaponTypeButtons();
        ResursesContentButton.gameObject.SetActive(true);        
        UnActiveButtons(ResurseButtons);
    }
    public void SetResursesContent(List<ResurseCraft> products)
    {
        SetActiveResurseMarketSpace();
        if (ResurseButtons.Count <= products.Count)
        {
            for (int i = 0; i < products.Count; i++)
            {
                if (ResurseButtons[i].GetModelInButton() != products[i])
                {
                    ResurseButtons[i].SetModelInButton(products[i]);
                }
                ResurseButtons[i].SetActiveButton(true);
            }
        }
    }

}
