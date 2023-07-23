using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ResourceMarket
{
    public class ItemTradeInfoView : MonoBehaviour
    {
        [SerializeField] private Image _icon;
        [SerializeField] private TMP_Text _buyInfoText;
        [SerializeField] private TMP_Text _sellInfoText;

        public void InitView(IMarketItem marketItem)
        {
            _icon.sprite = marketItem.Data.Icon;
            _buyInfoText.text = $"{marketItem.BuyCost} G";
            _sellInfoText.text = $"{marketItem.ExchangeCost} G";
        }
    }
}
