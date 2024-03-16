using UnityEngine;

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

        public Transform IngridientsHolder => _ingridientsHolder;
        public GameObject IngridientPrefab => _ingridientPrefab;
        public BrewStatusUI BrewStatus => _brewStatus;
    }
}
