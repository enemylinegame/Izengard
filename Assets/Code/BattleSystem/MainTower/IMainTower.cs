using System;


namespace BattleSystem.MainTower
{
    public interface IMainTower
    {
        int Id { get; }
        
       // IWarBuildingView View { get; }
        
        event Action<IMainTower> OnReachedZeroHealth;
        
        void Enable();

        void Disable();
    }
}