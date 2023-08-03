using UnityEngine;


namespace Code.Game
{
    public class PauseManager
    {
        public void OnPause()
        {
            Time.timeScale = 0.0f;
        }

        public void OffPause()
        {
            Time.timeScale = 1.0f;
        }
    }
}