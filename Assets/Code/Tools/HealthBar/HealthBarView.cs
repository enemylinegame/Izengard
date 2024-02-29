using UnityEngine;
using UnityEngine.UI;

namespace Tools
{
    public class HealthBarView : MonoBehaviour
    {
        [SerializeField]
        private Image _healthBarSprite;
        [SerializeField]
        private Color _fillColor;
        [SerializeField]
        private float _reduceSpeed = 2f;

        private Camera _mainCamera;

        public void Init()
        {
            _mainCamera = Camera.main;
            _healthBarSprite.color = _fillColor;
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        public void UpdateRotation()
        {
            var lookPos = transform.position - _mainCamera.transform.position;
            transform.rotation = Quaternion.LookRotation(lookPos);
        }

        public void UpdateHealthReduce(float targetValue)
        {
            _healthBarSprite.fillAmount
              = Mathf.MoveTowards(_healthBarSprite.fillAmount, targetValue, _reduceSpeed);
        }
    }
}
