using Izengard.Abstraction.Interfaces;
using Izengard.Tools;

namespace Izengard.Units 
{
    public class UnitModel : IUnit
    {
        #region Consts

        private const int ARMOR_REDUCE_COEF = 2;
        private const int ARMOR_LOSE_POINT  = 1;

        #endregion

        #region Private Field

        private readonly ObjectTransformModel _transformModel;
        
        private readonly IParametr<int> _health;
        private readonly IParametr<int> _armor;
        private readonly IParametr<float> _size;
        private readonly IParametr<float> _speed;
        private readonly IParametr<float> _detectionRange;
        
        private UnitFactionType _faction;
        private UnitType _type;

        private IUnitDefence _defence;
        private IUnitOffence _offence;

        #endregion

        #region Public Property

        public ObjectTransformModel TransformModel => _transformModel;
        
        public UnitFactionType Faction => _faction;
        public UnitType Type => _type;

        public IParametr<int> Health => _health;

        public IParametr<int> Armor => _armor;
        
        public IParametr<float> Size => _size;

        public IParametr<float> Speed => _speed;

        public IParametr<float> DetectionRange => _detectionRange;

        public IUnitDefence Defence => _defence;
        public IUnitOffence Offence => _offence;


        #endregion

        public UnitModel(
            ObjectTransformModel transformModel, 
            UnitFactionType factionType,
            IUnitStatsData stats,
            IUnitDefence defence,
            IUnitOffence offence) 
        {
            _transformModel = transformModel;
            
            _faction = factionType;
            
            _type = stats.Type;
            _health = new ParametrModel<int>(stats.HealthPoints, 0, stats.HealthPoints);
            _armor = new ParametrModel<int>(stats.ArmorPoints, 0, int.MaxValue);
            _size = new ParametrModel<float>(stats.Size, 0, float.MaxValue);
            _speed = new ParametrModel<float>(stats.Speed, 0, float.MaxValue);
            _detectionRange = new ParametrModel<float>(stats.DetectionRange, 0, float.MaxValue);

            _defence = defence;
            _offence = offence;
        }

        public void TakeDamage(UnitDamage damageValue)
        {
            var resultDamageAmount
                = _defence.GetAfterDefDamage(damageValue);

            var armorPoints = _armor.GetValue();
            if (armorPoints != 0)
            {
                resultDamageAmount /= ARMOR_REDUCE_COEF;
                var armoLost = armorPoints - ARMOR_LOSE_POINT;
                _armor.SetValue(armoLost);
            }

            var hpLost = _health.GetValue() - resultDamageAmount;
            _health.SetValue(hpLost);
        }

        public UnitDamage GetAttackDamage()
        {
            return _offence.GetDamage();
        }
    }
}

  
