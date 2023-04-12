using ResurseSystem;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResurseHolderButton : MonoBehaviour
{
    [SerializeField] private Button ResurseButton;
    [SerializeField] private ResurseCraft ResurseInButton;
    [SerializeField] private TextMeshProUGUI CostText;
    [SerializeField] private TextMeshProUGUI NameItemText;


    public void SetModelInButton(ResurseCraft resurse)
    {
        ResurseInButton = resurse;
        ChangeInfoButton();
    }
    public ResurseCraft GetModelInButton()
    {
        return ResurseInButton;
    }
    public Button GetButton()
    {
        return ResurseButton;
    }
    public void ChangeInfoButton()
    {
        if (ResurseInButton != null)
        {
            ResurseButton.image.sprite = ResurseInButton.Icon;
            CostText.text = ResurseInButton.NameOFResurse;
            CostText.text = ResurseInButton.CostInGold.ToString();
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
