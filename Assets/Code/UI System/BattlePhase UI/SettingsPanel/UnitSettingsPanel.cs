using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UnitSettingsPanel : MonoBehaviour
    {
        [SerializeField]
        protected Button openButton;
        [SerializeField]
        protected Button closeButton;

        [SerializeField]
        private Button _spawnButton;

        public event Action OnSpawn;

        protected void Awake()
        {
            openButton.onClick.AddListener(OpenPanel);
            closeButton.onClick.AddListener(ClosePanel);

            openButton.gameObject.SetActive(false);
            closeButton.gameObject.SetActive(true);

            _spawnButton.onClick.AddListener(() => OnSpawn?.Invoke());
        }

        private void OnDestroy()
        {
            openButton.onClick.AddListener(OpenPanel);
            closeButton.onClick.AddListener(ClosePanel);

            _spawnButton.onClick.RemoveAllListeners();
        }

        private void OpenPanel()
        {
            openButton.gameObject.SetActive(false);
            closeButton.gameObject.SetActive(true);

            transform.DOLocalMoveX(-10, 0.2f, true);
        }

        private void ClosePanel()
        {
            openButton.gameObject.SetActive(true);
            closeButton.gameObject.SetActive(false);

            transform.DOLocalMoveX(400, 0.2f, true);
        }
    }
}
