using UnityEngine;

namespace StartupMenu
{
    public class StartupMenuEntryPoint : MonoBehaviour
    {
        [SerializeField] private Transform _placeForUI;

        private StartupMenuController _startupMenuController;

        private void Start()
        {
            _startupMenuController = new StartupMenuController(_placeForUI);
        }
    }
}
