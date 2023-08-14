using Code.Game;
using Audio_System;
using UnityEngine;
using UnityEngine.Audio;

namespace StartupMenu
{
    public class StartupMenuEntryPoint : MonoBehaviour
    {
        [SerializeField] private Transform _placeForUI;
        [SerializeField] private ScriptableObject _baseSettings;
        [Space(10)]
        [SerializeField] private AudioMixer _audioMixer;
        [SerializeField] private AudioPresenter _audioPresenter;

        private StartupMenuController _startupMenuController;
        
        private void Start()
        {
            _startupMenuController 
                = new StartupMenuController(
                    new GameStateManager(),
                    _placeForUI, 
                    (ISettingsData)_baseSettings, 
                    _audioMixer,
                    _audioPresenter);
        }


        private void OnDestroy()
        {
            _startupMenuController?.Dispose();
        }
    }
}
