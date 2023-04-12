using System.Collections.Generic;
using UnityEngine;

namespace ResurseSystem
{
    [System.Serializable]
    public class ResurseCost : IResurseCost
    {
        public List<ResurseHolder> CostInResurse => _costInResurse;       
        public bool PricePaidFlag => _pricePaid;        

        [SerializeField]
        private List<ResurseHolder> _costInResurse;
        [SerializeField]
        private bool _pricePaid;
        



        public ResurseCost(List<ResurseHolder> resurseHolders)
        {
            _costInResurse = new List<ResurseHolder>();
            foreach (ResurseHolder holder in resurseHolders)
            {
                _costInResurse.Add(new ResurseHolder(holder));
            }
            _pricePaid = false;            
        }
        public ResurseCost(ResurseCost cost)
        {
            _costInResurse = new List<ResurseHolder>();
            foreach (ResurseHolder holder in cost.CostInResurse)
            {
                _costInResurse.Add(new ResurseHolder(holder));
            }
            _pricePaid = cost.PricePaidFlag;
        }

        public void CheckRequiredResurses()
        {
            _pricePaid = true;
            foreach (ResurseHolder holder in _costInResurse)
            {
                if (holder.CurrentValue<holder.MaxValue)
                {
                    _pricePaid = false;
                    Debug.Log($"Need {holder.MaxValue-holder.CurrentValue} of {holder.ObjectInHolder.NameOFResurse} for produce or building");
                }
                
            }            
        }
        
        public ResurseHolder AddResurse(ResurseHolder holder)
        {
            
            foreach (ResurseHolder costholder in _costInResurse)
            {
                if (holder.ObjectInHolder.ResurseType==costholder.ObjectInHolder.ResurseType)
                {
                    costholder.AddInHolder(holder);
                    CheckRequiredResurses();
                   
                }
            }
            return holder;
        }
        public void GetBackResurse(ResurseStock stock)
        {
            foreach (ResurseHolder costHolder in _costInResurse)
            {
                stock.AddInStock(costHolder);
            }
        }
        public void GetNeededResurse(ResurseStock stock)
        {            
            foreach (ResurseHolder costHolder in _costInResurse)
            {
                stock.GetFromStock(costHolder);
            }            
            CheckRequiredResurses();
        }
        public void ResetPaid()
        {            
            _pricePaid = false;
            foreach (ResurseHolder holder in _costInResurse)
            {
                holder.SetCurrentValueHolder(0);
            }
            
        }
        public string GetCostInText()
        {
            string costtxt = "";
            foreach (ResurseHolder holder in _costInResurse)
            {
                costtxt+=$"{holder.ObjectInHolder.name} : {holder.MaxValue} ";
            }
            return costtxt;
        }
    }
}
