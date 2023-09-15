using Izengard.Damage;
using Izengard.Tools;
using Izengard.Units.Data;

namespace Izengard.Units 
{
    public class UnitModel : IUnit
    {
        private const int ARMOR_REDUCE_ÑOEF = 2;
        private const int ARMOR_LOSE_POINT  = 1;

        private readonly ObjectTransformModel _transformModel;
        
        private readonly ParametrModel<int> _health;
        private readonly ParametrModel<int> _armor;
        private readonly ParametrModel<float> _size;
        private readonly ParametrModel<float> _speed;
        private readonly ParametrModel<float> _detectionRange;
        
        private UnitFactionType _faction;
        private UnitType _type;

        private UnitDefenceModel _defenceModel;
        private UnitOffenceModel _offenceModel;

        public ObjectTransformModel TransformModel => _transformModel;
        
        public UnitFactionType Faction => _faction;
        public UnitType Type => _type;

        public ParametrModel<int> Health => _health;

        public ParametrModel<int> Armor => _armor;
        
        public ParametrModel<float> Size => _size;

        public ParametrModel<float> Speed => _speed;

        public ParametrModel<float> DetectionRange => _detectionRange;

        public UnitModel(ObjectTransformModel transformModel, IUnitData data) 
        {
            _transformModel = transformModel;
            
            _faction = data.Faction;

            _health = new ParametrModel<int>(data.HealthPoints, 0, data.HealthPoints);
            _armor = new ParametrModel<int>(data.ArmorPoints, 0, int.MaxValue);
            _size = new ParametrModel<float>(data.Size, 0, float.MaxValue);
            _speed = new ParametrModel<float>(data.Speed, 0, float.MaxValue);
            _detectionRange = new ParametrModel<float>(data.DetectionRange, 0, float.MaxValue);

            _defenceModel = new UnitDefenceModel(data.DefenceData);
            _offenceModel = new UnitOffenceModel(data.OffenceData);
        }

        public void TakeDamage(IUnitDamage damageValue)
        {
            var resultDamageAmount
                = _defenceModel.GetAfterDefDamage(damageValue);

            var armorPoints = _armor.GetValue();
            if (armorPoints != 0)
            {
                resultDamageAmount /= ARMOR_REDUCE_ÑOEF;
                var armoLost = armorPoints - ARMOR_LOSE_POINT;
                _armor.SetValue(armoLost);
            }

            var hpLost = _health.GetValue() - resultDamageAmount;
            _health.SetValue(hpLost);
        }

        public IUnitDamage GetAttackDamage()
        {
            return _offenceModel.GetDamage();
        }
    }
}

  
