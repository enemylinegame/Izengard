namespace Izengard.Abstraction.Interfaces
{
    public interface IOffence<TData, TValue>
    {
        TData OffenceData { get; }

        TValue GetDamage();
    }
}
