using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class BuildingPanel : MonoBehaviour
    {
        public Transform PositionPanel;
        public GameObject ObjectHUD;
        public RectTransform GridPanel;
        public Button CloseButton;
        public Button OpenButton;

        private void Start()
        {
            PositionPanel = transform;
        }
    }
}