namespace Abstraction
{
    public interface IPaused
    {
        bool IsPaused { get; }

        public void OnPause();

        public void OnRelease();
    }
}
