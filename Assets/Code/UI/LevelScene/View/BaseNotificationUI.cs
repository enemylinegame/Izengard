using System;
using System.Threading.Tasks;
using Code.UI.LevelScene;
using ResourceSystem;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace Code
{
    public class BaseNotificationUI : MonoBehaviour, ITextVisualizationOnUI
    {
        [SerializeField]private TMP_Text _basicText;
        [SerializeField]private TMP_Text _secondaryText;

        private TimeRemaining _timeRemaining;
        
        //TODO Change "Async" on "TimeRemaining"
        public async Task BasicTemporaryUIVisualization(String text, int time)
        {
            _basicText.text = text.ToString();
            _basicText.gameObject.SetActive(true);
            await Task.Delay(translateTime(time));
            _basicText.gameObject.SetActive(false);
            _basicText.text = "";
        }
        
        public async Task SecondaryTemporaryUINotification(String text, int time)
        {
            _secondaryText.text = text.ToString();
            _secondaryText.gameObject.SetActive(true);
            await Task.Delay(translateTime(time));
            _secondaryText.gameObject.SetActive(false);
            _secondaryText.text = "";
        }
        
        public void BasicUIVisualization(String text, bool IsOn)
        {
            _basicText.text = text.ToString();
            _basicText.gameObject.SetActive(IsOn);
            _basicText.text = "";
        }

        public void SecondaryUINotification(String text, bool IsOn)
        {
            _secondaryText.text = text.ToString();
            _secondaryText.gameObject.SetActive(IsOn);
            _secondaryText.text = "";
        }
        /// <summary>
        /// Translate Time on seconds
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        private int translateTime(int time)
        {
            return time * 1000;
        }
    }
}