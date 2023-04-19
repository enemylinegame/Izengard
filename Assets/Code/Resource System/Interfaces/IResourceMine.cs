using ResourceSystem.SupportClases;

namespace ResourceSystem
{ 
    public interface IResourceMine : IIconHolder
    {
        public string NameOfMine { get; }
        public int ExtractionTime { get; }

        public int MaxMineAmount { get; }


    }
}
