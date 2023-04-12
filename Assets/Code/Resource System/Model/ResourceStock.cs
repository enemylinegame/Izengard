using System;
using System.Collections.Generic;

namespace ResourceSystem
{   
    [System.Serializable]
    public class ResourceStock : Stock<ResurseCraft,ResourceHolder>
    {           
        
       
        public Action<ResourceHolder> ResursesChange;
        

        public ResourceStock(List<ResourceHolder> resurses)
        {
            _holdersInStock = new List<ResourceHolder>();
            for (int i = 0; i < resurses.Count; i++)
            {
                _holdersInStock.Add(new ResourceHolder(resurses[i]));
            }

        }
        public ResourceStock(ResourceStock stock)
        {
            _holdersInStock = new List<ResourceHolder>();
            for (int i=0;i<stock.HoldersInStock.Count;i++)
            {
                _holdersInStock.Add(new ResourceHolder(stock.HoldersInStock[i]));
            }
                        
        }

        public void AddResurseHolder(ResourceHolder holder)
        {
            _holdersInStock.Add(holder);
            ChangeValueInStock?.Invoke(holder);
        }

        public float GetResursesCount(ResourceType type)
        {
            foreach (ResourceHolder holder in _holdersInStock)
            {
                if (holder.ObjectInHolder.ResourceType == type)
                    return holder.CurrentValue;                                  
            }
            return 0;
        }        
        
        public void ChangeHoldersInStock(List<ResourceHolder> newHolders)
        {
            _holdersInStock = newHolders;
            foreach (ResourceHolder holder in _holdersInStock)
            {
                ChangeValueInStock?.Invoke(holder);
            }
        }
       
    }


}
