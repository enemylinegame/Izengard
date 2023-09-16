using System;

namespace Izengard.Abstraction.Interfaces
{
    public interface IParametr<TParam>
    {
        event Action<TParam> OnValueChange;
        TParam GetValue();
        void SetValue(TParam value);
    }
}
