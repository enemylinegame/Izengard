using System;
using System.Threading.Tasks;
using Code.BuildingSystem;
using Code.TileSystem;
using UnityEngine;
using UnityEngine.Events;

namespace Code.UI.CenterPanel
{
    public class CenterPanelController : ITextVisualizationOnUI, IPlayerNotifier
    {
        private CenterUI _view;
        private BaseNotificationUI _notification => _view.BaseNotificationUI;
        private TileSelectionView _tileSelection => _view.TIleSelection;
        public event Action CloseBuildingsBuy;
        
        public CenterPanelController(CenterPanelFactory factory)
        {
            _view = factory.GetView(factory.UIElementsConfig.CenterPanel);
            _view.CloseBuildingsBuy.onClick.AddListener((() =>
            {
                _view.BuildingBuy.gameObject.SetActive(false);
                CloseBuildingsBuy?.Invoke();
            }));
        }

        public void ActivateBuildingBuyUI()
        {
            _view.BuildingBuy.gameObject.SetActive(true);
            
        }
        public void DeactivateBuildingBuyUI()
        {
            _view.BuildingBuy.gameObject.SetActive(false);
            CloseBuildingsBuy?.Invoke();
            
        }
        

        public void ActivateTileTypeSelection(UnityAction<TileType, TileView> action, TileView view)
        {
            _tileSelection.gameObject.SetActive(true);
            _tileSelection.TileEco.onClick.AddListener((() => action(TileType.Eco, view)));
            _tileSelection.TileWar.onClick.AddListener((() => action(TileType.war, view)));
            _tileSelection.Back.onClick.AddListener(DeactivateTileTypeSelection);
            
        }
        
        public void DeactivateTileTypeSelection()
        {
            _tileSelection.gameObject.SetActive(false);
            _tileSelection.TileEco.onClick.RemoveAllListeners();
            _tileSelection.TileWar.onClick.RemoveAllListeners();
            _tileSelection.Back.onClick.RemoveAllListeners();
            
        }

        public Transform TransformBuildButtonsHolder()
        {
            return _view.BuildButtonsHolder;
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