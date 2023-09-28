using Abstraction;
using System;
using UnityEngine;
namespace UnitSystem
{
    public class UnitHandler : IUnit, IDisposable
    {
        private readonly IUnitView _view;
        private readonly UnitModel _model;
        private readonly INavigation<Vector3> _navigation;

        private int _id;
        public int Id => _id;

        public IUnitView View => _view;

        public UnitModel Model => _model;

        public INavigation<Vector3> Navigation => _navigation;

        public UnitHandler(
            int index,
            IUnitView view, 
            UnitModel unitModel,
            INavigation<Vector3> navigation)
        {
            _id = index;

            _view = 
                view ?? throw new ArgumentNullException(nameof(view));

            _model = 
                unitModel ?? throw new ArgumentNullException(nameof(unitModel));

            _navigation = 
                navigation ?? throw new ArgumentNullException(nameof(navigation));

            _view.Hide();
        }

        public void Enable()
        {
            Subscribe();

            _view.Show();

            _view.ChangeHealth(_model.Health.GetValue());
            _view.ChangeSize(_model.Size.GetValue());
            _view.ChangeSpeed(_model.Speed.GetValue());
        }

        private void Subscribe()
        {
            _model.Health.OnValueChange += _view.ChangeHealth;
            _model.Size.OnValueChange += _view.ChangeSize;
            _model.Speed.OnValueChange += _view.ChangeSpeed;
        }

        public void Disable()
        {
            Unsubscribe();

            _view.Hide();

            _navigation.Disable();
        }

        private void Unsubscribe()
        {
            _model.Health.OnValueChange -= _view.ChangeHealth;
            _model.Size.OnValueChange -= _view.ChangeSize;
            _model.Speed.OnValueChange -= _view.ChangeSpeed;
        }

        #region IDamageable

        public void TakeDamage(IDamage damageValue)
        {
            var resultDamageAmount
                = _model.Defence.GetAfterDefDamage(damageValue);

            var hpLost = _model.Health.GetValue() - resultDamageAmount;
            _model.Health.SetValue(hpLost);
        }

        #endregion

        #region IDamageDealer

        public IDamage GetAttackDamage()
        {
            return _model.Offence.GetDamage();
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

        public Vector3 GetRotation()
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
