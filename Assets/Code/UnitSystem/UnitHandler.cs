using System;
using UnityEngine;

namespace Izengard.UnitSystem
{
    public class UnitHandler : IUnit, IDisposable
    {
        private readonly IUnitView _view;
        private readonly UnitModel _untiModel;

        private int _index;
        public int Index => _index;

        public IUnitView View => _view;

        public UnitModel Model => _untiModel;

        public UnitHandler(
            int index,
            IUnitView view, 
            UnitModel unitModel)
        {
            _index = index;

            _view = 
                view ?? throw new ArgumentNullException(nameof(view));

            _untiModel = 
                unitModel ?? throw new ArgumentNullException(nameof(unitModel));
        }

        public void Enable()
        {
            Subscribe();

            _view.Show();

            _view.ChangeHealth(_untiModel.Health.GetValue());
            _view.ChangeSize(_untiModel.Size.GetValue());
            _view.ChangeSpeed(_untiModel.Speed.GetValue());
            _view.ChangeArmor(_untiModel.Defence.ArmorPoints.GetValue());
        }

        private void Subscribe()
        {
            _untiModel.Health.OnValueChange += _view.ChangeHealth;
            _untiModel.Size.OnValueChange += _view.ChangeSize;
            _untiModel.Speed.OnValueChange += _view.ChangeSpeed;
            _untiModel.Defence.ArmorPoints.OnValueChange += _view.ChangeArmor;
        }

        public void Disable()
        {
            Unsubscribe();

            _view.Hide();
        }

        private void Unsubscribe()
        {
            _untiModel.Health.OnValueChange -= _view.ChangeHealth;
            _untiModel.Size.OnValueChange -= _view.ChangeSize;
            _untiModel.Speed.OnValueChange -= _view.ChangeSpeed;
            _untiModel.Defence.ArmorPoints.OnValueChange -= _view.ChangeArmor;
        }

        #region IDamageable

        public void TakeDamage(UnitDamage damageValue)
        {
            var resultDamageAmount
                = _untiModel.Defence.GetAfterDefDamage(damageValue);

            var hpLost = _untiModel.Health.GetValue() - resultDamageAmount;
            _untiModel.Health.SetValue(hpLost);
        }

        #endregion

        #region IDamageDealer

        public UnitDamage GetAttackDamage()
        {
            return _untiModel.Offence.GetDamage();
        }

        #endregion

        #region IPositioned

        public Vector3 GetPosition()
        {
            return _view.SelfTransform.position;
        }

        public void SetPosition(Vector3 pos)
        {
            _view.SelfTransform.position = pos;
        }

        #endregion

        #region IRotated

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

        #endregion

        #region IDisposable

        private bool _disposed = false;

        public void Dispose()
        {
            if (_disposed)
                return;

            Disable();

            _disposed = true;
        }

        #endregion
    }
}
