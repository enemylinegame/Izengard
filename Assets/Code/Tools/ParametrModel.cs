using System;

namespace Izengard.Tools
{
    public class ParametrModel<T> where T : IComparable<T>
    {
        private T _paramValue;
        private T _minValue;
        private T _maxValue;

        public ParametrModel(T initValue, T minValue, T maxValue)
        {
            _paramValue = initValue;
            _minValue = minValue;
            _maxValue = maxValue;
        }

        public T GetValue() => _paramValue;

        public void SetValue(T newValue)
        {
            if (newValue.CompareTo(_maxValue) > 0)
            {
                _paramValue = _maxValue;
            }
            else if (newValue.CompareTo(_minValue) < 0)
            {
                _paramValue = _minValue;
            }
            else
            {
                _paramValue = newValue;
            }
        }
    }
}
