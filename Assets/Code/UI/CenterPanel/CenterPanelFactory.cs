using Core;
using UnityEngine;
using Utils;

namespace Code.UI
{
    public class CenterPanelFactory: UIViewFactory<CenterPanel>
    {
        public CenterPanelFactory(UIElementsConfig uIElementsConfig, Canvas mainCanvas) : base(uIElementsConfig, mainCanvas){}
    }
}