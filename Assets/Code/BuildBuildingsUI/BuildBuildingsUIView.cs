using BuildingSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace Views.BuildBuildingsUI
{
    public class BuildBuildingsUIView : MonoBehaviour
    {
        [field: SerializeField] public Button OpenMenuButton { get; private set; }
        [field: SerializeField] public Button CloseMenuButton { get; private set; }
        [field: SerializeField] public Transform BuildButtonsHolder { get; private set; }

        [SerializeField] private Button _prefabButton;
        public readonly Dictionary<TypeOfBuildings, Button> ButtonsInMenu = new Dictionary<TypeOfBuildings, Button>();


        public void Init(GlobalBuildingsModels models)
        {
            var buildingsType = Enum.GetValues(typeof(TypeOfBuildings)).Cast<TypeOfBuildings>();

            foreach (var buildingType in buildingsType)
            {
                if (IsModelContainsBuldingType(buildingType, models))
                {
                    var button = Instantiate(_prefabButton, BuildButtonsHolder);
                    ButtonsInMenu.Add(buildingType, button);

                    SetButtonView(buildingType, button);
                }
            }
        }

        public void Deinit()
        {
            foreach (var kvp in ButtonsInMenu)
            {
                DestroyImmediate(kvp.Value);
            }
            ButtonsInMenu.Clear();
        }

        private void SetButtonView(TypeOfBuildings typeOfBuildings, Button button)
        {
            var text = button.GetComponentInChildren<TMP_Text>();
            if (text != null) text.text = typeOfBuildings.ToString();
        }

        private bool IsModelContainsBuldingType(TypeOfBuildings buildingType, GlobalBuildingsModels models)
        {
            if (models.FindItemMarketBuildingModel(buildingType) != null) return true;
            if (models.FindItemProduceBuildingModel(buildingType) != null) return true;
            if (models.FindResurseMarketBuildingModel(buildingType) != null) return true;
            if (models.FindResurseProduceBuildingModel(buildingType) != null) return true;
            return false;
        }
    }
}