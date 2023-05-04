using UnityEngine;


namespace CombatSystem
{
    public interface IDefenderUnitView
    {
        Sprite GetSprite();
        bool IsInsideBarrack { get; }
    }
}
