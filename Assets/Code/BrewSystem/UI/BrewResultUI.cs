using TMPro;
using UnityEngine;

namespace BrewSystem.UI
{
    public class BrewResultUI : MonoBehaviour
    {
        [SerializeField]
        private GameObject[] _starResults;
        [SerializeField]
        private TMP_Text _resultDescription;

        public void InitUI()
        {
            ResetStars();

            _resultDescription.text = "";
        }

        public void ShowResult(BrewResultType resultType)
        {
            switch (resultType)
            {
                default: 
                    {
                        ShowStarsByCount(0);
                        _resultDescription.text = "YOU LOSE!";
                        break; 
                    }
                case BrewResultType.Low: 
                    {
                        ShowStarsByCount(1);
                        _resultDescription.text = "BAD";
                        break; 
                    }
                case BrewResultType.Normal:
                    {
                        ShowStarsByCount(2);
                        _resultDescription.text = "MEDIUM";
                        break;
                    }
                case BrewResultType.Ideal:
                    {
                        ShowStarsByCount(3);
                        _resultDescription.text = "COOL";
                        break;
                    }
            }
        }

        private void ShowStarsByCount(int value)
        {
            ResetStars();

            for (int i = 0; i < value; i++)
            {
                _starResults[i].SetActive(true);
            }
        }

        private void ResetStars()
        {
            for (int i = 0; i < _starResults.Length; i++)
            {
                _starResults[i].SetActive(false);
            }
        }
    }
}
