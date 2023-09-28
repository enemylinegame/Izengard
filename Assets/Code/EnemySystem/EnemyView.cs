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
            Debug.Log($"{_name}[Size] = {sizeValue}");
            SelfTransform.localScale = Vector3.one * sizeValue;
        }

        public override void ChangeSpeed(float speedValue)
        {
            Debug.Log($"{_name}[Speed] = {speedValue}");

            if (UnitNavigation)
            {
                UnitNavigation.speed = speedValue;
            }
        }

        protected override void OnSetTransform()
        {
            _selfTransform ??= transform;
        }

        protected override void OnSetUnitAnimator()
        {
            _unitAnimator ??= _enemyAnimtor;
        }

        protected override void OnSetUnitNavigation()
        {
            _unitNavigation ??= _enemyNavMesh;
        }
    }
}
