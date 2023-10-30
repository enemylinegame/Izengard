using System;

namespace Abstraction
{
    public interface IParametr<TParam>
    {
        event Action<TParam> OnValueChange;
        event Action<TParam> OnMinValueSet;
        
        TParam GetValue();
        void SetValue(TParam value);
    }
}
