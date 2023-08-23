using Core;
using UnityEngine;
using Utils;

namespace Code.UI.CenterPanel
{
    public class CenterPanelFactory: UIViewFactory<CenterUI>
    {
        public CenterPanelFactory(UIElementsConfig uIElementsConfig, Canvas mainCanvas) : base(uIElementsConfig, mainCanvas){}
    }
}