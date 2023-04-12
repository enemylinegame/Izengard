using BuildingSystem;
using System.Collections.Generic;
using Code;
using Code.TileSystem;
using Controllers.BuildBuildingsUI;
using ResourceSystem;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;


namespace Views.BuildBuildingsUI
{
    public class BuildingsUIView : MonoBehaviour
    {
        [field: SerializeField] public Button CloseMenuButton { get; private set; }
        [field: SerializeField] public Button PrefabButtonClear { get; private set; }
        [field: SerializeField] public GameObject BuildingInfo { get; private set; }
        [field: SerializeField] public Transform[] Windows { get; private set; }
        [field: SerializeField] public Transform BuildButtonsHolder { get; set; }
        [field: SerializeField] public Transform ByBuildButtonsHolder { get; set; }

        [SerializeField] private Button _buyPrefabButton;
        public Dictionary<BuildingConfig, Button> ButtonsInMenu = new Dictionary<BuildingConfig, Button>();
        public Dictionary<GameObject, BuildingUIInfo> DestroyBuildingInfo = new Dictionary<GameObject, BuildingUIInfo>();
        public List<BuildingConfig> ButtonsBuy = new List<BuildingConfig>();


        public void Init(List<BuildingConfig> models)
        {
            foreach (var building in models)
            {
                var button = Instantiate(_buyPrefabButton, BuildButtonsHolder);
                ButtonsInMenu.Add(building, button);
                CreateButtonUI(building, button);
            }
        }

        public BuildingUIInfo CreateBuildingInfo(BuildingConfig config, TileUIController controller, Building building)
        {
            var button = Instantiate(BuildingInfo, ByBuildButtonsHolder);
            var view = button.GetComponent<BuildingUIInfo>();
            
            view.Icon.sprite = config.Icon;
            view.Type.text = config.BuildingType.ToString();
            view.Types = config.BuildingType;
            view.UnitsBusy.text = view.Units +"/5";
            DestroyBuildingInfo.Add(button, view);
            view.DestroyBuildingInfo.onClick.AddListener((() => DestroyBuildingAndInfo(controller.DestroyBuilding, view, controller.View)));
            view.PlusUnit.onClick.AddListener((() => view.Hiring(true, controller, building)));
            view.MinusUnit.onClick.AddListener((() => view.Hiring(false, controller, building)));
            return view;
        }
        public BuildingUIInfo LoadBuildingInfo(Building building, int Units, Dictionary<GameObject, BuildingConfig> buildingConfigs, TileUIController controller)
        {
            var button = Instantiate(BuildingInfo, ByBuildButtonsHolder);
            var view = button.GetComponent<BuildingUIInfo>();
            view.Icon.sprite = building.Icon.sprite;
            view.Type.text = building.Type.ToString();
            view.Types = building.Type;
            view.UnitsBusy.text = Units +"/5";
            view.Units = Units;
            DestroyBuildingInfo.Add(button, view);
            view.DestroyBuildingInfo.onClick.AddListener((() => DestroyBuildingAndInfo(buildingConfigs, view, controller.View)));
            view.PlusUnit.onClick.AddListener((() => view.Hiring(true, controller, building)));
            view.MinusUnit.onClick.AddListener((() => view.Hiring(false, controller, building)));
            return view;
        }

        public void Deinit()
        {
            foreach (var kvp in ButtonsInMenu)
            {
                Destroy(kvp.Value.gameObject);
            }
            ButtonsInMenu.Clear();
            
        }

        public void ClearButtonsUIBuy()
        {
            foreach (var kvp in DestroyBuildingInfo)
            {
                Destroy(kvp.Key.gameObject);
            }
            DestroyBuildingInfo.Clear();
            ButtonsBuy.Clear();
        }

        public void DestroyBuildingAndInfo(Dictionary<GameObject, BuildingConfig> buildingConfigs, BuildingUIInfo Button, TileView view)
        {
            foreach (var kvp in buildingConfigs)
            {
                if (kvp.Value.BuildingType == Button.Types)
                {
                    Button.DestroyBuildingInfo.onClick.RemoveAllListeners();
                    Button.PlusUnit.onClick.RemoveAllListeners();
                    Button.MinusUnit.onClick.RemoveAllListeners();
                    buildingConfigs.Remove(kvp.Key);
                    DestroyBuildingInfo.Remove(Button.gameObject);
                    view.FloodedBuildings.Remove(kvp.Key.GetComponent<Building>());
                    Destroy(kvp.Key.gameObject);
                    Destroy(Button.gameObject);
                    break;
                }
                
            }
        }

        private void CreateButtonUI(BuildingConfig buildingConfig, Button button)
        {
            var view = button.GetComponent<BuildButtonView>();
            if (view != null) 
            {
                view.BuildingName.text = buildingConfig.BuildingType.ToString();
                foreach (var cost in buildingConfig.BuildingCost)
                {
                    view.CostForBuildingsUI.text += cost.ResourceType + ":" + cost.Cost + " ";
                }
                view.Description.text = buildingConfig.Description;
                view.Icon.sprite = buildingConfig.Icon;
            }
            else
            {
                Debug.LogError("Button field is empty");
            }
        }
    }
}