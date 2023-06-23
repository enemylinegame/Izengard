using System.Collections.Generic;
using Code.BuildingSystem;
using Code.TileSystem;
using UnityEngine;
using UnityEngine.UI;
using Views.BuildBuildingsUI;
using CombatSystem.Views;
using Code.Player;


namespace Code.UI
{
    public class UIController : IOnTile, ITileLoadInfo
    {
        private RightUI _rightUI;
        private BottomUI _bottomUI;
        private CenterUI _centerUI;
        private InputController _inputController;
        private WarsView _warsView;

        public RightUI RightUI => _rightUI;
        public BottomUI BottomUI => _bottomUI;
        public CenterUI CenterUI => _centerUI;
        public WarsView WarsView => _warsView;

        public List<BuildingConfig> ButtonsBuy = new List<BuildingConfig>();
        public Dictionary<BuildingConfig, Button> ButtonsInMenu = new Dictionary<BuildingConfig, Button>();
        public Dictionary<GameObject, BuildingUIInfo> DestroyBuildingInfo = new Dictionary<GameObject, BuildingUIInfo>();
        /// <summary>
        /// Главный контроллер UI
        /// </summary>
        public UIController(RightUI rightUI, BottomUI bottomUI, CenterUI centerUI, InputController inputController)
        {
            _rightUI = rightUI;
            _bottomUI = bottomUI;
            _centerUI = centerUI;
            _inputController = inputController;

            _warsView = new WarsView(bottomUI.WarsUIView, inputController);
            
            IsWorkUI(UIType.All, false);
            
            _bottomUI.BuildingMenu.PrefabButtonClear.onClick.AddListener(() => IsWorkUI(UIType.Buy, true));
            _bottomUI.BuildingMenu.PrefabButtonClear.onClick.AddListener(() => _bottomUI.BuildingMenu.PrefabButtonClear.gameObject.SetActive(false));
            _centerUI.CloseBuildingsBuy.onClick.AddListener(() => IsWorkUI(UIType.Buy, false));
            _centerUI.CloseBuildingsBuy.onClick.AddListener(() => _bottomUI.BuildingMenu.PrefabButtonClear.gameObject.SetActive(true));
            inputController.Add(this);
            
            CenterUI.TIleSelection.Back.onClick.AddListener(() => IsWorkUI(UIType.TileSel, false));
        }
        /// <summary>
        /// Включение/отключение любой части UI
        /// </summary>
        /// <param name="type">Тип UI</param>
        /// <param name="isOn"> Вкл/Откл</param>
        public void IsWorkUI(UIType type, bool isOn)
        {
            switch (type)
            {
                case UIType.All:
                    _centerUI.BuildingBuy.SetActive(isOn);
                    ClearButtonsUIBuy(isOn);
                    IsOnTileUI(isOn);
                    _inputController.LockRightClick = !isOn;
                    break;
                case UIType.Tile:
                    IsOnTileUI(isOn);
                    _inputController.LockRightClick = !isOn;
                    break;
                case UIType.Buy:
                    _inputController.LockRightClick = isOn;
                    _centerUI.BuildingBuy.SetActive(isOn);
                    break;
                case UIType.Сonfirmation:
                    break;
                case UIType.Unit:
                    break;
                case UIType.TileSel:
                    _inputController.LockRightClick = !isOn;
                    _centerUI.TIleSelection.gameObject.SetActive(isOn);
                    break;
                case UIType.Esc:
                    break;
            }
        }
        /// <summary>
        /// удаление кнопок для постройки зданий для загрузки другого тайла
        /// </summary>
        public void Deinit()
        {
            foreach (var kvp in ButtonsInMenu)
            {
                GameObject.Destroy(kvp.Value.gameObject);
            }
            ButtonsInMenu.Clear();
            
        }
        /// <summary>
        /// Удаление блока информации построенного здания для загрузки другого тайла
        /// </summary>
        private void ClearButtonsUIBuy(bool isOn)
        {
            if (isOn == false)
            {
                foreach (var kvp in DestroyBuildingInfo)
                {
                    GameObject.Destroy(kvp.Key.gameObject);
                }
                DestroyBuildingInfo.Clear();
                ButtonsBuy.Clear();
            }
            
        }

        private void IsOnTileUI(bool isOpen)
        {
            foreach (var window in _bottomUI.BuildingMenu.Windows) window.gameObject.SetActive(isOpen);
            _bottomUI.BuildingMenu.CloseMenuButton.gameObject.SetActive(isOpen);
        }

        public void LoadInfoToTheUI(TileView tile)
        {
            if(tile.TileModel.HouseType == HouseType.None) return;
            IsWorkUI(UIType.Tile, true);
        }

        public void Cancel()
        {
            IsWorkUI(UIType.All, false);
            Deinit();
            _bottomUI.TileUIView.Upgrade.onClick.RemoveAllListeners();
        }
    }

    public enum UIType
    {
        All,
        Buy,
        Сonfirmation,
        Tile,
        Unit,
        TileSel,
        Esc,
    }
}