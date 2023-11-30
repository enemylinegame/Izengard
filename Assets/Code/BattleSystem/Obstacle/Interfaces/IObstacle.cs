using Abstraction;
using System;

namespace BattleSystem.Obstacle
{
    public interface IObstacle : IDamageable
    {
        public int Id { get; }

        public IParametr<int> Health { get; }

        public IObstacleView View { get; }

        public event Action<int> OnReacheZeroHealth;

        public void Enable();
        public void Disable();
    }
}
