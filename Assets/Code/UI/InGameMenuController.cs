using Code.Game;

namespace Code.UI
{
    public class InGameMenuController
    {
        private InGameMenuUI _inGameMenuUI;
        private PauseManager _pauseManager;

        public InGameMenuController(InGameMenuUI inGameMenuUI, PauseManager pauseManager)
        {
            _inGameMenuUI = inGameMenuUI;
            _pauseManager = pauseManager;
        }
    }
}