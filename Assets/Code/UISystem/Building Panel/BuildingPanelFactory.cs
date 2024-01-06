using UnityEngine;

namespace UI
{
    public class BuildingPanelFactory : UIViewFactory<BuildingPanel>
    {
        public BuildingPanelFactory(UIElementsConfig uIElementsConfig, Canvas mainCanvas) : base(uIElementsConfig, mainCanvas){}
    }
}