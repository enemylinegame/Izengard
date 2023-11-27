using Abstraction;

namespace BattleSystem.Buildings.Interfaces
{
    public interface IWarBuildingView : IAttackTarget
    {
        void Show();
        void Hide();
        void ChangeHealth(int hpValue);
    }
}