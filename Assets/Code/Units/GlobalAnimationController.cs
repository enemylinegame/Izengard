using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalAnimationController
{
    private AnimationView _animationView;
    public GlobalAnimationController(AnimationView animationView)
    {
        _animationView = animationView;
    }

    private void StateMachine(UnitState state, AnimationView animationView)
    {
        switch (state)
        {
            case UnitState.Attack:
                int randomNumber1 = Random.Range(1, 3);
                animationView.Animator.ResetTrigger("TakeDamage");              
                animationView.Animator.SetInteger("AttackType", randomNumber1);
                animationView.Animator.SetTrigger("AttackTrigger");
                break;
            case UnitState.Idle:
                animationView.Animator.SetBool("Idle", true);
                break;
            case UnitState.Die:
                int randomNumber2 = Random.Range(1, 3);
                animationView.Animator.SetInteger("DeathType", randomNumber2);
                animationView.Animator.SetTrigger("DeathTrigger");
                break;
            case UnitState.Walk:
                animationView.Animator.SetBool("isMoving", true);
                break;
            case UnitState.Work:
                animationView.Animator.SetBool("isWorking", true);
                break;

        }
    }


}
