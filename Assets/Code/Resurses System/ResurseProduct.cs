
namespace ResurseSystem
{
    [System.Serializable]
    public class ResurseProduct : Product<ResurseCraft>
    {
        public ResurseProduct(ResurseCraft obj, float produceValue, float produceTime, ResurseCost costInResurse) :base(obj, produceValue, produceTime, costInResurse)
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
