using System.Collections.Generic;
using UnitSystem.Enum;
using UnityEngine;

namespace UnitSystem.Data
{
    [CreateAssetMenu(
        fileName = nameof(UnitSettings), 
        menuName = "UnitsData/" + nameof(UnitSettings))]
    public class UnitSettings : ScriptableObject, IUnitData
    {
        [SerializeField] private UnitMainStatsData _mainStats;
        [SerializeField] private List<UnitPriorityData> _unitPriorities;
        [SerializeField] private UnitDefenceData _defenceData;
        [SerializeField] private UnitOffenceData _offenceData;
        
        #region IUnitData

        public UnitFactionType Faction => _mainStats.Faction;
        public UnitType Type => _mainStats.Type;
        public UnitRoleType Role => _mainStats.Role;
        public int HealthPoints => _mainStats.HealthPoints;
        public float Size => _mainStats.Size;
        public float Speed => _mainStats.Speed;
        public float DetectionRange => _mainStats.DetectionRange;

        public IList<UnitPriorityData> UnitPriorities => _unitPriorities;

        public float EvadeChance => _defenceData.EvadeChance;
        public float ArmorPoints => _defenceData.ArmorPoints;
        public float BaseShieldPoints => _defenceData.ShieldData.BaseShieldPoints;
        public float FireShieldPoints => _defenceData.ShieldData.FireShieldPoints;
        public float ColdShieldPoints => _defenceData.ShieldData.ColdShieldPoints;
        public float BaseDamageResist => _defenceData.ResistData.BaseDamageResist;
        public float FireDamageResist => _defenceData.ResistData.FireDamageResist;
        public float ColdDamageResist => _defenceData.ResistData.ColdDamageResist;

        public UnitAttackType AttackType => _offenceData.AttackType;
        public UnitAbilityType AbilityType => _offenceData.AbilityType;
        public float MinRange => _offenceData.MinRange;
        public float MaxRange => _offenceData.MaxRange;
        public float CastingTime => _offenceData.CastingTime;
        public float AttackTime => _offenceData.AttackTime;
        public float CriticalChance => _offenceData.CriticalChance;
        public float CritScale => _offenceData.CritScale;
        public float FailChance => _offenceData.FailChance;
        public float OnFailDamage => _offenceData.OnFailDamage;
        public float BaseDamage => _offenceData.DamageData.BaseDamage;
        public float FireDamage => _offenceData.DamageData.FireDamage;
        public float ColdDamage => _offenceData.DamageData.ColdDamage;


        #endregion
    }
}
