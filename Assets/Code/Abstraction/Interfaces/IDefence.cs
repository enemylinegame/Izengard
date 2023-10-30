namespace Abstraction
{
    public interface IDefence<TData, TValue>
    {
        TData DefenceData { get; }

        int GetAfterDefDamage(TValue value);
    }
}
