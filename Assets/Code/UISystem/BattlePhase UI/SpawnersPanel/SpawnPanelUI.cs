using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class SpawnPanelUI: MonoBehaviour
    {
        [SerializeField]
        private Button openButton;
        [SerializeField]
        private Button closeButton;
        
        [SerializeField]
        private Button _createSpawnerButton;
        [SerializeField]
        private Button _removeSpawnerButton;
        [SerializeField]
        private Button _moveSpawnerButton;
        [Space(20)]
        [SerializeField]
        private RectTransform _spawnGridPanel;
        [SerializeField]
        private GameObject _spawnerHUD;

        public event Action OnCreateSpawnerClick;
        public event Action OnRemoveSpawnerClick;
        public event Action OnMoveSpawnerClick;

        public void AddHUD(int index) 
        {
            var hud = Instantiate(_spawnerHUD, _spawnGridPanel).GetComponent<SpawnerHUD>();
            hud.Init(index);
        }


        private void Awake()
        {
            openButton.onClick.AddListener(OpenPanel);
            closeButton.onClick.AddListener(ClosePanel);

            _createSpawnerButton.onClick.AddListener(() => OnCreateSpawnerClick?.Invoke());
            _removeSpawnerButton.onClick.AddListener(() => OnRemoveSpawnerClick?.Invoke());
            _moveSpawnerButton.onClick.AddListener(() => OnMoveSpawnerClick?.Invoke());
        }

        private void OpenPanel()
        {
            openButton.gameObject.SetActive(false);
            closeButton.gameObject.SetActive(true);

            transform.DOLocalMoveY(-375, 0.2f, true);
        }

        private void ClosePanel()
        {
            openButton.gameObject.SetActive(true);
            closeButton.gameObject.SetActive(false);

            transform.DOLocalMoveY(-615, 0.2f, true);
        }

        private void OnDestroy()
        {
            openButton.onClick.RemoveListener(OpenPanel);
            closeButton.onClick.RemoveListener(ClosePanel);

            _createSpawnerButton.onClick.RemoveAllListeners();
            _removeSpawnerButton.onClick.RemoveAllListeners();
            _moveSpawnerButton.onClick.RemoveAllListeners();
        }

    }
}
