using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class InfoPanel : MonoBehaviour
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