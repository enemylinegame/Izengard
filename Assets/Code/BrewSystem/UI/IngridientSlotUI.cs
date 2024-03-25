using UnityEngine;
using UnityEngine.UI;

namespace BrewSystem.UI
{
    public class IngridientSlotUI : MonoBehaviour
    {
        [SerializeField]
        private Image _icon;
        [SerializeField]
        private Sprite _defaultSprite;
        [SerializeField]
        private Color _normalColor;
        [SerializeField]
        private Color _disableColor;

        public bool IsOccupied { get; private set; }

        public void InitUI()
        {
            Unset();
        }

        public void Set(Sprite iconSprite)
        {
            _icon.sprite = iconSprite;

            _icon.color = _normalColor;

            IsOccupied = true;
        }
        public void Unset()
        {
            _icon.sprite = _defaultSprite;

            _icon.color = _disableColor;
            
            IsOccupied = false;
        }
    }
}
