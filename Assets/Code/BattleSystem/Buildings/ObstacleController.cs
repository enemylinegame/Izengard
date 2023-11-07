using BattleSystem.Buildings.Interfaces;
using System;
using System.Collections.Generic;
using Tools.Navigation;

namespace BattleSystem.Buildings
{
    public class ObstacleController: IOnStart
    {
        private readonly List<IObstacle> _obstaclesCollection = new();

        private readonly int _obstPlacedSurfaceId;
        private readonly NavigationUpdater _navigationUpdater;

        private int _obstacleIndex = 12354;

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
                var obstacle = new ObstacleHandler(_obstacleIndex, defendWall);

                _obstaclesCollection.Add(obstacle);

                _obstacleIndex++;
            }
        }

        public void OnStart()
        {
            for(int i =0; i < _obstaclesCollection.Count; i++)
            {
                var obstacle = _obstaclesCollection[i];
                
                obstacle.OnReacheZeroHealth += ObstacleReachedZeroHealth;

                obstacle.Enable();
            }
        }


        private void ObstacleReachedZeroHealth(int obstacleId)
        {
            var obstacle = _obstaclesCollection.Find(obst => obst.Id == obstacleId);

            obstacle.OnReacheZeroHealth -= ObstacleReachedZeroHealth;

            obstacle.Disable();

            _obstaclesCollection.Remove(obstacle);

            OnObstalceRemoved?.Invoke(obstacle);

            _navigationUpdater.UpdateSurface(_obstPlacedSurfaceId);
        }
    }
}
