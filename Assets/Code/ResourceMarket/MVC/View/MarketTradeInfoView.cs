using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace ResourceMarket
{
    public class MarketTradeInfoView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _goldText;
        [SerializeField] private GameObject _tradeInfoPrefab;
        [SerializeField] private Transform _itemsContainer;

        private List<ItemTradeInfoView> _itemsTradeInfoList;

        public void InitView(IList<IMarketItem> marketItems)
        {
            _itemsTradeInfoList = new List<ItemTradeInfoView>();
            foreach (var item in marketItems)
            {
                _itemsTradeInfoList.Add(CreateItemView(item));
            }
        }

        private ItemTradeInfoView CreateItemView(IMarketItem item)
        {
            var objectView = Instantiate(_tradeInfoPrefab, _itemsContainer, false);
            var itemView = objectView.GetComponent<ItemTradeInfoView>();

            itemView.InitView(item);
            return itemView;
        }

        public void UpdateGold(int currentGold)
        {
            _goldText.text = $"{currentGold}";
        }
    }
}
