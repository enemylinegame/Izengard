namespace Abstraction
{
    public interface INavigation<T>
    {
        void Enable();
        void Disable();

        void MoveTo(T position);
        void Stop();
    }
}
