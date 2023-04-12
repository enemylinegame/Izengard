
namespace ResourceSystem
{
    [System.Serializable]
    public class ResourceProduct : Product<ResurseCraft>
    {
        public ResourceProduct(ResurseCraft obj, float produceValue, float produceTime, ResourceCost costInResource) :base(obj, produceValue, produceTime, costInResource)
        {
            
        }
        public ResourceProduct(ResurseCraft obj, float produceValue, float produceTime) : base(obj, produceValue, produceTime)
        {

        }
        public ResourceProduct(ResourceProduct product) : base(product)
        {

        }
    }
}
