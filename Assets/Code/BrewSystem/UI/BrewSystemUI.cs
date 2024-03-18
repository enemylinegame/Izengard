using BrewSystem.Configs;
using UnityEngine;
using UnityEngine.UI;

namespace BrewSystem.UI
{
    public class BrewSystemUI : MonoBehaviour
    {
        [SerializeField]
        private Transform _ingridientsHolder;
        [SerializeField]
        private GameObject _ingridientPrefab;
        [SerializeField]
        private BrewStatusUI _brewStatus;
        [SerializeField]
        private Button _checkBrewResultButton;

        public Transform IngridientsHolder => _ingridientsHolder;
        public GameObject IngridientPrefab => _ingridientPrefab;
        public BrewStatusUI BrewStatus => _brewStatus;
        public Button CheckBrewResultButton => _checkBrewResultButton;

        public void InitUI(BrewConfig config)
        {
            _brewStatus.AbvValueSlider.minValue = config.MinABVValue;
            _brewStatus.AbvValueSlider.maxValue = config.MaxABVValue;

            _brewStatus.TasteValueSlider.minValue = config.MinTasteValue;
            _brewStatus.TasteValueSlider.maxValue = config.MaxTasteValue;
            
            _brewStatus.FlavorValueSlider.minValue = config.MinFlavorValue;
            _brewStatus.FlavorValueSlider.maxValue = config.MaxFlavorValue;
        }

        public void ChangeBrewStatus(float abv, float taste, float flavor)
        {
            _brewStatus.AbvValueSlider.value = abv;
            _brewStatus.TasteValueSlider.value = taste;
            _brewStatus.FlavorValueSlider.value = flavor;
        }
    }
}
