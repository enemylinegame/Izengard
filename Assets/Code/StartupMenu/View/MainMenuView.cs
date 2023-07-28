using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace StartupMenu
{
    public class MainMenuView : MonoBehaviour
    {

        [SerializeField] private Button _playButton;
        [SerializeField] private Button _settingsButton;
        [SerializeField] private Button _quitButton;

        internal void Init(
            UnityAction startGame, 
            UnityAction openSettings, 
            UnityAction exit)
        {
            _playButton.onClick.AddListener(startGame);
            _settingsButton.onClick.AddListener(openSettings);
            _quitButton.onClick.AddListener(exit);
        }

        protected void OnDestroy()
        {
            _playButton.onClick.RemoveAllListeners();
            _settingsButton.onClick.RemoveAllListeners();
            _quitButton.onClick.RemoveAllListeners();
        }

    }
}

