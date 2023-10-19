using System.ComponentModel;

namespace UnitSystem.Enum
{
    public enum UnitType
    {
        None = 0,
        [Description("Ополченец")]
        Militiaman = 1,
        [Description("Охотник")]
        Hunter = 2,
        [Description("Маг")]
        Mage = 3,
        [Description("Имп")]
        Imp = 4,
        [Description("Гончая")]
        Hound = 5,
        [Description("Бес")]
        Fiend = 6,
    }
}
