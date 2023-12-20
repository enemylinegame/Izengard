
using UnitSystem.View;
using UnityEngine;
using UnityEngine.AI;

namespace EnemySystem
{
    public class EnemyView : BaseUnitView
    {
        [SerializeField] private string _name;
        [SerializeField] private NavMeshAgent _enemyNavMesh;
        [SerializeField] private Collider _collider;
        [SerializeField] private UnitAnimationView _animationView;

        public override void ChangeHealth(int hpValue)
        {
            //Debug.Log($"EnemyView->ChangeHealth: {gameObject.name} hpValue = {hpValue}");
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
            selfTransform ??= transform;
        }

        protected override void SetUnitAnimator()
        {
            unitAnimation ??= _animationView;
        }

        protected override void SetUnitNavigation()
        {
            unitNavigation ??= _enemyNavMesh;
        }

        protected override void SetCollision()
        {
            unitCollider ??= _collider;
        }

    }
}
