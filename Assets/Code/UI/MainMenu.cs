using UnityEngine;
using UnityEngine.SceneManagement;

namespace Code.UI
{
    public class MainMenu : MonoBehaviour
    {
        private int _gameSceneIndex;

        private void Awake()
        {
            _gameSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        }

        public void Playgame()
        {
            SceneManager.LoadScene(_gameSceneIndex);
        }

        public void QuitGame()
        {
#if UNITY_EDITOR
            Debug.Log("EXIT!");
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}

