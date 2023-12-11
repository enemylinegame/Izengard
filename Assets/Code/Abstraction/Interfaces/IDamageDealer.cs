namespace Abstraction
{
    public interface IDamageDealer
    {
        IDamage GetAttackDamage();

        void StartAttack(IDamageable damageableTarget);
    }
}
