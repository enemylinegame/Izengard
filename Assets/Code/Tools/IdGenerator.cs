using Abstraction;

namespace Tools
{
    public class IdGenerator : IIdGenerator
    {
        private int _nextId = 1;
        
        public int GetNext()
        {
            return _nextId++;
        }
    }
}