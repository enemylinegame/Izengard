using ResourceSystem;
using UnityEngine;
using UnityEngine.UI;

public class ProduceItemButtonView : MonoBehaviour

//TODO: REMOVE
{    
    [SerializeField] public Button producedObjButton;    
  //  [SerializeField] private ItemProduct ItemProductInButton;
   // [SerializeField] private ResurseProduct ResurseProductInButton;
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
}
