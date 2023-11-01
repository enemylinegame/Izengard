using Abstraction;

namespace BattleSystem.Buildings.Interfaces
{
    public interface IWarBuildingsContainer
    {
        IAttackTarget GetMainTowerAsAttackTarget();
    }
}