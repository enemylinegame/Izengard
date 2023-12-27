using Abstraction;
using System;
using Tools;

namespace BattleSystem.Obstacle
{
    public class ObstacleHandler : IObstacle
    {
        private readonly int _id;
        private readonly IParametr<int> _health;
        private readonly IObstacleView _view;

        public event Action<int> OnReachedZeroHealth;
        public event Action<IDamage> OnTakeDamage;

        public int Id => _id;

        public IParametr<int> Health => _health;

        public IObstacleView View => _view;

        public ObstacleHandler(IObstacleView view)
        {
            _view = view;
            var cfg = view.Config;
            _health = new ParametrModel<int>(cfg.HealtPoints, 0, cfg.HealtPoints);

            _id = _view.Id;
            _view.Show();
        }

        public void Enable()
        {
            _health.OnValueChange += _view.ChangeHealth;
            _health.OnMinValueSet += ReachedZeroHealth;

            _view.Show();

            _view.ChangeHealth(_health.GetValue());
        }

        public void Disable() 
        {
            _health.OnValueChange -= _view.ChangeHealth;
            _health.OnMinValueSet -= ReachedZeroHealth;

            _view.Hide();
        }

        public void TakeDamage(IDamage damage)
        {
            var resultDamage = damage.BaseDamage;

            int hpLeft = _health.GetValue() - (int)resultDamage;
            _health.SetValue(hpLeft);
        }

        private void ReachedZeroHealth(int value)
        {
            OnReachedZeroHealth?.Invoke(Id);
        }
    }
}
