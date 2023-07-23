using UnityEngine;

namespace CombatSystem
{
    public class DefenderAnimation
    {
        private Animator _animator;

        private DefenderState _state;
        private int _isAttackAnimatorProperty = Animator.StringToHash("IsAttack");

        private bool _isEnabled;
        
        public DefenderAnimation(GameObject gameObject, DefenderUnit defenderUnit)
        {
            _animator = gameObject.GetComponent<Animator>();
            defenderUnit.OnStateChanged += StateChanged;
            _isEnabled = true;
        }

        private void StateChanged(DefenderState newState)
        {
            if (_state != newState)
            {
                _state = newState;
                if (_isEnabled)
                {
                    _animator.SetBool(_isAttackAnimatorProperty, _state == DefenderState.Fight);
                }
            }
        }

        public void Disable()
        {
            _isEnabled = false;
        }

    }
}