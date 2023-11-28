using System;
using UnitSystem.Enum;

namespace UnitSystem.Model
{
    public class UnitStateModel
    {
        private UnitState _currentState;
        private AttackPhase _attackPhase;
        
        public UnitState CurrentState => _currentState;
        public AttackPhase CurrentAttackPhase
        {
            get => _attackPhase;
            set => _attackPhase = value;
        } 

        public event Action<UnitState> OnStateChange;

        public UnitStateModel()
        {
            _currentState = UnitState.Idle;
        }

        public void ChangeState(UnitState newState) 
        {
            if(newState != _currentState)
            {
                _currentState = newState;
                OnStateChange?.Invoke(newState);
            }
        }
    }
}
