using BrewSystem.Configs;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BrewSystem.UI
{
    public class BrewSystemUI : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text _ingridientsCount;
        [SerializeField]
        private Transform _ingridientsHolder;
        [SerializeField]
        private GameObject _ingridientPrefab;
        [SerializeField]
        private BrewStatusUI _brewStatus;
        [SerializeField]
        private BrewResultUI _brewResult;
        [SerializeField]
        private BrewTankUI _brewTank;
        [SerializeField]
        private Button _checkBrewResultButton;

        public TMP_Text IngridientsCount => _ingridientsCount;
        public Transform IngridientsHolder => _ingridientsHolder;
        public GameObject IngridientPrefab => _ingridientPrefab;
        public BrewStatusUI BrewStatus => _brewStatus;
        public BrewResultUI BrewResult => _brewResult;
        public BrewTankUI BrewTank => _brewTank;
        public Button CheckBrewResultButton => _checkBrewResultButton;


        public void InitUI(BrewConfig config)
        {
            _brewStatus.AbvValueSlider.minValue = config.MinABVValue;
            _brewStatus.AbvValueSlider.maxValue = config.MaxABVValue;

            _brewStatus.TasteValueSlider.minValue = config.MinTasteValue;
            _brewStatus.TasteValueSlider.maxValue = config.MaxTasteValue;
            
            _brewStatus.FlavorValueSlider.minValue = config.MinFlavorValue;
            _brewStatus.FlavorValueSlider.maxValue = config.MaxFlavorValue;

            _brewResult.InitUI();
        }
    }
}
