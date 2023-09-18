using System;
using Izengard.Abstraction.Interfaces;
using Izengard.UnitSystem.View;
using UnityEngine;

namespace Izengard.UnitSystem
{
    public class UnitHandler : 
        IDamageable<UnitDamage>, 
        IDamageDealer<UnitDamage>, 
        IPositioned<Vector3>, 
        IRotated<Vector3>,
        IDisposable
    {
        private readonly BaseUnitView _view;
        private readonly IUnit _untiModel;

        public UnitHandler(BaseUnitView view, IUnit unitModel)
        {
            _view = 
                view ?? throw new ArgumentNullException(nameof(view));

            _untiModel = 
                unitModel ?? throw new ArgumentNullException(nameof(unitModel));

            Subscribe();
        }

        public void TakeDamage(UnitDamage damageValue)
        {
            var resultDamageAmount
                = _untiModel.Defence.GetAfterDefDamage(damageValue);

            var hpLost = _untiModel.Health.GetValue() - resultDamageAmount;
            _untiModel.Health.SetValue(hpLost);
        }

        public UnitDamage GetAttackDamage()
        {
            return _untiModel.Offence.GetDamage();
        }

        public Vector3 GetPosition()
        {
            return _view.SelfTransform.position;
        }

        public void SetPosition(Vector3 pos)
        {
            _view.SelfTransform.position = pos;
        }

        public Vector3 GetRatation()
        {
            var angleVector = _view.SelfTransform.rotation.eulerAngles;
            return angleVector;
        }

        public void SetRotation(Vector3 rotation)
        {
            var newRotation = Quaternion.Euler(rotation);
            _view.SelfTransform.rotation = newRotation;
        }

        public void Dispose()
        {
            Unsubscribe();
        }

        private void Subscribe()
        {
            _untiModel.Health.OnValueChange += _view.ChangeHealth;
            _untiModel.Size.OnValueChange += _view.ChangeSize;

            _untiModel.Defence.ArmorPoints.OnValueChange += _view.ChangeArmor;
        }

        private void Unsubscribe() 
        {
            _untiModel.Health.OnValueChange -= _view.ChangeHealth;
            _untiModel.Size.OnValueChange -= _view.ChangeSize;

            _untiModel.Defence.ArmorPoints.OnValueChange -= _view.ChangeArmor;
        }
    }
}
