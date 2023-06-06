using System;
using System.Collections.Generic;
using UnityEngine;
using Button = UnityEngine.UI.Button;

namespace LevelGenerator
{
    public class ParentButtonController : IDisposable, IOnLateUpdate
    {
        private readonly List<IDisposable> _spawnButtonControllers = new List<IDisposable>(2);
        private readonly Button _button;
        private TileSpawnInfo _tileSpawnInfo;
        private readonly Action<TileSpawnInfo> _onSpawnTileButtonClick;
        private readonly Vector3 _positionInWorld;
        private readonly Action<ParentButtonController> _removeButton;


        public ParentButtonController(Button button, TileSpawnInfo tileSpawnInfo, 
            Action<TileSpawnInfo> onSpawnTileButtonClick, Vector3 positionInWorld, Action<ParentButtonController> removeButton)
        {
            _button = button;
            _tileSpawnInfo = tileSpawnInfo;
            _onSpawnTileButtonClick = onSpawnTileButtonClick;
            _positionInWorld = positionInWorld;
            _removeButton = removeButton;

            _button.onClick.AddListener(OnButtonClick);
        }

        //private void InstansButton()
        //{
        //    _button.interactable = false;

        //    int counter = 0;

        //    foreach (Transform child in _button.transform)
        //    {
        //        child.gameObject.SetActive(true);
        //        if (counter == 0)
        //        {
        //            SetButtons(true, child.GetComponent<Button>(), () => counter++);
        //        }
        //        else
        //        {
        //            SetButtons(false, child.GetComponent<Button>(), () => counter = 0);
        //        }
        //    }

        //    void SetButtons(bool isDefendTile, Button button, Action counterAction)
        //    {
        //        _tileSpawnInfo.IsDefendTile = isDefendTile;
        //        _spawnButtonControllers.Add(new SpawnTileButtonController(_onSpawnTileButtonClick, button, _tileSpawnInfo, Dispose));
        //        counterAction();
        //    }
        //}

        public void Dispose()
        {
            foreach (var spawnButtonController in _spawnButtonControllers) spawnButtonController.Dispose();
            UnityEngine.Object.Destroy(_button.gameObject);
            _removeButton(this);
        }

        public void OnLateUpdate(float deltaTime)
        {
            _button.transform.position = Camera.main.WorldToScreenPoint(_positionInWorld);
        }

        private void OnButtonClick()
        {
            _onSpawnTileButtonClick(_tileSpawnInfo);
            Dispose();
        }
    }
}