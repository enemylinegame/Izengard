using ResourceSystem;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResurseHolderButton : MonoBehaviour
{
    [SerializeField] private Button ResurseButton;
    [SerializeField] private ResourceConfig resourceInButton;
    [SerializeField] private TextMeshProUGUI CostText;
    [SerializeField] private TextMeshProUGUI NameItemText;


    public void SetModelInButton(ResourceConfig resource)
    {
        resourceInButton = resource;
        ChangeInfoButton();
    }
    public ResourceConfig GetModelInButton()
    {
        return resourceInButton;
    }
    public Button GetButton()
    {
        return ResurseButton;
    }
    public void ChangeInfoButton()
    {
        if (resourceInButton != null)
        {
            ResurseButton.image.sprite = resourceInButton.Icon;
           // CostText.text = resourceInButton.ResourceName;
           // CostText.text = resourceInButton.CostInGold.ToString();
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
