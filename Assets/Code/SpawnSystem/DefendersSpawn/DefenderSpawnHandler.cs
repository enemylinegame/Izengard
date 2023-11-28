using Tools;
using UnitSystem.Enum;

namespace SpawnSystem
{
    public class DefenderSpawnHandler
    {
        private readonly ISpawnController _spawnController;
        private TimeRemaining _timer;

        private float _spawnInterval = 1.0f;
        private int _unitsToSpawn = 4;
        private int _unitsCounter = 0;

        private bool _isTiming;

        public DefenderSpawnHandler(ISpawnController defendersSpawnController) 
        {
            _spawnController = defendersSpawnController;
            _timer = new TimeRemaining(SpawnPack, _spawnInterval);
        }

        public void StartSpawn()
        {
            if (!_isTiming)
            {
                TimersHolder.AddTimer(_timer);
                _isTiming = true;
            }
        }

        public void StopSpawn()
        {
            if (_isTiming)
            {
                TimersHolder.RemoveTimer(_timer);
                _isTiming = false;
            }
        }

        private void SpawnPack()
        {
            _isTiming = false;
            _spawnController.SpawnUnit(UnitType.Militiaman);
            _unitsCounter++;
            if (_unitsCounter < _unitsToSpawn)
            {
                TimersHolder.AddTimer(_timer);
                _isTiming = true;
            }
        }

    }
}
