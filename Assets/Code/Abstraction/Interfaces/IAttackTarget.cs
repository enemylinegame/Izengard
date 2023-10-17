using UnityEngine;


namespace Abstraction
{
    public interface IAttackTarget : IDamageable
    {
        Vector3 GetPosition();
    }
}