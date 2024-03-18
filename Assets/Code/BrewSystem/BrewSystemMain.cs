using BrewSystem.Configs;
using BrewSystem.UI;
using UI;
using UnityEngine;

namespace BrewSystem
{
    public class BrewSystemMain : MonoBehaviour
    {
        [SerializeField]
        private UIElementsConfig _uiConfig;
        [SerializeField]
        private Canvas _canvas;
        [SerializeField]
        private BrewConfig _brewConfig;

        private BrewController _controller;

        private void Start()
        {
            var uiFactory = new BrewSystemUIFactory(_uiConfig, _canvas);

            _controller = new BrewController(uiFactory, _brewConfig);
        }
    }
}
