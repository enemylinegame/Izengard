﻿using Wave;

namespace CombatSystem.UnitEnemy.EnemyStates
{
    public class EnemyIdleState : EnemyBaseState
    {
        public EnemyIdleState(
            Enemy unit, 
            IEnemyAnimationController animationController) : base(unit, animationController)
        {

        }

        public override void OnEnter()
        {
            
        }

        public override void OnExit()
        {
            
        }
    }
}
