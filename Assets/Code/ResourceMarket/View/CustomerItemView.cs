using ResourceSystem;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ResourceMarket
{
    public class CustomerItemView : MonoBehaviour
    {
        [SerializeField] private Image _icon;
        [SerializeField] private TMP_Text _itemAmountText;

        private ResourceType _resourceType;
        public ResourceType ResourceType => _resourceType;

        public void InitView(ResourceConfig config)
        {
            _icon.sprite = config.Icon;
            _resourceType = config.ResourceType;
        }

        public void ChangeAmount(int amount)
        {
            _itemAmountText.text = amount.ToString();
        }
    }
}
