using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ResourceMarket 
{
    public sealed class MarketManagerV2 : MonoBehaviour
    {
        [Header("Castomer Settings")]
        [SerializeField] private int _initialGold = 1000;
        [SerializeField] private TMP_Text _goldText;
        [SerializeField] private TMP_Text _statusText;

        [Space(10)]
        [Header("Market Items")]
        [SerializeField] private MarketItem _woodItem;
        [SerializeField] private MarketItem _ironItem;
        [SerializeField] private int _exchangeTierOneAmount = 10;

        [Space(10)]
        [Header("Market Buttons")]
        [SerializeField] private Button _buyWoodButton;
        [SerializeField] private Button _buyIronButton;
        [SerializeField] private Button _sellWoodButton;
        [SerializeField] private Button _sellIronButton;


        private int _currentGold;

        private void Start()
        {
            _currentGold = _initialGold;

            _woodItem.Init(_exchangeTierOneAmount);
            _ironItem.Init(_exchangeTierOneAmount);

            _buyWoodButton.onClick.AddListener(BuyWood);
            _buyIronButton.onClick.AddListener(BuyIron);
            _sellWoodButton.onClick.AddListener(SellWood);
            _sellIronButton.onClick.AddListener(SellIron);

            UpdateGoldText(_currentGold);
            ChangeStatusText("");
        }

        private void BuyWood()
        {
            if (_currentGold >= _woodItem.ExchangeCost)
            {
                _woodItem.IncreaseAmount(_exchangeTierOneAmount);
                _currentGold -= _woodItem.ExchangeCost;
                UpdateGoldText(_currentGold);
                ChangeStatusText("");
            }
            else
            {
                ChangeStatusText("Нужно больше золота");
            }
        }

        private void BuyIron()
        {
            if (_currentGold >= _ironItem.ExchangeCost)
            {
                _ironItem.IncreaseAmount(_exchangeTierOneAmount);
                _currentGold -= _ironItem.ExchangeCost;
                UpdateGoldText(_currentGold);
                ChangeStatusText("");
            }
            else
            {
                ChangeStatusText("Нужно больше золота");
            }
        }

        private void SellWood()
        {
            if (_woodItem.CurrentAmount > _exchangeTierOneAmount)
            {
                _woodItem.DecreaseAmount(_exchangeTierOneAmount);
                _currentGold += _woodItem.ExchangeCost;
                UpdateGoldText(_currentGold);
                ChangeStatusText("");
            }
            else
            {
                ChangeStatusText("Нужно больше древесины");
            }
        }

        private void SellIron()
        {
            if (_ironItem.CurrentAmount > _exchangeTierOneAmount)
            {
                _ironItem.DecreaseAmount(_exchangeTierOneAmount);
                _currentGold += _ironItem.ExchangeCost;
                UpdateGoldText(_currentGold);
                ChangeStatusText("");
            }
            else
            {
                ChangeStatusText("Нужно больше железа");
            }
        }

        private void UpdateGoldText(int currentGold)
        {
            _goldText.text = currentGold.ToString();
        }

        private void ChangeStatusText(string text)
        {
            _statusText.text = text;
        }
    }
}