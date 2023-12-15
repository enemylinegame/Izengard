using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public abstract class UnitSettingsPanel : MonoBehaviour
    {
        [SerializeField]
        protected Transform panelRootTransform;

        [SerializeField]
        protected Button openButton;
        [SerializeField]
        protected Button closeButton;

        protected void Awake()
        {
            openButton.onClick.AddListener(OpenPanel);
            closeButton.onClick.AddListener(ClosePanel);

            openButton.gameObject.SetActive(false);
            closeButton.gameObject.SetActive(true);

            PanelAwake();
        }

        private void OnDestroy()
        {
            openButton.onClick.AddListener(OpenPanel);
            closeButton.onClick.AddListener(ClosePanel);

            PanelDestroy();
        }

        protected abstract void PanelAwake();
        protected abstract void PanelDestroy();

        protected virtual void OpenPanel()
        {
            openButton.gameObject.SetActive(false);
            closeButton.gameObject.SetActive(true);
        }

        protected virtual void ClosePanel()
        {
            openButton.gameObject.SetActive(true);
            closeButton.gameObject.SetActive(false);
        }
    }
}
