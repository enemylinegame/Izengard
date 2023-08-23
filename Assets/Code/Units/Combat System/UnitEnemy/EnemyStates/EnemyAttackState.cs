﻿using Wave;

namespace CombatSystem.UnitEnemy.EnemyStates
{
    public class EnemyAttackState : EnemyBaseState
    {
        public EnemyAttackState(
            Enemy unit, 
            IEnemyAnimationController animationController) : base(unit, animationController)
        {
        }

        public override void OnEnter()
        {
            _animationController.PlayAnimation(AnimationType.Attack);
        }

        public override void OnExit()
        {
            _animationController.StopAnimation();
        }

        public override void OnUpdate()
        {

        }

        public override void OnFixedUpdate()
        {

        }
    }
}
