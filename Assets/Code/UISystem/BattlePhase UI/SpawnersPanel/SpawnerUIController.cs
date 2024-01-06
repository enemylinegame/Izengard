namespace UI
{
    public class SpawnerUIController
    {
        private readonly SpawnPanelUI _spawnPanel;

        private int _spawnerCount;

        public SpawnerUIController(SpawnPanelUI spawnPanel)
        {
            _spawnPanel = spawnPanel;
            _spawnPanel.OnCreateSpawnerClick += CreateSpawner;

            _spawnerCount = 0;
        }

        private void CreateSpawner()
        {
            _spawnerCount++;

            _spawnPanel.AddHUD(_spawnerCount);
        }
    }
}
