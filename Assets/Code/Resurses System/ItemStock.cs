using EquipmentSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ResurseSystem
{ 
    [System.Serializable]
    public class ItemStock : Stock<ItemModel,ItemÑarrierHolder>
    {
        public ItemStock (List<ItemÑarrierHolder> models)
        {
            _holdersInStock = new List<ItemÑarrierHolder>();
            for (int i =0;i<models.Count;i++)

            _holdersInStock.Add (new ItemÑarrierHolder( models[i]));
        }
        public ItemStock (ItemStock itStock)
        {
            _holdersInStock = new List<ItemÑarrierHolder>();
            for (int i = 0; i < itStock.HoldersInStock.Count; i++)
            { 
                _holdersInStock.Add(new ItemÑarrierHolder(itStock.HoldersInStock[i]));
            }
        }
    }
}
