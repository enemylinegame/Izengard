using System;


namespace BattleSystem.Buildings.Interfaces
{
    public interface IWarBuilding 
    {
        int Id { get; }
        
        IWarBuildingView View { get; }
        
        event Action<IWarBuilding> OnReachedZeroHealth;
        
        void Enable();

        void Disable();
    }
}