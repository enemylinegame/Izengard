namespace Izengard.Abstraction.Interfaces
{
    public interface IRotated<TValue>
    {
        TValue GetRotation();
        void SetRotation(TValue rot);
    }
}
