using System;
using UnityEngine;
using UnityEngine.UI;

namespace Code.UI
{
    public class  InGameMenuPanel : MonoBehaviour
    {
        [field: SerializeField] public Button ContinueButton { get; private set; }
        [field: SerializeField] public Button RestartButton { get; private set; }
        [field: SerializeField] public Button QuitButton { get; private set; }
        [field: SerializeField] public Button SettingsButton { get; private set; }
        [field: SerializeField] public GameObject RootGameObject { get; private set; }
    }
}