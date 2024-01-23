using Tools;
using UnityEngine;
using UnityEngine.AI;

namespace UnitSystem.View
{
    public class UnitView : BaseUnitView
    {
        [SerializeField] private string _name;
        [SerializeField] private NavMeshAgent _enemyNavMesh;
        [SerializeField] private Collider _collider;
        [SerializeField] private UnitAnimationView _animationView;

        public override void ChangeHealth(int hpValue)
        {
            DebuGameManager.Log($"{_name} - hpValue = {hpValue}", new string[] {"Unit", "Health"});
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
