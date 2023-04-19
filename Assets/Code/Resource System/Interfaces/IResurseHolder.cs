namespace ResourceSystem
{     
    public enum ResourceType
    {
        None,
        Wood,
        Iron,
        Deer,
        Horse,
        MagicStones,
        Textile,
        Steel,
        Gold,
    }
    
    public interface IResurseHolder:IHolder<ResurseCraft>
    {               
        
    }
}
