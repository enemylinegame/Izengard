using System;
using UnityEngine;

namespace CombatSystem
{
    public class DefenderPreview
    {

        private DefenderSettings _settings;
        private DefenderUnit _unit;
        
        public Sprite Icon { get; private set; }

        public bool IsVisualSelection
        {
            set
            {
                if (_unit != null) _unit.IsVisualSelection = value;
            }
        }

        public bool IsInBarrack => _unit?.IsInBarrack ?? false;
        

        public DefenderUnit Unit
        {
            get => _unit;
            set
            {
                if (_unit == null)
                {
                    _unit = value;
                }
            }
        }

        public IHealthHolder HealthHolder  => _unit?.HealthHolder ?? null;

        public DefenderPreview(DefenderSettings settings)
        {
            _settings = settings;
            Icon = settings.Icon;
        }

        public void AddHeathListener(Action<float, float> listener)
        {
            if (_unit != null)
            {
                _unit.OnHealthChanged += listener;
            }
        }
        
        public void RemoveHeathListener(Action<float, float> listener)
        {
            if (_unit != null)
            {
                _unit.OnHealthChanged -= listener;
            }
        }

    }
}