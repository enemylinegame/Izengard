using Izengard.UnitSystem;
using Izengard.UnitSystem.Data;
using UnityEngine;

namespace Izengard
{
    public class Main : MonoBehaviour
    {
        [SerializeField] private UnitSettings _unitSettings;

        private UnitFactory _unitFactory;

        private void Start()
        {
            _unitFactory = new UnitFactory();

            var unit = _unitFactory.CreateUnit(_unitSettings);
        }
    }
}
