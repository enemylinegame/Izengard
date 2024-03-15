using UnityEngine;
using UnityEngine.UI;

namespace BrewSystem.UI
{
    internal class BrewStatusUI : MonoBehaviour
    {
        [SerializeField]
        private Slider _abvValueSlider;
        [SerializeField]
        private Slider _tasteValueSlider;
        [SerializeField]
        private Slider _flavorValueSlider;

        public Slider AbvValueSlider => _abvValueSlider;
        public Slider TasteValueSlider => _tasteValueSlider;
        public Slider FlavorValueSlider => _flavorValueSlider;
    }
}
