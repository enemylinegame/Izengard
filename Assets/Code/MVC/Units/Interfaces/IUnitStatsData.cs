﻿namespace Izengard.Units
{
    public interface IUnitStatsData
    {
        UnitType Type { get; }

        int HealthPoints { get; }
        int ArmorPoints { get; }

        float Size { get; }
        float Speed { get; }
        float DetectionRange { get; }
    }
}
