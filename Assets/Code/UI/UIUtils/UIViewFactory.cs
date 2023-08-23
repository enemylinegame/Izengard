using Core;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Utils
{
    public class UIViewFactory<T> where T : class
    {
        public UIElementsConfig UIElementsConfig { get; private set; }
        public Canvas MainCanvas { get; private set; }
        public UIViewFactory(UIElementsConfig uIElementsConfig, Canvas mainCanvas)
        {
            MainCanvas = mainCanvas;
            UIElementsConfig = uIElementsConfig;
        }
        public T GetView(GameObject reference, Transform parent)
        {
            var panelObject = Object.Instantiate(reference, parent);
            return panelObject.GetComponent<T>();
        }
        public T GetView(GameObject reference)
        {
            var panelObject = Object.Instantiate(reference, MainCanvas.transform);
            return panelObject.GetComponent<T>();
        }
    }
}
