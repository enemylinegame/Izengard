using System;
using UnitSystem.Model;
using UnityEngine;


namespace Abstraction
{
    public interface IAttacker
    {
        event Action<IAttacker> OnUnityDestroyed;
        IAttackTarget GetCurrentTarget();
        IDamage GetDamagePower();
        Vector3 GetPosition();
        float GetCastTime();
        float GetAttackTime();
        float GetMinAttackDistance();
        float GetMaxAttackDistance();
        void StartCast();
        void StartAutoAttack();
    }
}