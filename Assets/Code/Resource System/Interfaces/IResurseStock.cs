using System.Collections.Generic;

namespace ResourceSystem
{   
    public interface IResurseStock 
    {        
        public List<ResourceHolder> ResursesInStock { get; }        

        public float GetResursesCount(ResourceType type);
        public float GetResursesInStock(ResourceType type, float count);
        public void AddResursesFromHolder(IResurseHolder _getterHolder);
        public void AddResurseHolder(ResourceHolder holder);
        public void AddResursesCount(ResourceType res, float value);






    }
}
