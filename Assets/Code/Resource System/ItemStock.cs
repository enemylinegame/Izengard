using EquipmentSystem;
using System.Collections.Generic;

namespace ResourceSystem
{ 
    [System.Serializable]
    public class ItemStock : Stock<ItemModel,ItemСarrierHolder>
    {
        public ItemStock (List<ItemСarrierHolder> models)
        {
            _holdersInStock = new List<ItemСarrierHolder>();
            for (int i =0;i<models.Count;i++)

            _holdersInStock.Add (new ItemСarrierHolder( models[i]));
        }
        public ItemStock (ItemStock itStock)
        {
            _holdersInStock = new List<ItemСarrierHolder>();
            for (int i = 0; i < itStock.HoldersInStock.Count; i++)
            { 
                _holdersInStock.Add(new ItemСarrierHolder(itStock.HoldersInStock[i]));
            }
        }
    }
}
