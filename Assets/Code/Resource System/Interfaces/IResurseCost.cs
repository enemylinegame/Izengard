using System.Collections.Generic;
using ResourceSystem.SupportClases;

namespace ResourceSystem
{ 

    public interface IResourceCost 
    {
        public List<ResourcePriceModel> ResourcePrice { get; }
        public bool PricePaidFlag { get; }

        public void CheckRequiredResurses();
        public ResourceHolder AddResource(ResourceHolder holder);
        public void GetNeededResource(ResourceStock stock);
        public void ResetPaid();

    }
}
