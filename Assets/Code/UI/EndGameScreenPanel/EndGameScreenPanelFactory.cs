using Core;
using UnityEngine;
using Utils;

namespace Code.UI
{
    public class EndGameScreenPanelFactory : UIViewFactory<EndGameScreenPanel>
    {
        public EndGameScreenPanelFactory(UIElementsConfig uIElementsConfig, Canvas mainCanvas) : base(uIElementsConfig, mainCanvas){}
    }
}