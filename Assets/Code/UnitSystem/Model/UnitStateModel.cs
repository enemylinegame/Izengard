using System;
using UnitSystem.Enum;

namespace UnitSystem
{
    public class UnitStateModel
    {
        private UnitState _currentState;
        public UnitState CurrentState => _currentState;

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
