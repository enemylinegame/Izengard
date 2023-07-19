using System;
using System.Threading.Tasks;
using Code.UI;
using TMPro;
using UnityEngine;

namespace Code
{
    public sealed class BaseNotificationUI : MonoBehaviour, ITextVisualizationOnUI, IPlayerNotifier
    {
        [SerializeField]private TMP_Text _basicText;
        [SerializeField]private TMP_Text _secondaryText;
        
        //TODO Change "Async" on "TimeRemaining" it example "BaseUnitWaitHandler"
        public async Task BasicTemporaryUIVisualization(String text, int time)
        {
            _basicText.text = text.ToString();
            _basicText.gameObject.SetActive(true);
            await Task.Delay(MsToSec(time));
            _basicText.gameObject.SetActive(false);
            _basicText.text = "";
        }
        
        public async Task SecondaryTemporaryUINotification(String text, int time)
        {
            _secondaryText.text = text.ToString();
            _secondaryText.gameObject.SetActive(true);
            await Task.Delay(MsToSec(time));
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
        /// <param name="timeMs"></param>
        /// <returns></returns>
        private int MsToSec(int timeMs)
        {
            return timeMs * 1000;
        }

        public void Notify(string message)
        {
            BasicTemporaryUIVisualization(message, 
                GameContants.NOTIFICATION_TIME_SEC);
        }
    }
}