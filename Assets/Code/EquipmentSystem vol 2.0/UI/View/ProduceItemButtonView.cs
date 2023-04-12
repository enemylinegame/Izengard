using ResourceSystem;
using UnityEngine;
using UnityEngine.UI;

public class ProduceItemButtonView : MonoBehaviour
{    
    [SerializeField] public Button producedObjButton;    
    [SerializeField] private ItemProduct ItemProductInButton;
    [SerializeField] private ResourceProduct resourceProductInButton;

    public void SetProductInButton(ItemProduct product)
    {
        if (product!=ItemProductInButton)
        { 
            ItemProductInButton = product;            
            SetButtonView(product);
            resourceProductInButton = null;
        }
        else
        {
            if (ItemProductInButton != null)
            {
                SetButtonView(product);
            }
        }
    }
    public void SetProductInButton(ResourceProduct product)
    {
        if (product!= resourceProductInButton)
        { 
            resourceProductInButton = product;
            SetButtonView(product);
            ItemProductInButton = null;
        }
        else
        {
            if (resourceProductInButton!=null)
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
    public void SetButtonView(ResourceProduct product)
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
    public ResourceProduct GetCurrentResurseProductInButton()
    {
       
            return resourceProductInButton;
        
    }
    public void OnDisable()
    {
        
    }
}
