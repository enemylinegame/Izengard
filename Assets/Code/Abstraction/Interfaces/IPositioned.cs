namespace Abstraction
{
    public interface IPositioned<TValue>
    {
        TValue GetPosition();
        void SetPosition(TValue pos);
    }
}
