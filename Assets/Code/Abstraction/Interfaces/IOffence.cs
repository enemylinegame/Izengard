namespace Abstraction
{
    public interface IOffence<TData, TValue>
    {
        TData OffenceData { get; }

        TValue GetDamage();
    }
}
