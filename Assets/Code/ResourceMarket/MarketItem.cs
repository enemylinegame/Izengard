using TMPro;
using UnityEngine;

namespace ResourceMarket 
{
    public class MarketItem : MonoBehaviour
    {
        [Header("Amount Settings")]
        [SerializeField] private int _initialAmount = 100;
        [SerializeField] private int _minAmount = 50;

        [Header("Cost Settings")]
        [SerializeField] private int _exchangeRate = 100;

        [Header("Item Texts Fields")]
        [SerializeField] private TMP_Text _amountText;
        [SerializeField] private TMP_Text _costText;

        private int _exchangeAmount;
        private int _currentAmount;

        public int CurrentAmount => _currentAmount;
        public int ExchangeCost => _exchangeAmount * _exchangeRate;

        public void Init(int exchangeAmount)
        {
            _exchangeAmount = exchangeAmount;

            _currentAmount = _initialAmount;
            _costText.text = $"{_exchangeAmount} for {ExchangeCost} gold";
            UpdateAmountText(_currentAmount);
        }

        public void IncreaseAmount(int amount)
        {
            _currentAmount -= amount;

            if (_currentAmount < _minAmount)
            {
                _currentAmount = _minAmount;
            }

            UpdateAmountText(_currentAmount);
        }

        public void DecreaseAmount(int amount)
        {
            _currentAmount += amount;

            UpdateAmountText(_currentAmount);
        }

        private void UpdateAmountText(int currentAmount)
        {
            _amountText.text = currentAmount.ToString();
        }
    }
}

