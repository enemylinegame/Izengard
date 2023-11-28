using Abstraction;

namespace BattleSystem.Obstacle
{
    public interface IObstacleView : IAttackTarget
    {
        public DefenWallConfig Config { get; }

        void Show();
        void Hide();

        void ChangeHealth(int hpValue);
    }
}
