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

        public EnemyHunterController()
        {
            _unitCollection = new List<IUnit>();

            /*if (_unit.Model.Offence.AttackType == UnitAttackType.Melee)
            {
                _enemyStopDistance = _unit.Model.Offence.MinRange;
            }
            else if (_unit.Model.Offence.AttackType == UnitAttackType.Range) 
            {
                _enemyStopDistance = _unit.Model.Offence.MaxRange;
            }*/

            //_unit.View.OnPulledInFight += OnPullInFight;

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
            if (unit.Model.Role != UnitRoleType.Hunter)
                return;

            InitUnitLogic(unit);
            _unitCollection.Add(unit);
        }

        public void RemoveUnit(int unitId)
        {
            var unit = _unitCollection.Find(u => u.Id == unitId);
           // unit.Disable();
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
                        Debug.Log($"EnemyHunter[{unit.Id}] reached trget");
                        RemoveUnit(unit.Id);
                    }
                }
            }
        }


      /*  private bool IsGetIntoFight()
        {
            var selfPos = _unit.GetPosition();
            var colliders = Physics.OverlapSphere(selfPos, _enemyStopDistance);
            if (colliders.Length != 0)
            {
                foreach (var collider in colliders)
                {
                    var go = collider.gameObject;
                    if (go.layer != ENEMY_LAYER && go.TryGetComponent<IUnitView>(out var unit))
                    {
                        if (unit.IsFighting == false) 
                        {
                            unit.PullIntoFight();
                            return true;
                        }
                    }
                }
            }

            return false;
        }*/

      /*  private Vector3 CheckForUnitsInRange()
        {
            var selfPos = _unit.GetPosition();
            var range = Mathf.Max(_unit.Model.DetectionRange.GetValue(), _enemyStopDistance);
            var colliders = Physics.OverlapSphere(selfPos, range);
            if (colliders.Length != 0)
            {
                foreach (var collider in colliders)
                {
                    var go = collider.gameObject;
                    if (go.layer != ENEMY_LAYER && go.TryGetComponent<IUnitView>(out var unit))
                    {
                        if (unit.IsFighting == false)
                            return unit.SelfTransform.position;
                    }
                }
            }

            return _currentTarget;
        }*/

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
