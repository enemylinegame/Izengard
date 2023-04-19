using ResourceSystem;
using UnityEngine;
using UnityEngine.UI;

public class ProduceItemButtonView : MonoBehaviour
{    
    [SerializeField] public Button producedObjButton;    
    [SerializeField] private ItemProduct ItemProductInButton;
    [SerializeField] private ResurseProduct ResurseProductInButton;

    public void SetProductInButton(ItemProduct product)
    {
        if (product!=ItemProductInButton)
        { 
            ItemProductInButton = product;            
            SetButtonView(product);
            ResurseProductInButton = null;
        }
        else
        {
            if (ItemProductInButton != null)
            {
                SetButtonView(product);
            }
        }
    }
    public void SetProductInButton(ResurseProduct product)
    {
        if (product!= ResurseProductInButton)
        { 
            ResurseProductInButton = product;
            SetButtonView(product);
            ItemProductInButton = null;
        }
        else
        {
            if (ResurseProductInButton!=null)
            {
                SetButtonView(product);
            }
        }
    }
    public void SetButtonView(ItemProduct product)
    {
        if (producedObjButton.image.sprite != product.ObjectProduct.Icon)
        {
            producedObjButton.image.sprite = product.ObjectProduct.Icon;
        }
        SetActiveButton(true);
    }
    public void SetButtonView(ResurseProduct product)
    {
        if (producedObjButton.image.sprite != product.ObjectProduct.Icon)
        {
            producedObjButton.image.sprite = product.ObjectProduct.Icon;
        }
        SetActiveButton(true);

    }
    public void SetActiveButton(bool value)
    {
        if (value)
        {
            gameObject.SetActive(true);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
    public ItemProduct GetCurrentItemProductInButton()
    {
        
            return ItemProductInButton;
        
    }
    public ResurseProduct GetCurrentResurseProductInButton()
    {
       
            return ResurseProductInButton;
        
    }
    public void OnDisable()
    {
        
    }
}
