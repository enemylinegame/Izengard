using Abstraction;
using Tools;
using UnitSystem.Enum;

namespace UnitSystem.Model
{
    public class UnitStatsModel 
    {
        #region Private Field
        
        private readonly IParametr<int> _health;
        private readonly IParametr<float> _size;
        private readonly IParametr<float> _speed;
        private readonly IParametr<float> _detectionRange;
        
        private UnitFactionType _faction;
        private UnitType _type;
        private UnitRoleType _role;

        #endregion

        #region Public Property
    
        public UnitFactionType Faction => _faction;
        public UnitType Type => _type;
        public UnitRoleType Role => _role;

        public IParametr<int> Health => _health;
     
        public IParametr<float> Size => _size;

        public IParametr<float> Speed => _speed;

        public IParametr<float> DetectionRange => _detectionRange;

        #endregion

        public UnitStatsModel(IUnitData data) 
        {       
            _faction = data.Faction;
            _type = data.Type;
            _role = data.Role;

            _health = new ParametrModel<int>(data.HealthPoints, 0, data.HealthPoints);
            _size = new ParametrModel<float>(data.Size, 0, float.MaxValue);
            _speed = new ParametrModel<float>(data.Speed, 0, float.MaxValue);
            _detectionRange = new ParametrModel<float>(data.DetectionRange, 0, float.MaxValue);
        }
    }
}

  
