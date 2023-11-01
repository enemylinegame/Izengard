namespace Abstraction
{
    public interface IDefence<TData, TValue>
    {
        TData DefenceData { get; }

        float GetAfterDefDamage(TValue value);
    }
}
