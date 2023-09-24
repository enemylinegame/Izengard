using UnityEngine;
using UnityEngine.UI;

namespace Code.UI
{
    public class BuildingPanel : MonoBehaviour
    {
        public Transform PositionPanel;
        public Button CloseButton;
        public Button OpenButton;

        private void Start()
        {
            PositionPanel = transform;
        }
    }
}