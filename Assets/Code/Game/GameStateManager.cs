using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Code.Game
{
    public class GameStateManager
    {

        public Action OnDispose;
        
        private const int MAIN_MENU_SCENE_ID = 0;
        private const int GAME_SCENE_ID = 1;

        private bool _isPause;
        

        public void RestartGame()
        {
            if (_isPause)
            {
                OffPause();
            }
            OnDispose?.Invoke();
            SceneManager.LoadScene(GAME_SCENE_ID);
        }

        public void SwitchToGame()
        {
            if (_isPause)
            {
                OffPause();
            }
            OnDispose?.Invoke();
            SceneManager.LoadScene(GAME_SCENE_ID);
        }

        public void SwitchToMainMenu()
        {
            if (_isPause)
            {
                OffPause();
            }
            OnDispose?.Invoke();
            SceneManager.LoadScene(MAIN_MENU_SCENE_ID);
        }

        public void OnPause()
        {
            Time.timeScale = 0.0f;
            _isPause = true;
        }

        public void OffPause()
        {
            Time.timeScale = 1.0f;
            _isPause = false;
        }
    }
}