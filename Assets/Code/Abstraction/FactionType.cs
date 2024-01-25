using System.ComponentModel;

namespace Abstraction
{
    public enum FactionType
    {
        [Description("None")]
        None = 0,
        [Description("Defender")]
        Defender = 1,
        [Description("Enemy")]
        Enemy = 2,
    }
}
