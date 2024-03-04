using UnityEngine;

namespace UI
{
    public class BattlePanelFactory : UIViewFactory<BattlePanelUI>
    {
        public BattlePanelFactory(UIElementsConfig uIElementsConfig, Canvas mainCanvas) : base(uIElementsConfig, mainCanvas) { }
    }
}
