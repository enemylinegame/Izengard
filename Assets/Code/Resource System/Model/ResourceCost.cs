using System.Collections.Generic;
using ResourceSystem.SupportClases;
using UnityEngine;

namespace ResourceSystem
{
    [System.Serializable]
    public class ResourceCost : IResourceCost
    {
        public List<ResourcePriceModel> ResourcePrice => resourcePrice;       
        public bool PricePaidFlag => _pricePaid;        

        [SerializeField]
        private List<ResourcePriceModel> resourcePrice;
        [SerializeField]
        private bool _pricePaid;
        



        public ResourceCost(List<ResourcePriceModel> resurseHolders)
        {
            resourcePrice = new List<ResourcePriceModel>();
            foreach (ResourcePriceModel holder in resurseHolders)
            {
                resourcePrice.Add(new ResourcePriceModel(holder.ResourceType,holder.Cost));
            }
            _pricePaid = false;            
        }
      /*  public ResourceCost(ResourcePriceModel cost)
        {
            resourcePrice = new List<ResourceHolder>();
            foreach (ResourcePriceModel holder in cost.ResourcePrice)
            {
                resourcePrice.Add(new ResourcePriceModel(holder.ResourceType,holder.Cost));
            }
            _pricePaid = cost.PricePaidFlag;
        }*/

        public void CheckRequiredResurses()
        {
           /* _pricePaid = true;
            foreach (ResourceHolder holder in resourcePrice)
            {
                if (holder.CurrentValue<holder.MaxValue)
                {
                    _pricePaid = false;
                    Debug.Log($"Need {holder.MaxValue-holder.CurrentValue} of {holder.ObjectInHolder.NameOFResurse} for produce or building");
                }
                
            } */           
        }
        
        public ResourceHolder AddResource(ResourceHolder holder)
        {
            
            /*foreach (ResourceHolder costholder in resourcePrice)
            {
                if (holder.ObjectInHolder.ResourceType==costholder.ObjectInHolder.ResourceType)
                {
                    costholder.AddInHolder(holder);
                    CheckRequiredResurses();
                   
                }
            }
            return holder;*/
            return null;
        }
        public void GetBackResource(ResourceStock stock)
        {
           /* foreach (ResourceHolder costHolder in resourcePrice)
            {
                stock.AddInStock(costHolder);
            }*/
        }
        public void GetNeededResource(ResourceStock stock)
        {            
            /*foreach (ResourceHolder costHolder in resourcePrice)
            {
                stock.GetFromStock(costHolder);
            }            
            CheckRequiredResurses();*/
        }
        public void ResetPaid()
        {            
           /* _pricePaid = false;
            foreach (ResourceHolder holder in resourcePrice)
            {
                holder.SetCurrentValueHolder(0);
            }
            */
        }
        public string GetCostInText()
        {
           /* string costtxt = "";
            foreach (ResourceHolder holder in resourcePrice)
            {
                costtxt+=$"{holder.ObjectInHolder.name} : {holder.MaxValue} ";
            }
            return costtxt;*/
           return null;
        }
    }
}
