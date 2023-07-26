using UnityEngine;

namespace StartupMenu
{
    public class StartupMenuEntryPoint : MonoBehaviour
    {
        [SerializeField] private GameObject _mainMenu;
        [SerializeField] private GameObject _settingsMenu;
        [SerializeField] private Transform _placeForUI;

        private StartupMenuController _startupMenuController;

        private void Start()
        {
            _startupMenuController = new StartupMenuController(_placeForUI,_mainMenu, _settingsMenu);
        }
    }
}
