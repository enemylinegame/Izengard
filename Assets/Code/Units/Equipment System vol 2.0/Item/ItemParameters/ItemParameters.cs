using UnityEngine;

namespace EquipmentSystem
{
    [System.Serializable]
    public class ItemParameter<T>
    {
        [SerializeField] protected T _value;
        protected string _name;
        [SerializeField] protected string _description;

        public T Value => _value;
        public string Name => _name;
        public string Description => _description;

        public void ChangeValue(T tempValue)
        {
            _value = tempValue;
        }
    }
    [System.Serializable]
    public class DamageModificator : ItemParameter<float>
    {
        public DamageModificator(float modificator)
        {
            _value = modificator;
            _name = "Модификатор урона";
            _description = "";
        }
    }
    [System.Serializable]
    public class SpeedModificator : ItemParameter<float>
    {
        public SpeedModificator(float modificator)
        {
            _value = modificator;
            _name = "Модификатор корости";
            _description = "";
        }
    }
    [System.Serializable]
    public class RangeAttackModificator : ItemParameter<float>
    {
        public RangeAttackModificator(float modificator)
        {
            _value = modificator;
            _name = "модификатор дальности атаки";
            _description = "";
        }
    }
    [System.Serializable]
    public class DefendsModificator : ItemParameter<float>
    {
        public DefendsModificator(float modificator)
        {
            _value = modificator;
            _name = "модификатор защиты";
            _description = "";
        }
    }
    [System.Serializable]
    public class ArmorSlotParameter : ItemParameter<ArmorSlotTypes>
    {
        public ArmorSlotParameter(ArmorSlotTypes value)
        {
            _value = value;
            _name = "Слот для брони";
            _description = "";
        }
    }
    [System.Serializable]
    public class WeaponSlotParameter : ItemParameter<WeaponSlotTypes>
    {
        public WeaponSlotParameter(WeaponSlotTypes value)
        {
            _value = value;
            _name = "Слот для оружия";
            _description = "";
        }
    }
    [System.Serializable]
    public class WeaponTypeParameter : ItemParameter<WeaponTypes>
    {
        public WeaponTypeParameter(WeaponTypes value)
        {
            _value = value;
            _name = "Тип оружия";
            _description = "";
        }
    }
    [System.Serializable]
    public class WeaponGripParameter : ItemParameter<WeaponGripTypes>
    {
        public WeaponGripParameter(WeaponGripTypes value)
        {
            _value = value;
            _name = "Хват оружия";
            _description = "";
        }
    }

}


