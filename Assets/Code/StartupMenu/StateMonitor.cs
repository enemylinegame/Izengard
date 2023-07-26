using System;

namespace StartupMenu
{
    public enum MenuState
    {
        None,
        Start,
        Settings,
        Game,
        Exit
    }

    public class StateMonitor
    {
        public event Action<MenuState> OnStateChange;

        private MenuState _currentState;

        public MenuState CurrentState
        {
            get => _currentState;
            set
            {
                _currentState = value;
                OnStateChange?.Invoke(_currentState);
            }
        }
    }
}
