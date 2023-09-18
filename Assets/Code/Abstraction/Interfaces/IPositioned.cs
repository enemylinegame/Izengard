namespace Izengard.Abstraction.Interfaces
{
    public interface IPositioned<TValue>
    {
        TValue GetPosition();
        void SetPosition(TValue pos);
    }
}
