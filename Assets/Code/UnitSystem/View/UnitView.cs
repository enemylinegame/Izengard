using Tools;
using UnityEngine;
using UnityEngine.AI;

namespace UnitSystem.View
{
    public class UnitView : BaseUnitView
    {
        [SerializeField] private NavMeshAgent _enemyNavMesh;
        [SerializeField] private Collider _collider;
        [SerializeField] private UnitAnimationView _animationView;

        public override void ChangeHealth(int hpValue)
        {
            DebugGameManager.Log($"{_name}. Health value changed. Current Health = {hpValue}",
                new []{DebugTags.Unit, DebugTags.Health});
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

        public override void SetUnitName(string name)
        {
            _name = name;
        }
    }
}
