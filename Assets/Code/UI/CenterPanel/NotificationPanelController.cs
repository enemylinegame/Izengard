using System;
using System.Threading.Tasks;

namespace Code.UI
{
    public class NotificationPanelController : ITextVisualizationOnUI, IPlayerNotifier
    {
        private readonly NotificationPanel _notification;

        public NotificationPanelController(NotificationPanel view)
        {
            _notification = view;
        }
        
        public async Task BasicTemporaryUIVisualization(String text, int time)
        {
            _notification.BasicText.text = text.ToString();
            _notification.BasicText.gameObject.SetActive(true);
            await Task.Delay(MsToSec(time));
            _notification.BasicText.gameObject.SetActive(false);
            _notification.BasicText.text = "";
        }
        
        public async Task SecondaryTemporaryUINotification(String text, int time)
        {
            _notification.SecondaryText.text = text.ToString();
            _notification.SecondaryText.gameObject.SetActive(true);
            await Task.Delay(MsToSec(time));
            _notification.SecondaryText.gameObject.SetActive(false);
            _notification.SecondaryText.text = "";
        }
        
        public void BasicUIVisualization(String text, bool IsOn)
        {
            _notification.BasicText.text = text.ToString();
            _notification.BasicText.gameObject.SetActive(IsOn);
            _notification.BasicText.text = "";
        }

        public void SecondaryUINotification(String text, bool IsOn)
        {
            _notification.SecondaryText.text = text.ToString();
            _notification.SecondaryText.gameObject.SetActive(IsOn);
            _notification.SecondaryText.text = "";
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