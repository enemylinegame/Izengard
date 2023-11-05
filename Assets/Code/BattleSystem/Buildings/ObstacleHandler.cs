using Abstraction;
using BattleSystem.Buildings.Interfaces;
using System;
using Tools;

namespace BattleSystem.Buildings
{
    public class ObstacleHandler : IObstacle
    {
        private readonly int _id;
        private readonly IParametr<int> _health;
        private readonly IObstacleView _view;

        private bool _isAlive;

        public event Action<int> OnReacheZeroHealth;

        public int Id => _id;

        public IParametr<int> Health => _health;

        public IObstacleView View => _view;

        public bool IsAlive => _isAlive;

        public ObstacleHandler(int id, IObstacleView view)
        {
            _id = id;
            _view = view;

            var cfg = view.Config;
            _health = new ParametrModel<int>(cfg.HealtPoints, 0, cfg.HealtPoints);

            _view.Init(_id);

            _isAlive = true;
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
            OnReacheZeroHealth?.Invoke(Id);
        }
    }
}
