using System;
using UnitSystem.Enum;

namespace UnitSystem.Model
{
    public class UnitStateModel
    {
        private UnitStateType _current;
        private AttackPhase _attackPhase;
        
        public UnitStateType Current => _current;
        public AttackPhase CurrentAttackPhase
        {
            get => _attackPhase;
            set => _attackPhase = value;
        } 

        public event Action<UnitStateType> OnStateChange;

        public UnitStateModel()
        {
            _current = UnitStateType.Idle;
        }

        public void ChangeState(UnitStateType newState) 
        {
            if(newState != _current)
            {
                _current = newState;
                OnStateChange?.Invoke(newState);
            }
        }
    }
}
