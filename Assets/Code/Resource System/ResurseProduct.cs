
namespace ResourceSystem
{
    [System.Serializable]
    public class ResurseProduct : Product<ResurseCraft>
    {
        public ResurseProduct(ResurseCraft obj, float produceValue, float produceTime, ResourceCost costInResource) :base(obj, produceValue, produceTime, costInResource)
        {
            
        }
        public ResurseProduct(ResurseCraft obj, float produceValue, float produceTime) : base(obj, produceValue, produceTime)
        {

        }
        public ResurseProduct(ResurseProduct product) : base(product)
        {

        }
    }
}
