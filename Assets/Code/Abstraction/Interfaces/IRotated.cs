namespace Abstraction
{
    public interface IRotated<TValue>
    {
        TValue GetRotation();
        void SetRotation(TValue rot);
    }
}
