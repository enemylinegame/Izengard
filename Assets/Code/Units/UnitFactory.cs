using System;

namespace Izengard.Units
{
    public class UnitFactory
    {
        public UnitFactory() { }

        public IUnit CreateUnit(UnitFactionType factionType)
        {
            var unit = factionType switch
            {
                UnitFactionType.Enemy => CreateEnemy(),
                UnitFactionType.Defender => CreateDefender(),
                _=>null
            };

            return unit;
        }

        private IUnit CreateEnemy()
        {
            throw new NotImplementedException();
        }

        private IUnit CreateDefender()
        {
            throw new NotImplementedException();
        }
    }
}
