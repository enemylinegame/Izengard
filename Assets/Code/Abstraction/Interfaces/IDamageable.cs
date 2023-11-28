namespace Abstraction
{
    public interface IDamageable
    {
        bool IsAlive { get; }
        void TakeDamage(IDamage damage);
    }
}
