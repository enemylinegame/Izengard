using Izengard.Abstraction.Interfaces;
using Izengard.Tools;
using UnityEngine;

namespace Izengard.Units
{
    public class StubUnitModel : IUnit
    {
        public ObjectTransformModel TransformModel => throw new System.NotImplementedException();

        public UnitFactionType Faction => UnitFactionType.None;

        public UnitType Type => UnitType.None;

        public IParametr<int> Health { get; private set; }

        public IParametr<int> Armor { get; private set; }

        public IParametr<float> Size { get; private set; }
        public IParametr<float> Speed { get; private set; }

        public IParametr<float> DetectionRange { get; private set; }

        public IUnitDefence Defence { get; private set; }

        public IUnitOffence Offence { get; private set; }

        public StubUnitModel(IUnitData data, string logMessage)
        {
            Health = new ParametrModel<int>(0, 0, 0);
            Armor = new ParametrModel<int>(0, 0, 0);
            Size = new ParametrModel<float>(0, 0, 0);
            Speed = new ParametrModel<float>(0, 0, 0);
            DetectionRange = new ParametrModel<float>(0,0,0);

            Defence = new UnitDefenceModel(data.DefenceData);
            Offence = new UnitOffenceModel(data.OffenceData);

            Debug.LogWarning(logMessage);
        }

        public UnitDamage GetAttackDamage() 
        {
            return new UnitDamage
            {
                BaseDamage = 0,
                FireDamage = 0,
                ColdDamage = 0
            };
        }

        public void TakeDamage(UnitDamage damageValue) { }
    }
}
