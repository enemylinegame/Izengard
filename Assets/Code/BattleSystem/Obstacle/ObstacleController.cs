using System;
using System.Collections.Generic;
using Tools.Navigation;

namespace BattleSystem.Obstacle
{
    public class ObstacleController: IOnStart
    {
        private readonly List<IObstacle> _obstaclesCollection = new();

        private readonly int _obstPlacedSurfaceId;
        private readonly NavigationUpdater _navigationUpdater;

        public List<IObstacle> ObstaclesCollection => _obstaclesCollection;

        public event Action<IObstacle> OnObstalceRemoved;

        public ObstacleController(
            int obstPlacedSurfaceId, 
            NavigationUpdater navigationUpdater, 
            IReadOnlyList<IObstacleView> defendWalls)
        {
            _obstPlacedSurfaceId = obstPlacedSurfaceId;
            
            _navigationUpdater = navigationUpdater;

            foreach (var defendWall in defendWalls) 
            {
                var obstacle = new ObstacleHandler(defendWall);

                _obstaclesCollection.Add(obstacle);
            }
        }

        public void OnStart()
        {
            for(int i =0; i < _obstaclesCollection.Count; i++)
            {
                var obstacle = _obstaclesCollection[i];
                
                obstacle.OnReachedZeroHealth += ObstacleReachedZeroHealth;

                obstacle.Enable();
            }
        }

        private void ObstacleReachedZeroHealth(string obstacleId)
        {
            var obstacle = _obstaclesCollection.Find(obst => obst.Id == obstacleId);

            obstacle.OnReachedZeroHealth -= ObstacleReachedZeroHealth;

            obstacle.Disable();

            _obstaclesCollection.Remove(obstacle);

            OnObstalceRemoved?.Invoke(obstacle);

            _navigationUpdater.UpdateSurface(_obstPlacedSurfaceId);
        }
    }
}
