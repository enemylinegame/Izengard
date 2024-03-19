using UnityEngine;
using UnityEngine.UI;

namespace BrewSystem.UI
{
    public class BrewStatusUI : MonoBehaviour
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

        public void ChangeStatus(float abv, float taste, float flavor)
        {
            AbvValueSlider.value = abv;
            TasteValueSlider.value = taste;
            FlavorValueSlider.value = flavor;
        }
    }
}
