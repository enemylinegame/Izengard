using UnityEngine;

namespace ResurseSystem
{     
    public enum ResurseType
    {
        Wood,
        Iron,
        Deer,
        Horse,
        MagikStones,
        Textile,
        Steele,
        Gold,
    }
    
    public interface IResurseHolder:IHolder<ResurseCraft>
    {               
        
    }
}
