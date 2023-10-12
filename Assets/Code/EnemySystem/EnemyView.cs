using UnitSystem.View;
using UnityEngine;
using UnityEngine.AI;

namespace EnemySystem
{
    public class EnemyView : BaseUnitView
    {
        [SerializeField] private string _name;
        [SerializeField] private NavMeshAgent _enemyNavMesh;
        [SerializeField] private Animator _enemyAnimtor;

        public override void ChangeHealth(int hpValue)
        {
            Debug.Log($"{_name}[Helth] = {hpValue}");
        }

        public override void ChangeSize(float sizeValue)
        {
            SelfTransform.localScale = Vector3.one * sizeValue;
        }

        public override void ChangeSpeed(float speedValue)
        {
            if (UnitNavigation)
            {
                UnitNavigation.speed = speedValue;
            }
        }

        protected override void SetTransform()
        {
            _selfTransform ??= transform;
        }

        protected override void SetUnitAnimator()
        {
            _unitAnimator ??= _enemyAnimtor;
        }

        protected override void SetUnitNavigation()
        {
            _unitNavigation ??= _enemyNavMesh;
        }
    }
}
