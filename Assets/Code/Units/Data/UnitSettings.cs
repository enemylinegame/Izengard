using UnityEngine;

namespace Izengard.Units.Data
{
    [CreateAssetMenu(fileName = nameof(UnitSettings), menuName = "UnitsData/" + nameof(UnitSettings))]
    public class UnitSettings : ScriptableObject, IUnitData
    {
        [SerializeField] private UnitFactionType _faction;
        [SerializeField] private UnitType _type;
        [SerializeField] private int _healthPoints = 100;
        [SerializeField] private int _armorPoints = 20;
        [SerializeField] private float _size = 1f;
        [SerializeField] private float _speed = 1f;
        [SerializeField] private float _detectionRange = 5f;
        [SerializeField] private UnitDefenceSettings _defenceData;
        [SerializeField] private UnitOffenceSettings _offenceData;
        
        #region IUnitData

        public UnitFactionType Faction => _faction;
        public  UnitType Type => _type;

        public int HealthPoints => _healthPoints;
        public int ArmorPoints => _armorPoints;
        public float Size => _size;
        public float Speed => _speed;
        public float DetectionRange => _detectionRange;

        public IUnitDefenceData DefenceData => _defenceData;

        public IUnitOffenceData OffenceData => _offenceData;

        #endregion
    }
}
