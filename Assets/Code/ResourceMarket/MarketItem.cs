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
        //новые переменные для новой формулы рынка
        [SerializeField] private double _exchangeCoef = 0.45;
        [SerializeField] private double _marketCoef = 0.05;
        [SerializeField] private int _marketAmount = 1;

        [Header("Item Texts Fields")]
        [SerializeField] private TMP_Text _amountText;
        [SerializeField] private TMP_Text _costText;
        //отображение количества рынков
        [SerializeField] private TMP_Text _marketAmountText;

        private int _exchangeAmount;
        private int _currentAmount;


       

        public int CurrentAmount => _currentAmount;
        //старая формула теперь работает как фиксированная цена покупки (т.е 100% стоимость продажи)
        public int BuyCost => _exchangeAmount * _exchangeRate;
        // новая формула подразумевает зависимость цены продажи от количества рынков
        public int ExchangeCost => _exchangeAmount * (int)(_exchangeRate * (_exchangeCoef + _marketCoef * _marketAmount));

        public void Init(int exchangeAmount)
        {
            _exchangeAmount = exchangeAmount;

            _currentAmount = _initialAmount;

            // разделение на продажу и покупку
            _costText.text = $"sell {_exchangeAmount} for {ExchangeCost} gold, (buy {_exchangeAmount} for {BuyCost})";
            UpdateAmountText(_currentAmount);

        }

        public void IncreaseAmount(int amount)
        {
            _currentAmount += amount;

            if (_currentAmount < _minAmount)
            {
                _currentAmount = _minAmount;
            }

            UpdateAmountText(_currentAmount);
        }

        public void DecreaseAmount(int amount)
        {
            _currentAmount -= amount;

            UpdateAmountText(_currentAmount);
        }

        // изменение количество маркетов для DebugUI 
        public void IncreaseMarketAmount()
        {
            if(_marketAmount <= 5)
            {
                _marketAmount++;
                UpdateMarketAmountTxt(_marketAmount);
            }

            
        }
        // изменение количество маркетов для DebugUI 
        public void DecreaseMarketAmount()
        {
            if (_marketAmount >= 2)
            {
                _marketAmount--;
                UpdateMarketAmountTxt(_marketAmount);
            }
        }

        private void UpdateAmountText(int currentAmount)
        {
            _amountText.text = currentAmount.ToString();
        }

        //апдейт текста на кол-во маркетов
        private void UpdateMarketAmountTxt(int _marketAmount)
        {
            _marketAmountText.text = _marketAmount.ToString();
            _costText.text = $"sell {_exchangeAmount} for {ExchangeCost} gold, (buy {_exchangeAmount} for {BuyCost})";
            UpdateAmountText(_currentAmount);
        }

    }
}

