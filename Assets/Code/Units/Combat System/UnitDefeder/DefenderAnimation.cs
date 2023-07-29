using UnityEngine;

namespace CombatSystem
{
    public class DefenderAnimation
    {
        private Animator _animator;

        private DefenderState _state;
        private readonly int _attackTriggerProperty = Animator.StringToHash("AttackTrigger");
        private readonly int _deadTriggerProperty = Animator.StringToHash("DeathTrigger");
        private readonly int _hasTargetProperty = Animator.StringToHash("HasTarget");
        private readonly int _isMovingProperty = Animator.StringToHash("IsMoving");
        private readonly int _takeDamageProperty = Animator.StringToHash("TakeDamage");


        private bool _isEnabled;
        
        public DefenderAnimation(GameObject gameObject, DefenderUnit defenderUnit)
        {
            _animator = gameObject.GetComponent<Animator>();
            defenderUnit.OnStateChanged += StateChanged;
            _isEnabled = true;
        }


        private void StateChanged(DefenderState newState)
        {
            if (_state != newState && _isEnabled)
            {
                _state = newState;
                bool isMoving = _state == DefenderState.Going || _state == DefenderState.Pursuit ||
                                _state == DefenderState.GotoBarrack;
                _animator.SetBool(_isMovingProperty, isMoving);
                
                if (_state == DefenderState.Dying)
                {
                    int randomNumber = Random.Range(1, 3);
                    _animator.SetInteger("DeathType", randomNumber);
                    _animator.SetTrigger(_deadTriggerProperty);
                }
            }
        }

        public void Disable()
        {
            _isEnabled = false;
        }

        public void TakeDamage()
        {
            if (_isEnabled)
            {
                _animator.SetTrigger(_takeDamageProperty);
            }
        }

        public void StartAttack()
        {
            if (_isEnabled)
            {
                int randomNumber = Random.Range(1, 3);
                _animator.ResetTrigger(_takeDamageProperty);
                _animator.SetTrigger(_attackTriggerProperty);
                _animator.SetInteger("AttackType", randomNumber);
            }
        }

    }
}