using System;
using UnityEngine.UI;


namespace LevelGenerator
{
    public class SpawnTileButtonController : IDisposable
    {
        private readonly Action<TileSpawnInfo> _onButtonClick;
        private readonly Button _button;
        private readonly TileSpawnInfo _spawnInfo;
        private readonly Action _removeButton;


        public SpawnTileButtonController(Action<TileSpawnInfo> onButtonClick, Button button, 
            TileSpawnInfo tileSpawnInfo, Action removeButton)
        {
            _onButtonClick = onButtonClick;
            _button = button;
            _button.onClick.AddListener(OnButtonClick);
            _spawnInfo = tileSpawnInfo;
            _removeButton = removeButton;
        }

        private void OnButtonClick()
        {
            _onButtonClick(_spawnInfo);
            _removeButton();
        }

        public void Dispose()
        {
            _button.onClick.RemoveListener(OnButtonClick);
        }
    }
}