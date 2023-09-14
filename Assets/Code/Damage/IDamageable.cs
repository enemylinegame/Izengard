namespace Izengard.Damage
{
    public interface IDamageable<T>
    {
        void TakeDamage(T damageValue);
    }
}
