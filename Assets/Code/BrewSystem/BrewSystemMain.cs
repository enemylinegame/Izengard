using BrewSystem.Configs;
using BrewSystem.UI;
using UnityEngine;

namespace BrewSystem
{
    internal class BrewSystemMain : MonoBehaviour
    {
        [SerializeField]
        private BrewSystemUI _brewSystemUI;
        [SerializeField]
        private IngridientsDataConfig IngridientsDataConfig;

        private BrewController _controller;

        private void Start()
        {
            _controller = new BrewController(_brewSystemUI, IngridientsDataConfig);
        }
    }
}
