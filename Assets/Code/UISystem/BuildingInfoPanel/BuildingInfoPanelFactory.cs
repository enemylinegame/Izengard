using UnityEngine;

namespace UI
{
    public class BuildingInfoPanelFactory :UIViewFactory<BuildingInfoPanel>
    {
        public BuildingInfoPanelFactory(UIElementsConfig uIElementsConfig, Canvas mainCanvas) : base(uIElementsConfig, mainCanvas){}
    }
}