namespace ResourceSystem
{     
    public enum ResourceType
    {
        None=0,
        Wood =1,
        Iron =2,
        Deer =3,
        Horse =4,
        MagicStones =5,
        Textile =6,
        Steel =7,
        Gold =9,
    }
    
    public interface IResurseHolder:IHolder<ResurseCraft>
    {               
        
    }
}
