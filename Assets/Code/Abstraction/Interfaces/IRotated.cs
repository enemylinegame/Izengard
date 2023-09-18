namespace Izengard.Abstraction.Interfaces
{
    public interface IRotated<TValue>
    {
        TValue GetRatation();
        void SetRotation(TValue rot);
    }
}
