using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ResurseSystem
{ 
    [System.Serializable]
    public abstract class Stock<T,M> where T:ScriptableObject where M : Holder<T>
        
    {
        public List<M> HoldersInStock => _holdersInStock;
        public System.Action<M> ChangeValueInStock;


        [SerializeField] protected List<M> _holdersInStock;
        

        public void AddInStock(M addingHolder)
        {
            foreach (M holder in _holdersInStock)
            {
                if (addingHolder.ObjectInHolder==holder.ObjectInHolder)
                {
                    holder.AddInHolder(addingHolder);
                    ChangeValueInStock?.Invoke(holder);
                    return;
                }                
            }
        }
        public void AddInStock(T obj,float value)
        {
            foreach (M holder in _holdersInStock)
            {
                if (obj==holder.ObjectInHolder)
                {
                    holder.AddInHolder(obj, value);
                    break;
                }                
            }
        }
        public void GetFromStock(M gettingHolder)
        {
            foreach (M holder in _holdersInStock)
            {                
                if (gettingHolder.ObjectInHolder == holder.ObjectInHolder)
                {
                    holder.GetFromHolder(gettingHolder);
                    ChangeValueInStock?.Invoke(holder);
                    return;
                }
            }
        }
        public void CompileStocks(Stock<T,M> stock)
        {
            foreach (M compholder in _holdersInStock)
            {
                foreach (M adderholder in stock._holdersInStock)
                {
                    if (compholder.ObjectInHolder == adderholder.ObjectInHolder)
                    {
                        compholder.SetMaxValueHolder(compholder.MaxValue + adderholder.MaxValue);
                        compholder.AddInHolder(adderholder);
                        break;
                    }
                }
            }
        }
        public void ResetStockHoldersValue()
        {
            foreach (M holder in _holdersInStock)
            {
                holder.SetCurrentValueHolder(0);
            }
        }
        public void AddProductInStock(Product<T> product)
        {
            foreach (M holder in _holdersInStock)
            {
                if(holder.ObjectInHolder==product.ObjectProduct)
                {
                    holder.AddInHolder(product.ObjectProduct, product.ProduceValue);
                }
            }
        }


    }
}
