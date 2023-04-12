using UnityEngine;

namespace EquipmentSystem
{ 
    [System.Serializable]
    public class CurrentParameters 
    {
        [SerializeField] private DefendsModificator _defendsModificator = new DefendsModificator(0);
        [SerializeField] private SpeedModificator _speedModificator = new SpeedModificator(0);
        [SerializeField] private RangeAttackModificator _rangeAttackModificator = new RangeAttackModificator(0);
        [SerializeField] private DamageModificator _damageModificator = new DamageModificator(0);

        public DefendsModificator CurrentDefends => _defendsModificator;
        public SpeedModificator CurrentSpeed => _speedModificator;
        public RangeAttackModificator CurrentRangeAttackMod => _rangeAttackModificator;
        public DamageModificator CurrentDamage => _damageModificator;

        public CurrentParameters()
        {
            _defendsModificator=new DefendsModificator(0);
            _speedModificator = new SpeedModificator(0);
            _rangeAttackModificator = new RangeAttackModificator(0);
            _damageModificator = new DamageModificator(0);
        }
        public CurrentParameters(CurrentParameters param)
        {
            _defendsModificator = new DefendsModificator(param.CurrentDefends.Value);
            _speedModificator = new SpeedModificator(param.CurrentSpeed.Value);
            _rangeAttackModificator = new RangeAttackModificator(param.CurrentRangeAttackMod.Value);
            _damageModificator = new DamageModificator(param.CurrentDamage.Value);
        }
        public void ChangeParameters(CurrentParameters param)
        {
            _defendsModificator.ChangeValue(param.CurrentDefends.Value);
            _speedModificator.ChangeValue(param.CurrentSpeed.Value);
            _rangeAttackModificator.ChangeValue(param.CurrentRangeAttackMod.Value);
            _damageModificator.ChangeValue(param.CurrentDamage.Value);
        }
        public void SumParameters(CurrentParameters param)
        {
            float tempres = _defendsModificator.Value + param._defendsModificator.Value;
            _defendsModificator.ChangeValue(tempres);

            tempres = _speedModificator.Value + param._speedModificator.Value;
            _speedModificator.ChangeValue(tempres);

            tempres = _rangeAttackModificator.Value + param._rangeAttackModificator.Value;
            _rangeAttackModificator.ChangeValue(tempres);

            tempres = _damageModificator.Value + param._damageModificator.Value;
            _damageModificator.ChangeValue(tempres);
        }
        public static CurrentParameters operator + (CurrentParameters a , CurrentParameters b)
        {
            CurrentParameters result = new CurrentParameters();
            result.ChangeParameters(a);
            result.SumParameters(b);
            return result;
        }

    }
}
