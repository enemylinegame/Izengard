using Abstraction;


namespace BattleSystem.MainTower
{
    public interface IMainTower : IKillable<IMainTower>
    {
        int Id { get; }
        
        
        void Enable();

        void Disable();
    }
}