using LevelGenerator.Interfaces;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace LevelGenerator
{
    public class ButtonsSetter : IButtonsSetter
    {
        private readonly float _offsetInstanceTiles;
        private readonly Button _buttonSpawn;
        private readonly Transform _btnParents;
        private readonly Action<TileSpawnInfo> _onSpawnTileButtonClick;
        private readonly IReadOnlyDictionary<Vector2Int, VoxelTile> _spawnedTiles;
        private readonly HashSet<IOnLateUpdate> _buttons = new HashSet<IOnLateUpdate>();


        public ButtonsSetter(Action<TileSpawnInfo> onSpawnTileButtonClick, Transform buttonParent, float offsetInstanceTiles,
            IReadOnlyDictionary<Vector2Int, VoxelTile> spawnedTiles, Button buttonSpawn)
        {
            _onSpawnTileButtonClick = onSpawnTileButtonClick;
            _btnParents = buttonParent;
            _offsetInstanceTiles = offsetInstanceTiles;
            _spawnedTiles = spawnedTiles;
            _buttonSpawn = buttonSpawn;
        }

        public void SetButtons(Vector2Int tileGridPosition)
        {
            CreateButton(tileGridPosition);
        }

        private void CreateButton(Vector2Int tileGridPosition)
        {
            var tilePosition = _spawnedTiles[tileGridPosition].transform.position;
            for (byte i = 0; i < _spawnedTiles[tileGridPosition].TablePassAccess.Length; ++i)
            {
                var way = i.GetVector2IntWay();
                var newTileGridPosition = tileGridPosition + way;
                if (_spawnedTiles[tileGridPosition].TablePassAccess[i] == 1 && !_spawnedTiles.ContainsKey(newTileGridPosition))
                {
                    var wayVector3 = new Vector3(way.x, 0, way.y);
                    Vector3 posToSpawnBtn = tilePosition + wayVector3 * _offsetInstanceTiles;
                    InstansButton(tileGridPosition, newTileGridPosition, posToSpawnBtn);
                }
            }
        }

        private void InstansButton(Vector2Int baseTileGridPosition, Vector2Int newTileGridPosition, Vector3 posForButton)
        {
            Vector2 pos = Camera.main.WorldToScreenPoint(posForButton);
            Button btnMain = UnityEngine.Object.Instantiate(_buttonSpawn, pos, Quaternion.identity, _btnParents);
            var tileSpawnInfo = new TileSpawnInfo
            {
                GridBasePosition = baseTileGridPosition,
                GridSpawnPosition = newTileGridPosition
            };
            _buttons.Add(new ParentButtonController(btnMain, tileSpawnInfo, _onSpawnTileButtonClick, posForButton, RemoveButton));
        }

        private void RemoveButton(ParentButtonController button)
        {
            _buttons.Remove(button);
        }

        public void OnLateUpdate(float deltaTime)
        {
            foreach (var button in _buttons) button.OnLateUpdate(deltaTime);
        }

        public void Dispose()
        {
            foreach (var button in _buttons)
            {
                if (button is IDisposable disposable) disposable.Dispose();
            }
        }
    }
}