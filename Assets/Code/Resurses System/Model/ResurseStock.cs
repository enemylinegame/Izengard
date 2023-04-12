using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ResurseSystem
{   
    [System.Serializable]
    public class ResurseStock : Stock<ResurseCraft,ResurseHolder>
    {           
        
       
        public Action<ResurseHolder> ResursesChange;
        

        public ResurseStock(List<ResurseHolder> resurses)
        {
            _holdersInStock = new List<ResurseHolder>();
            for (int i = 0; i < resurses.Count; i++)
            {
                _holdersInStock.Add(new ResurseHolder(resurses[i]));
            }

        }
        public ResurseStock(ResurseStock stock)
        {
            _holdersInStock = new List<ResurseHolder>();
            for (int i=0;i<stock.HoldersInStock.Count;i++)
            {
                _holdersInStock.Add(new ResurseHolder(stock.HoldersInStock[i]));
            }
                        
        }

        public void AddResurseHolder(ResurseHolder holder)
        {
            _holdersInStock.Add(holder);
            ChangeValueInStock?.Invoke(holder);
        }

        public float GetResursesCount(ResurseType type)
        {
            foreach (ResurseHolder holder in _holdersInStock)
            {
                if (holder.ObjectInHolder.ResurseType == type)
                    return holder.CurrentValue;                                  
            }
            return 0;
        }        
        
        public void ChangeHoldersInStock(List<ResurseHolder> newHolders)
        {
            _holdersInStock = newHolders;
            foreach (ResurseHolder holder in _holdersInStock)
            {
                ChangeValueInStock?.Invoke(holder);
            }
        }
       
    }


}
