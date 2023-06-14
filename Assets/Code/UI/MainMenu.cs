using UnityEngine;
using UnityEngine.SceneManagement;

namespace Code.UI
{
    public class MainMenu : MonoBehaviour
    {
        public void Playgame()
        
            //

        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        public void QuitGame()
        {
            Debug.Log("EXIT!");
            Application.Quit();
        }
    }
}

