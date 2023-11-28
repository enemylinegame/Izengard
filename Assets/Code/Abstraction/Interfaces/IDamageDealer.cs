using System;

namespace Abstraction
{
    public interface IDamageDealer
    {
        event Action<IDamageDealer, IDamageable> OnAttackProcessEnd;

        IDamage GetAttackDamage();

        void StartAttack(IDamageable damageableTarget);
    }
}
