using System;
using Code.Units.HireDefendersSystem;
using UnityEngine;

namespace CombatSystem
{
    public class DefenderPreview : IGetProgress
    {

        public event Action OnDefenderSet;  
        
        private DefenderSettings _settings;
        private DefenderUnit _unit;
        private HireProgress _hireProgress;

        private bool _isInBarrack;
        
        public Sprite Icon { get; private set; }

        public bool IsVisualSelection
        {
            set
            {
                if (_unit != null) _unit.IsVisualSelection = value;
            }
        }

        public bool IsInBarrack
        {
            get => _unit?.IsInBarrack ?? _isInBarrack;
            set
            {
                if (_unit == null)
                {
                    _isInBarrack = value;
                }
            }
            
        }
        

        public DefenderUnit Unit
        {
            get => _unit;
            set
            {
                if (_unit == null)
                {
                    _unit = value;
                    OnDefenderSet?.Invoke();
                }
            }
        }

        public IHealthHolder HealthHolder  => _unit?.HealthHolder ?? null;

        public bool IsInHiringProcess => _unit == null;

        public DefenderSettings Settings => _settings; 
        
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

        public void SetHireProgress(HireProgress hireProgress) => _hireProgress = hireProgress;


        #region IGetProgress

        public float GetCurrentProgress()
        {
            float progress = 0.0f;
            if (_hireProgress != null)
            {
                if (_hireProgress.TimePassed <= _hireProgress.Duration)
                {
                    progress = _hireProgress.TimePassed;
                }
                else
                {
                    progress = _hireProgress.Duration;
                }
            }
            return progress;
        }
        
        public float GetMaxProgress()
        {
            return _hireProgress?.Duration ?? 0.0f;
        }

        #endregion 
        
    }
}