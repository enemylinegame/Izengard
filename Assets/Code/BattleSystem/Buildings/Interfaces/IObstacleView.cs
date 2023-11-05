using BattleSystem.Buildings.Configs;

namespace BattleSystem.Buildings.Interfaces
{
    public interface IObstacleView : IWarBuildingView
    {
        public DefenWallConfig Config { get; }
    }
}
