using UnityEngine;

namespace Code.UI
{
    public class BuildingInfoPanelFactory :UIViewFactory<BuildingInfoPanel>
    {
        public BuildingInfoPanelFactory(UIElementsConfig uIElementsConfig, Canvas mainCanvas) : base(uIElementsConfig, mainCanvas){}
    }
}