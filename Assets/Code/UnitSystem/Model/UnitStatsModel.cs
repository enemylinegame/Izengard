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
        private UnitRoleType _role;

        #endregion

        #region Public Property
    
        public UnitFactionType Faction => _faction;
        public UnitRoleType Role => _role;

        public IParametr<int> Health => _health;
     
        public IParametr<float> Size => _size;

        public IParametr<float> Speed => _speed;

        public IParametr<float> DetectionRange => _detectionRange;

        #endregion

        public UnitStatsModel(IUnitStatsData stats) 
        {       
            _faction = stats.Faction; 
            _role = stats.Role;

            _health = new ParametrModel<int>(stats.HealthPoints, 0, stats.HealthPoints);
            _size = new ParametrModel<float>(stats.Size, 0, float.MaxValue);
            _speed = new ParametrModel<float>(stats.Speed, 0, float.MaxValue);
            _detectionRange = new ParametrModel<float>(stats.DetectionRange, 0, float.MaxValue);
        }
    }
}

  
