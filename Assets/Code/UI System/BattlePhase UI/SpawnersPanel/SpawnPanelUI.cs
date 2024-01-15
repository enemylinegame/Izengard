using DG.Tweening;
using System;
using System.Collections.Generic;
using UnitSystem.Enum;
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

        [Space(10)]
        [SerializeField]
        private SpawnerTypeSelectionPanel _spawnerTypeSelectionPanel;

        [Space(20)]
        [SerializeField]
        private RectTransform _spawnGridPanel;
        [SerializeField]
        private GameObject _spawnerHUD;

        public event Action OnCreateSpawnerClick;
        public event Action OnRemoveSpawnerClick;
        public event Action OnMoveSpawnerClick;

        public SpawnerTypeSelectionPanel SpawnerTypeSelectionPanel => _spawnerTypeSelectionPanel;

        public event Action<string> OnSpawnerSelectAction;

        private List<SpawnerHUD> _spawnerHUDCollection = new();
      
        public void AddHUD(string spawnerId, UnitFactionType unitFaction) 
        {
            var hud = Instantiate(_spawnerHUD, _spawnGridPanel).GetComponent<SpawnerHUD>();

            string name = null;

            switch (unitFaction)
            {
                case UnitFactionType.Enemy:
                    {
                        name = $"E";
                        break;
                    }
                case UnitFactionType.Defender: 
                    {
                        name = $"D";                   
                        break;
                    }
            }

            hud.Init(spawnerId, name);

            hud.OnSelectAction += SpawnerSelected;

            _spawnerHUDCollection.Add(hud);
        }

        public void RemoveHUD(string spawnerId) 
        {
            var hud = _spawnerHUDCollection.Find(spw => spw.Id == spawnerId);
            
            if (hud == null)
                return;
            
            hud.OnSelectAction -= SpawnerSelected;

            hud.Deinit();

            Destroy(hud.gameObject);

            _spawnerHUDCollection.Remove(hud);
        }

        public void ClearHUD()
        {
            for(int i =0; i < _spawnerHUDCollection.Count; i++)
            {
                var hud = _spawnerHUDCollection[i];
                
                hud.OnSelectAction -= SpawnerSelected;

                hud.Deinit();

                Destroy(hud.gameObject);
            }

            _spawnerHUDCollection.Clear();
        }

        private void SpawnerSelected(string spawnerId)
        {
            var spawnerHUD = _spawnerHUDCollection.Find(spwn => spwn.Id == spawnerId);

            for(int i =0; i < _spawnerHUDCollection.Count; i++)
            {
                _spawnerHUDCollection[i].Unselect();
            }

            spawnerHUD.Select();

            OnSpawnerSelectAction?.Invoke(spawnerId);
        }

        public void UnselectAll()
        {
            for (int i = 0; i < _spawnerHUDCollection.Count; i++)
            {
                _spawnerHUDCollection[i].Unselect();
            }
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
