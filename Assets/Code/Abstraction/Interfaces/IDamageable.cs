using System;

namespace Abstraction
{
    public interface IDamageable
    {
        event Action<IDamage> OnTakeDamage;

        void TakeDamage(IDamage damage);
    }
}
