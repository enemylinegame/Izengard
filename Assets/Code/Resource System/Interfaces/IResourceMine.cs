namespace ResourceSystem
{ 
    public interface IResourceMine : IIconHolder
    {
        public string NameOfMine { get; }
        public float ExtractionTime { get; }
        public ResourceHolder ResourceHolderMine { get; }
        public void SetExtractionTime(float time);
        public ResourceHolder MineResource();
        



    }
}
