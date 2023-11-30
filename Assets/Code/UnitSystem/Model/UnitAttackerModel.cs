using System;
using Abstraction;
using UnityEngine;

namespace UnitSystem.Model
{
    public class UnitAttackerModel : IAttacker, IDisposable
    {

        private readonly IUnit _unit;
        private IAttackTarget _target;

        public void SetTarget(IAttackTarget newTarget)
        {
            _target = newTarget;
            OnTargetChanged?.Invoke(this);
        }


        public UnitAttackerModel(IUnit unit)
        {
            _unit = unit;
        }
        
        #region IAttacker
        
        public event Action<IAttacker> OnUnityDestroyed;
        public event Action<IAttacker> OnTargetChanged;
        public IAttackTarget GetCurrentTarget()
        {
            return _target;
        }

        public IDamage GetDamagePower()
        {
            return _unit.GetAttackDamage();
        }

        public Vector3 GetPosition()
        {
            return _unit.View.SelfTransform.position;
        }

        public float GetCastTime()
        {
            //throw new NotImplementedException();
            return  1.0f / _unit.Offence.CastingTime; 
        }

        public float GetAttackTime()
        {
            //throw new NotImplementedException();
            return 1.0f / _unit.Offence.AttackTime;
        }

        public float GetMinAttackDistance()
        {
            return _unit.Offence.MinRange;
        }

        public float GetMaxAttackDistance()
        {
            return _unit.Offence.MaxRange;
        }

        public void StartCast()
        {
            //throw new NotImplementedException();
            Debug.Log("UnitAttackerModel->StartCast: " + _unit.View.SelfTransform.gameObject.name);
        }

        public void StartAutoAttack()
        {
            //throw new NotImplementedException();
            Debug.Log("UnitAttackerModel->StartAutoAttack: " + _unit.View.SelfTransform.gameObject.name);
        }

        public void StopCast()
        {
            //throw new NotImplementedException();
            Debug.Log("UnitAttackerModel->StopCast: " + _unit.View.SelfTransform.gameObject.name);
        }

        public void StopAutoAttack()
        {
            //throw new NotImplementedException();
            Debug.Log("UnitAttackerModel->StopAutoAttack: " + _unit.View.SelfTransform.gameObject.name);
        }

        public string GetName()
        {
            return _unit.View.SelfTransform.gameObject.name;
        }
        
        #endregion


        #region IDisposable

        public void Dispose()
        {
            OnUnityDestroyed?.Invoke(this);
        }

        #endregion
        
        
    }
}