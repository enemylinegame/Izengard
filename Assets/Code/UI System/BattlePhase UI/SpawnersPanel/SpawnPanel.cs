using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class SpawnPanel: MonoBehaviour
    {
        [SerializeField]
        private Button openButton;
        [SerializeField]
        private Button closeButton;
        
        [SerializeField]
        private Button createSpawnerButton;
        [SerializeField]
        private Button removeSpawnerButton;
        [SerializeField]
        private Button moveSpawnerButton;


        private void Awake()
        {
            openButton.onClick.AddListener(OpenPanel);
            closeButton.onClick.AddListener(ClosePanel);
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
        }

    }
}
