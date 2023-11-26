﻿using System;
using Abstraction;
using BattleSystem.Buildings.Interfaces;
using Tools;
using UnitSystem;

namespace BattleSystem.Buildings
{
    public class WarBuildingHandler : IWarBuilding
    {
        private readonly IWarBuildingView _view;
        private readonly IUnitDefence _defence;
        private readonly IParametr<int> _health;

        private int _maxHealth;
        private int _id;

        public int Id => _id;
        public IWarBuildingView View => _view;
        
        public event Action<IWarBuilding> OnReachedZeroHealth;

        public WarBuildingHandler(IWarBuildingView view, IUnitDefence defence, int durability)
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