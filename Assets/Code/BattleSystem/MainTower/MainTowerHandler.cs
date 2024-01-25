using System;
using Abstraction;
using Tools;

namespace BattleSystem.MainTower
{
    public class MainTowerHandler : IMainTower
    {
        private readonly MainTowerView _view;
        private readonly MainTowerDefenceModel _defence;
        private readonly IParametr<int> _health;

        private int _maxHealth;
        private string _id;

        public string Id => _id;
        public MainTowerView View => _view;
        
        public event Action<IMainTower> OnReachedZeroHealth;

        public MainTowerHandler(MainTowerView view, MainTowerDefenceModel defence, int durability)
        {        
            _view = view;
            _defence = defence;
            _maxHealth = durability;
            _health = new ParametrModel<int>(durability, 0, durability);

            _id = _view.Id;
        }

        public void Enable()
        {
            _health.OnValueChange += _view.ChangeHealth;
            _health.OnMinValueSet += ReachedZeroHealth;

            _view.OnTakeDamage += TakeDamage;

            _view.Show();
            _view.ChangeHealth(_health.GetValue());
        }

        public void Disable()
        {
            _health.OnValueChange -= _view.ChangeHealth;
            _health.OnMinValueSet -= ReachedZeroHealth;

            _view.OnTakeDamage -= TakeDamage;

            _view.Hide();
        }

        public void Reset()
        {
            _health.SetValue(_maxHealth);
        }

        private void TakeDamage(IDamage damage)
        {
            float resultDamageAmount = _defence.GetAfterDefDamage(damage);

            int hpLeft = _health.GetValue() - (int)resultDamageAmount;
            _health.SetValue(hpLeft);
        }


        private void ReachedZeroHealth(int value)
        {
            OnReachedZeroHealth?.Invoke(this);
        }
        
    }
}