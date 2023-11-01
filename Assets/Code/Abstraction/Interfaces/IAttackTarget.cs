using UnityEngine;


namespace Abstraction
{
    public interface IAttackTarget : IDamageable
    {
        int Id { get; }
        Vector3 Position { get; }
        //bool IsAlive { get; }             // from IDamageable
        //void TakeDamage(IDamage damage);  // from IDamageable
    }
}