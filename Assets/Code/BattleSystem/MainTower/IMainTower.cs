using Abstraction;


namespace BattleSystem.MainTower
{
    public interface IMainTower : IKillable<IMainTower>
    {
        string Id { get; }
        
        
        void Enable();

        void Disable();
    }
}