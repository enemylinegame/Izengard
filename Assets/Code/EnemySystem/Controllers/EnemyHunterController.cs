using System.Collections.Generic;
using UnitSystem;
using UnitSystem.Enum;
using UnityEngine;

namespace EnemySystem.Controllers
{
    public class EnemyHunterController : IUnitController
    {
        private List<IUnit> _unitCollection;

        private Vector3 _currentTarget;

        private bool _isEnable;

        public event System.Action<IUnit> OnUnitDone;

        public EnemyHunterController()
        {
            _unitCollection = new List<IUnit>();

            _isEnable = false;
        }

        public void Enable()
        {
            foreach (var unit in _unitCollection)
            {
                InitUnitLogic(unit);
            }

            _isEnable = true;
        }

        private void InitUnitLogic(IUnit unit)
        {
            unit.Enable();
            unit.Navigation.Enable();

            _currentTarget
                = unit.UnitPriority.GetMainTowerPosition();

            MoveUnitToTarget(unit, _currentTarget);
        }

        public void Disable()
        {
            foreach (var unit in _unitCollection)
            {
                StopUnit(unit);

                unit.Disable();
            }

            _isEnable = false;
        }
        public void AddUnit(IUnit unit)
        {
            InitUnitLogic(unit);
            _unitCollection.Add(unit);
        }

        public void RemoveUnit(int unitId)
        {
            var unit = _unitCollection.Find(u => u.Id == unitId);
            _unitCollection.Remove(unit);
        }

        public void OnUpdate(float deltaTime)
        {
            if (_isEnable == false)
                return;

            if (!_isEnable)
                return;

            if (_unitCollection.Count != 0)
            {
                for (int i = 0; i < _unitCollection.Count; i++)
                {
                    var unit = _unitCollection[i];
                }
            }
        }

        public void OnFixedUpdate(float fixedDeltaTime)
        {
            if (!_isEnable)
                return;

            if (_unitCollection.Count != 0)
            {
                for (int i = 0; i < _unitCollection.Count; i++)
                {
                    var unit = _unitCollection[i];

                    if (CheckStopDistance(unit, _currentTarget) == true)
                    {
                        StopUnit(unit);
                        OnUnitDone?.Invoke(unit);
                    }
                }
            }
        }

        private bool CheckStopDistance(IUnit unit, Vector3 currentTarget)
        {
            var distance = Vector3.Distance(unit.GetPosition(), currentTarget);
            if (distance <= unit.Model.Offence.MaxRange)
            {
                return true;
            }
            return false;
        }

        private void MoveUnitToTarget(IUnit unit, Vector3 target)
        {
            unit.Navigation.MoveTo(target);
        }

        private void StopUnit(IUnit unit)
        {
            unit.Navigation.Stop();
        }
    }
}
