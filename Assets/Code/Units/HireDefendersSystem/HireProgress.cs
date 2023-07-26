using Code.TileSystem;
using CombatSystem;

namespace Code.Units.HireDefendersSystem
{
    public class HireProgress
    {
        public DefenderPreview Defender;
        public DefenderSettings Settings;
        public TileModel Tile;
        public float Duration;
        public float TimePassed;
    }
}