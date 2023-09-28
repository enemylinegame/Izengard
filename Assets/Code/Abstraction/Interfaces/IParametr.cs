using System;

namespace Abstraction
{
    public interface IParametr<TParam>
    {
        event Action<TParam> OnValueChange;
        TParam GetValue();
        void SetValue(TParam value);
    }
}
