using UnityEngine.SceneManagement;

namespace Code.Game
{
    public class GameStateManager
    {
        private const int MAIN_MENU_SCENE_ID = 0;
        private const int GAME_SCENE_ID = 1;




        public void RestartGame()
        {
            SceneManager.LoadScene(GAME_SCENE_ID);
        }
        
        public void SwitchToGame()
        {
            SceneManager.LoadScene(GAME_SCENE_ID);
        }

        public void SwitchToMainMenu()
        {
            SceneManager.LoadScene(MAIN_MENU_SCENE_ID);
        }
    }
}