using UnityEngine;
using TMPro;

namespace ResourceMarket 
{
    public class MarketManager : MonoBehaviour
    {
        [Header("ResourceList")]
        public int wood;
        public int iron;
        public int deer;
        public int horse;
        public int textile;
        public int magicStone;
        public int gold;
        public int workers;


        [Header("Variables")]
        public int exchangeAmount;
        public int woodExchangeRate;
        public int ironExchangeRate;
        public int marketCount;


        [Header("TMP Stock")]
        [SerializeField]
        public TMP_Text errorTxt;
        [SerializeField]
        public TMP_Text goldCount;
        [SerializeField]
        private TMP_Text woodCount;
        [SerializeField]
        private TMP_Text ironCount;

        [Header("TMP Costs")]
        [SerializeField]
        public TMP_Text woodCost;
        [SerializeField]
        public TMP_Text ironCost;

        void Start()
        {
            woodCost.text = exchangeAmount.ToString() + "  for " + (woodExchangeRate * exchangeAmount).ToString() + " gold";
            ironCost.text = exchangeAmount.ToString() + "  for " + (ironExchangeRate * exchangeAmount).ToString() + " gold";
            errorTxt.text = "";
        }
        void Update()
        {
            goldCount.text = gold.ToString();
            woodCount.text = wood.ToString();
            ironCount.text = iron.ToString();

        }
        public void SellWood()
        {
            if (wood >= exchangeAmount)
            {
                gold += exchangeAmount * woodExchangeRate;
                wood -= exchangeAmount;
                errorTxt.text = "";
            }
            else
            {
                Debug.Log("Нужно больше древесины");
                errorTxt.text = "Нужно больше древесины";
            }

        }
        public void SellIron()
        {
            if (iron >= exchangeAmount)
            {
                gold += exchangeAmount * ironExchangeRate;
                iron -= exchangeAmount;
                errorTxt.text = "";
            }
            else
            {
                Debug.Log("Нужно больше железа");
                errorTxt.text = "Нужно больше железа";
            }

        }
        public void buyWood()
        {
            if (gold >= exchangeAmount * woodExchangeRate)
            {
                gold -= exchangeAmount * woodExchangeRate;
                wood += exchangeAmount;
                errorTxt.text = "";
            }
            else
            {
                Debug.Log("Нужно больше золота");
                errorTxt.text = "Нужно больше золота";
            }


        }
        public void buyIron()
        {
            if (gold >= exchangeAmount * ironExchangeRate)
            {
                gold -= exchangeAmount * ironExchangeRate;
                iron += exchangeAmount;
                errorTxt.text = "";
            }

            else
            {
                Debug.Log("Нужно больше золота");
                errorTxt.text = "Нужно больше золота";
            }

        }
    }
}
