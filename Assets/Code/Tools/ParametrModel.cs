using System;
using Abstraction;

namespace Tools
{
    public class ParametrModel<TParam> : IParametr<TParam>
        where TParam : IComparable<TParam>
    {
        private TParam _paramValue;
        private TParam _minValue;
        private TParam _maxValue;

        public event Action<TParam> OnValueChange;
        public event Action<TParam> OnMinValueSet;
        public ParametrModel(TParam initValue, TParam minValue, TParam maxValue)
        {
            _paramValue = initValue;
            _minValue = minValue;
            _maxValue = maxValue;
        }

        public TParam GetValue() => _paramValue;

        public void SetValue(TParam newValue)
        {
            if (newValue.CompareTo(_maxValue) > 0)
            {
                _paramValue = _maxValue;
            }
            else if (newValue.CompareTo(_minValue) <= 0)
            {
                _paramValue = _minValue;
                OnMinValueSet?.Invoke(_minValue);
            }
            else
            {
                _paramValue = newValue;
            }

            OnValueChange?.Invoke(_paramValue);
        }
    }
}
