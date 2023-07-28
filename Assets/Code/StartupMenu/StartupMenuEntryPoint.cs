using UnityEngine;
using UnityEngine.Audio;

namespace StartupMenu
{
    public class StartupMenuEntryPoint : MonoBehaviour
    {
        [SerializeField] private Transform _placeForUI;
        [SerializeField] private AudioMixer _audioMixer;
        [SerializeField] private AudioSource _clickAudioSource;

        private StartupMenuController _startupMenuController;

        private void Start()
        {
            _startupMenuController = new StartupMenuController(_placeForUI, _audioMixer, _clickAudioSource);
        }
    }
}
